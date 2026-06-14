using GymManagement.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers;

public class MembersController : Controller
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var members = await _memberService.GetAllMembersAsync(ct);
        return View(members);
    }

    [HttpGet]
    public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
    {
        //Get Member by ID
        var result = await _memberService.GetMemberDetailsAsync(id, ct);

        if(result is null)
        {
            TempData["ErrorMessage"] = "Member Not Found !!";
            return RedirectToAction("Index");
        }

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
    {
        //Get health member
        var result = await _memberService.GetMemberHealthRecordAsync(id, ct);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Health Record is not found !!";
            return RedirectToAction("Index");
        }

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _memberService.CreateMemberAsync(model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Member!";
            }

            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditMember(int id, CancellationToken ct)
    {
        //Get data by Id
        var result = await _memberService.GetMemberToUpdateAsync(id, ct);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Member is not found to Update !!";
            return RedirectToAction("Index");
        }

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid) //server side validation
        {
            var result = await _memberService.UpdateMemberAsync(id, model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Member!";
            }

            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        //check if member exist
        var member = await _memberService.GetMemberDetailsAsync(id, ct);

        if (member is null)
        {
            TempData["ErrorMessage"] = "Member Not Found !!";
            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
    {
        var result = await _memberService.DeleteMemberAsync(id, ct);

        if (result)
        {
            TempData["SuccessMessage"] = "Member Deleted Successfully !";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed To Delete Member !";
        }

        return RedirectToAction("Index");
    }


}

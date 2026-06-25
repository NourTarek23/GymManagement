using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers;

public class SessionsController : Controller
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var result = await _sessionService.GetAllSessionsAsync(ct);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct = default)
    {
        var result = await _sessionService.GetSessionByIdAsync(id, ct);

        if (result.success)
        {
            return View(result.value);
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct = default)
    {
        ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersAsync(ct), "Id", "Name"); 
        ViewBag.Categories = new SelectList(await _sessionService.GetAllCategoriesAsync(ct), "Id", "CategoryName"); 

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct = default)
    {
        if (ModelState.IsValid)
        {
            var result = await _sessionService.CreateSessionAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
            }

            return RedirectToAction(nameof(Index));
        }
        ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersAsync(ct), "Id", "Name");
        ViewBag.Categories = new SelectList(await _sessionService.GetAllCategoriesAsync(ct), "Id", "CategoryName");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
    {
        var model = await _sessionService.GetSessionToUpdateAsync(id, ct);


        if(model.success)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersAsync(ct), "Id", "Name"); 
            return View(model.value);
        }
        else
        {
            TempData["ErrorMessage"] = model.error;
            return RedirectToAction(nameof(Index));
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SessionToUpdateViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) 
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetAllTrainersAsync(ct), "Id", "Name");
            return View(model); 
        }
       
        var result = await _sessionService.UpdateSessionAsync(id, model, ct);

        if (result.success)
        {
            TempData["SuccessMessage"] = "Session Updated Successfully !";
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        var result = await _sessionService.GetSessionByIdAsync(id, ct);

        if (result.success)
        {
            return View();
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
    {
        var result = await _sessionService.DeleteSessionAsync(id, ct);

        if (result.success)
        {
            TempData["SuccessMessage"] = "Session Deleted Successfully !";
        }
        else
        {
            TempData["ErrorMessage"] = result.error;
        }

        return RedirectToAction("Index");
    }


}

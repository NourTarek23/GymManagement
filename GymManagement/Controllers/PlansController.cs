using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace GymManagement.Controllers;

public class PlansController : Controller
{
    private readonly IPlanService _planService;

    public PlansController(IPlanService planService)
    {
        _planService = planService;
    }


    //Index
    //GET: /Plans/Index --> Index --> List all Plans
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var plans = await _planService.GetAllPlansAsync(ct);

        return View(plans);
    }

    //Details
    //GET: /Plans/Details/1
    [HttpGet]
    public async Task<IActionResult> Details(int id ,CancellationToken ct = default)
    {
        var result = await _planService.GetPlanDetailsAsync(id, ct);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Plan is not Found to View !!";
            return RedirectToAction(nameof(Index));
        }

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await _planService.GetPlanToUpdateAsync(id, ct);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Plan is not Found to Update !!";
            return RedirectToAction(nameof(Index));
        }

        return View(result);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, PlanToUpdateViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _planService.UpdatePlanAsync(id, model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = $"{model.Name} Updated Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed To Update {model.Name} !";
            }

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var result = await _planService.UpdatePlanStatusAsync(id, ct);

        if (result)
        {
            TempData["SuccessMessage"] = "Plan Status Changed !";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed To Change Plan Status !";
        }

        return RedirectToAction(nameof(Index));
    }




}

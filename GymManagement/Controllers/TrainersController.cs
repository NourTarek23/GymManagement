using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Trainers;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers;

public class TrainersController : Controller
{
    private readonly ITrainerService _trainerService;

    public TrainersController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await _trainerService.GetAllTrainersAsync(ct);

        return View(result);
    }

    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var result = await _trainerService.GetTrainerDetailsAsync(id, ct);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Trainer Not Found !!";
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
    public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _trainerService.CreateTrainerAsync(model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Trainer!";
            }

            return RedirectToAction("Index");
        }

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await _trainerService.GetTrainerToUpdateAsync(id, ct);
        if (result is null)
        {
            TempData["ErrorMessage"] = "Trainer Is Not Found To Update !!";
            return RedirectToAction(nameof(Index));
        }

        return View(result);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _trainerService.UpdateTrainerAsync(id, model, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer !";
            }

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);

        if (trainer is null)
        {
            TempData["ErrorMessage"] = "Member Not Found !!";
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> DeleteConfirmed(int id, CancellationToken ct)
    {
        var result = await _trainerService.DeleteTrainerAsync(id, ct);

        if (result)
        {
            TempData["SuccessMessage"] = "Trainer Deleted Successfully !";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed To Delete Trainer !";
        }

        return RedirectToAction(nameof(Index));
    }


}

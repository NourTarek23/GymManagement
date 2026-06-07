using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers;

public class PlansController : Controller
{
    private readonly IPlanRepository _planRepository;


    public PlansController(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }


    //Index
    //GET: /Plans/Index --> Index --> List all Plans
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var plans = await _planRepository.GetAllAsync(ct: ct);

        return View(plans);
    }

    //Details
    //GET: /Plans/Details/1
    public async Task<IActionResult> Details(int id ,CancellationToken ct = default)
    {
        var plan = await _planRepository.GetByIdAsync(id ,ct);

        if (plan is null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

}

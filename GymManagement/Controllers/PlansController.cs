using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Controllers;

public class PlansController : Controller
{
    private readonly GymDbContext _context = new GymDbContext();

    //Index
    //GET: /Plans/Index --> Index --> List all Plans
    public async Task<IActionResult> Index()
    {
        var plans = await _context.Plans.ToListAsync();

        return View(plans);
    }

    //Details
    //GET: /Plans/Details/1
    public async Task<IActionResult> Details(int id)
    {
        var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == id);

        if (plan is null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

}

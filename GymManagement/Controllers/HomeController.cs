using GymManagement.BLL.Services.Interfaces;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAnalyticService _analyticService;

    public HomeController(ILogger<HomeController> logger, IAnalyticService analyticService)
    {
        _logger = logger;
        _analyticService = analyticService;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var model = await _analyticService.GetAnalyticsAsync(ct);

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

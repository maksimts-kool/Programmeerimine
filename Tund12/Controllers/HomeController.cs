using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tund12.Data;
using Tund12.Models;

namespace Tund12.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var täna = DateTime.Today;

        // Kõik käimasolevad koolitused
        var käimasolevad = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Opetaja)
            .Include(k => k.Registreerimised)
            .Where(k => k.AlgusKuupaev <= täna && k.LoppKuupaev >= täna)
            .OrderBy(k => k.LoppKuupaev)
            .Take(3)
            .ToListAsync();

        ViewBag.Käimasolevad = käimasolevad;

        // Kui kasutaja on sisse logitud, näita tema koolitusi
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = _userManager.GetUserId(User);
            var omasid = await _db.Registreerimised
                .Include(r => r.Koolitus)
                    .ThenInclude(k => k!.Keelekursus)
                .Where(r => r.ApplicationUserId == userId && r.Staatus == RegistreerimiseStaatus.Kinnitatud)
                .Select(r => r.Koolitus)
                .Where(k => k != null)
                .OrderBy(k => k!.AlgusKuupaev)
                .ToListAsync();

            ViewBag.MinuKoolitused = omasid;
        }

        return View();
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

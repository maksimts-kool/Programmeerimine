using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tund13.Data;
using Tund13.Models;

namespace Tund13.Controllers;

[Authorize(Roles = "Opetaja")]
public class OpetajaController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public OpetajaController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Otsi praeguse kasutaja õpetajaprofiil
    private async Task<Opetaja?> GetMinuProfiilAsync()
    {
        var userId = _userManager.GetUserId(User)!;
        return await _db.Opetajad.FirstOrDefaultAsync(o => o.ApplicationUserId == userId);
    }

    // GET /Opetaja – töölaud: minu koolitused
    public async Task<IActionResult> Index()
    {
        var opetaja = await GetMinuProfiilAsync();
        if (opetaja == null)
            return View("ProfiilPuudub");

        var koolitused = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Registreerimised)
            .Where(k => k.OpetajaId == opetaja.Id)
            .OrderBy(k => k.AlgusKuupaev)
            .ToListAsync();

        ViewBag.OpetajaProfiil = opetaja;
        return View(koolitused);
    }

    // GET /Opetaja/Opilased/5 – osalejate nimekiri konkreetsel koolitusel
    public async Task<IActionResult> Opilased(int id)
    {
        var opetaja = await GetMinuProfiilAsync();
        if (opetaja == null) return Forbid();

        var koolitus = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Registreerimised)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(k => k.Id == id && k.OpetajaId == opetaja.Id);

        if (koolitus == null) return NotFound();

        return View(koolitus);
    }
}

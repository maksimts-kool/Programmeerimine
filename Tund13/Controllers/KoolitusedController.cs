using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tund13.Data;
using Tund13.Models;

namespace Tund13.Controllers;

public class KoolitusedController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public KoolitusedController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // GET /Koolitused – avalik nimekiri
    public async Task<IActionResult> Index()
    {
        var koolitused = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Opetaja)
            .Include(k => k.Registreerimised)
            .OrderBy(k => k.AlgusKuupaev)
            .ToListAsync();

        return View(koolitused);
    }

    // GET /Koolitused/Details/5 – detailvaade
    public async Task<IActionResult> Details(int id)
    {
        var koolitus = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Opetaja)
            .Include(k => k.Registreerimised)
            .FirstOrDefaultAsync(k => k.Id == id);

        if (koolitus == null) return NotFound();

        // Kas praegune kasutaja on juba registreerunud?
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.OnRegistreerunud = koolitus.Registreerimised
                .Any(r => r.ApplicationUserId == userId &&
                          r.Staatus != RegistreerimiseStaatus.Tuhistatud);
        }

        return View(koolitus);
    }

    // POST /Koolitused/Registreeru/5 – registreeri sisseloginud õpilane
    [Authorize(Roles = "Opilane")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registreeru(int id)
    {
        var koolitus = await _db.Koolitused
            .Include(k => k.Registreerimised)
            .FirstOrDefaultAsync(k => k.Id == id);

        if (koolitus == null) return NotFound();

        var userId = _userManager.GetUserId(User)!;

        // Duplikaat-kontroll: sama õpilane, sama koolitus
        bool onJubaRegistreerunud = await _db.Registreerimised
            .AnyAsync(r => r.KoolitusId == id &&
                           r.ApplicationUserId == userId &&
                           r.Staatus != RegistreerimiseStaatus.Tuhistatud);

        if (onJubaRegistreerunud)
        {
            TempData["Viga"] = "Sa oled juba sellele koolitusele registreerunud.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Grupp täis?
        int osavõtjaidArv = koolitus.Registreerimised
            .Count(r => r.Staatus != RegistreerimiseStaatus.Tuhistatud);

        if (osavõtjaidArv >= koolitus.MaxOsalejaid)
        {
            TempData["Viga"] = "Kahjuks on grupp täis.";
            return RedirectToAction(nameof(Details), new { id });
        }

        _db.Registreerimised.Add(new Registreerimine
        {
            KoolitusId = id,
            ApplicationUserId = userId,
            RegistreerimiseAeg = DateTime.Now,
            Staatus = RegistreerimiseStaatus.Ootel
        });

        await _db.SaveChangesAsync();
        TempData["Teade"] = "Registreerimine edukalt saadetud! Ootab kinnitust.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // POST /Koolitused/TuhistaRegistreerimine/5
    [Authorize(Roles = "Opilane")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TuhistaRegistreerimine(int id)
    {
        var userId = _userManager.GetUserId(User)!;
        var reg = await _db.Registreerimised
            .FirstOrDefaultAsync(r => r.Id == id && r.ApplicationUserId == userId);

        if (reg == null) return NotFound();

        reg.Staatus = RegistreerimiseStaatus.Tuhistatud;
        await _db.SaveChangesAsync();

        TempData["Teade"] = "Registreerimine tühistatud.";
        return RedirectToAction(nameof(MinuKoolitused));
    }

    // GET /Koolitused/MinuKoolitused
    [Authorize(Roles = "Opilane")]
    public async Task<IActionResult> MinuKoolitused()
    {
        var userId = _userManager.GetUserId(User)!;
        var registreerimised = await _db.Registreerimised
            .Include(r => r.Koolitus)
                .ThenInclude(k => k!.Keelekursus)
            .Include(r => r.Koolitus)
                .ThenInclude(k => k!.Opetaja)
            .Where(r => r.ApplicationUserId == userId)
            .OrderByDescending(r => r.RegistreerimiseAeg)
            .ToListAsync();

        return View(registreerimised);
    }
}

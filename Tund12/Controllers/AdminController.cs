using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tund12.Data;
using Tund12.Models;

namespace Tund12.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // ═══════════════════════════════════════════════════════
    // TÖÖLAUD
    // ═══════════════════════════════════════════════════════
    public async Task<IActionResult> Index()
    {
        ViewBag.KursuseidKokku = await _db.Keelekursused.CountAsync();
        ViewBag.OpetajaidKokku = await _db.Opetajad.CountAsync();
        ViewBag.KoolitusidKokku = await _db.Koolitused.CountAsync();
        ViewBag.RegistreerimisteKokku = await _db.Registreerimised.CountAsync();
        ViewBag.OotelRegistreerimised = await _db.Registreerimised
            .CountAsync(r => r.Staatus == RegistreerimiseStaatus.Ootel);
        return View();
    }

    // ═══════════════════════════════════════════════════════
    // KEELEKURSUSED
    // ═══════════════════════════════════════════════════════
    public async Task<IActionResult> Keelekursused()
    {
        var list = await _db.Keelekursused
            .Include(k => k.Koolitused)
            .OrderBy(k => k.Keel).ThenBy(k => k.Tase)
            .ToListAsync();
        return View(list);
    }

    public IActionResult LisaKursus() => View(new Keelekursus());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LisaKursus(Keelekursus kursus)
    {
        if (!ModelState.IsValid) return View(kursus);
        _db.Keelekursused.Add(kursus);
        await _db.SaveChangesAsync();
        TempData["Teade"] = "Keelekursus lisatud.";
        return RedirectToAction(nameof(Keelekursused));
    }

    public async Task<IActionResult> MuudaKursus(int id)
    {
        var k = await _db.Keelekursused.FindAsync(id);
        if (k == null) return NotFound();
        return View(k);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MuudaKursus(Keelekursus kursus)
    {
        if (!ModelState.IsValid) return View(kursus);
        _db.Keelekursused.Update(kursus);
        await _db.SaveChangesAsync();
        TempData["Teade"] = "Keelekursus uuendatud.";
        return RedirectToAction(nameof(Keelekursused));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> KustutaKursus(int id)
    {
        var k = await _db.Keelekursused.FindAsync(id);
        if (k != null) { _db.Keelekursused.Remove(k); await _db.SaveChangesAsync(); }
        TempData["Teade"] = "Keelekursus kustutatud.";
        return RedirectToAction(nameof(Keelekursused));
    }

    // ═══════════════════════════════════════════════════════
    // ÕPETAJAD
    // ═══════════════════════════════════════════════════════
    public async Task<IActionResult> Opetajad()
    {
        var list = await _db.Opetajad
            .Include(o => o.User)
            .Include(o => o.Koolitused)
            .OrderBy(o => o.Nimi)
            .ToListAsync();
        return View(list);
    }

    public IActionResult LisaOpetaja() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LisaOpetaja(
        string nimi, string kvalifikatsioon,
        string email, string parool)
    {
        if (string.IsNullOrWhiteSpace(nimi) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(parool))
        {
            ModelState.AddModelError("", "Kõik väljad on kohustuslikud.");
            return View();
        }

        // Loo kasutajakonto
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, parool);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);
            return View();
        }

        await _userManager.AddToRoleAsync(user, "Opetaja");

        // Loo õpetaja profiil
        _db.Opetajad.Add(new Opetaja
        {
            Nimi = nimi,
            Kvalifikatsioon = kvalifikatsioon,
            ApplicationUserId = user.Id
        });

        await _db.SaveChangesAsync();
        TempData["Teade"] = $"Õpetaja {nimi} lisatud ja konto loodud.";
        return RedirectToAction(nameof(Opetajad));
    }

    public async Task<IActionResult> MuudaOpetaja(int id)
    {
        var o = await _db.Opetajad.Include(o => o.User).FirstOrDefaultAsync(x => x.Id == id);
        if (o == null) return NotFound();
        return View(o);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MuudaOpetaja(int id, string nimi, string kvalifikatsioon)
    {
        var o = await _db.Opetajad.FindAsync(id);
        if (o == null) return NotFound();

        if (string.IsNullOrWhiteSpace(nimi))
        {
            ModelState.AddModelError("Nimi", "Nimi on kohustuslik.");
            return View(o);
        }

        o.Nimi = nimi;
        o.Kvalifikatsioon = kvalifikatsioon;

        _db.Opetajad.Update(o);
        await _db.SaveChangesAsync();
        TempData["Teade"] = $"Õpetaja {nimi} andmed uuendatud.";
        return RedirectToAction(nameof(Opetajad));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> KustutaOpetaja(int id)
    {
        var o = await _db.Opetajad.Include(o => o.Koolitused).FirstOrDefaultAsync(x => x.Id == id);
        if (o == null) return NotFound();

        if (o.Koolitused != null && o.Koolitused.Any())
        {
            TempData["Teade"] = "Viga: Õpetajat ei saa kustutada, sest tal on seotud koolitusi.";
            return RedirectToAction(nameof(Opetajad));
        }

        _db.Opetajad.Remove(o);
        await _db.SaveChangesAsync();
        TempData["Teade"] = "Õpetaja profiil kustutatud.";
        return RedirectToAction(nameof(Opetajad));
    }

    // ═══════════════════════════════════════════════════════
    // KOOLITUSED
    // ═══════════════════════════════════════════════════════
    public async Task<IActionResult> Koolitused()
    {
        var list = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Opetaja)
            .Include(k => k.Registreerimised)
            .OrderBy(k => k.AlgusKuupaev)
            .ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> LisaKoolitus()
    {
        await LaadDropdownid();
        return View(new Koolitus { AlgusKuupaev = DateTime.Today, LoppKuupaev = DateTime.Today.AddMonths(2) });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LisaKoolitus(Koolitus koolitus)
    {
        ModelState.Remove("Keelekursus");
        ModelState.Remove("Opetaja");
        ModelState.Remove("Registreerimised");

        if (!ModelState.IsValid) { await LaadDropdownid(); return View(koolitus); }

        _db.Koolitused.Add(koolitus);
        await _db.SaveChangesAsync();
        TempData["Teade"] = "Koolitus lisatud.";
        return RedirectToAction(nameof(Koolitused));
    }

    public async Task<IActionResult> MuudaKoolitus(int id)
    {
        var k = await _db.Koolitused.FindAsync(id);
        if (k == null) return NotFound();
        await LaadDropdownid(k.KeelekursusId, k.OpetajaId);
        return View(k);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MuudaKoolitus(Koolitus koolitus)
    {
        ModelState.Remove("Keelekursus");
        ModelState.Remove("Opetaja");
        ModelState.Remove("Registreerimised");

        if (!ModelState.IsValid) { await LaadDropdownid(koolitus.KeelekursusId, koolitus.OpetajaId); return View(koolitus); }

        _db.Koolitused.Update(koolitus);
        await _db.SaveChangesAsync();
        TempData["Teade"] = "Koolitus uuendatud.";
        return RedirectToAction(nameof(Koolitused));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> KustutaKoolitus(int id)
    {
        var k = await _db.Koolitused.FindAsync(id);
        if (k != null) { _db.Koolitused.Remove(k); await _db.SaveChangesAsync(); }
        TempData["Teade"] = "Koolitus kustutatud.";
        return RedirectToAction(nameof(Koolitused));
    }

    // ═══════════════════════════════════════════════════════
    // REGISTREERIMISED
    // ═══════════════════════════════════════════════════════
    public async Task<IActionResult> Registreerimised()
    {
        var list = await _db.Registreerimised
            .Include(r => r.User)
            .Include(r => r.Koolitus)
                .ThenInclude(k => k!.Keelekursus)
            .OrderByDescending(r => r.RegistreerimiseAeg)
            .ToListAsync();
        return View(list);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MuudaStaatus(int id, RegistreerimiseStaatus staatus)
    {
        var reg = await _db.Registreerimised.FindAsync(id);
        if (reg != null) { reg.Staatus = staatus; await _db.SaveChangesAsync(); }
        TempData["Teade"] = $"Staatus muudetud: {staatus}.";
        return RedirectToAction(nameof(Registreerimised));
    }

    // ═══════════════════════════════════════════════════════
    // EMAIL ÕPILASTELE
    // ═══════════════════════════════════════════════════════
    public async Task<IActionResult> SaadaEmail(int koolitusId)
    {
        var koolitus = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Registreerimised)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(k => k.Id == koolitusId);

        if (koolitus == null) return NotFound();

        ViewBag.Koolitus = koolitus;
        ViewBag.Opilased = koolitus.Registreerimised
            .Where(r => r.Staatus == RegistreerimiseStaatus.Kinnitatud)
            .Select(r => r.User?.Email)
            .Where(e => e != null)
            .ToList();

        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaadaEmail(int koolitusId, string teema, string sisu)
    {
        var koolitus = await _db.Koolitused
            .Include(k => k.Keelekursus)
            .Include(k => k.Registreerimised)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(k => k.Id == koolitusId);

        if (koolitus == null) return NotFound();

        var aadressid = koolitus.Registreerimised
            .Where(r => r.Staatus == RegistreerimiseStaatus.Kinnitatud)
            .Select(r => r.User?.Email)
            .Where(e => e != null)
            .ToList();

        // Simuleerime saatmist (reaalses süsteemis SmtpClient / MailKit)
        foreach (var email in aadressid)
        {
            Console.WriteLine($"[EMAIL] → {email} | Teema: {teema} | Sisu: {sisu}");
        }

        TempData["Teade"] = $"E-kiri saadetud {aadressid.Count} õpilasele.";
        return RedirectToAction(nameof(Registreerimised));
    }

    // ─── Abi: lae dropdown-andmed ─────────────────────────────────
    private async Task LaadDropdownid(int? kursusId = null, int? opetajaId = null)
    {
        ViewBag.Keelekursused = new SelectList(
            await _db.Keelekursused.OrderBy(k => k.Nimetus).ToListAsync(),
            "Id", "Nimetus", kursusId);

        ViewBag.Opetajad = new SelectList(
            await _db.Opetajad.OrderBy(o => o.Nimi).ToListAsync(),
            "Id", "Nimi", opetajaId);
    }
}

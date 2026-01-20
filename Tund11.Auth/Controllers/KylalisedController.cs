using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Tund11.Auth.Data;
using Tund11.Auth;
using Tund11.Models;

namespace Tund11.Auth.Controllers;

public class KylalisedController(ApplicationDbContext context, IStringLocalizer<SharedResources> localizer) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly IStringLocalizer<SharedResources> _localizer = localizer;

    public async Task<IActionResult> Index(string filter = "all")
    {
        var query = _context.Kylalised
            .AsNoTracking()
            .Include(k => k.Pyha)
            .AsQueryable();

        if (filter == "willbe")
        {
            // Check only the LATEST response for this specific email and holiday
            query = query.Where(k => _context.Ankeetid
                .Where(a => a.Epost.ToLower() == k.Email.ToLower() && a.PyhaId == k.PyhaId)
                .OrderByDescending(a => a.LoomiseKupaev)
                .ThenByDescending(a => a.Id)
                .Select(a => (bool?)a.OnOsaleb)
                .FirstOrDefault() == true);
        }
        else if (filter == "wontbe")
        {
            // Check only the LATEST response for this specific email and holiday
            query = query.Where(k => _context.Ankeetid
                .Where(a => a.Epost.ToLower() == k.Email.ToLower() && a.PyhaId == k.PyhaId)
                .OrderByDescending(a => a.LoomiseKupaev)
                .ThenByDescending(a => a.Id)
                .Select(a => (bool?)a.OnOsaleb)
                .FirstOrDefault() == false);
        }

        var kylalised = await query.OrderBy(k => k.Nimi).ToListAsync();

        ViewData["CurrentFilter"] = filter;
        return View(kylalised);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var kylaline = await _context.Kylalised
            .AsNoTracking()
            .Include(k => k.Pyha)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (kylaline is null)
        {
            return NotFound();
        }

        // Get participation status from ankeet
        var ankeetResponse = await _context.Ankeetid
            .AsNoTracking()
            .Where(a => a.Epost.ToLower() == kylaline.Email.ToLower() && a.PyhaId == kylaline.PyhaId)
            .OrderByDescending(a => a.LoomiseKupaev)
            .FirstOrDefaultAsync();

        ViewData["ParticipationStatus"] = ankeetResponse?.OnOsaleb;

        return View(kylaline);
    }

    public async Task<IActionResult> Create()
    {
        var hasPyhad = await _context.Pyhad.AsNoTracking().AnyAsync();
        if (!hasPyhad)
        {
            TempData["Error"] = _localizer["Error_AddHolidayFirst"];
            return RedirectToAction(actionName: "Create", controllerName: "Pyhad");
        }

        await PopulatePyhadSelectList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nimi,Email,PyhaId")] Kylaline kylaline, string? participationStatus)
    {
        var pyhaExists = await _context.Pyhad.AsNoTracking().AnyAsync(p => p.Id == kylaline.PyhaId);
        if (!pyhaExists)
        {
            ModelState.AddModelError(nameof(Kylaline.PyhaId), _localizer["Validation_SelectHoliday"]);
        }

        if (!ModelState.IsValid)
        {
            await PopulatePyhadSelectList(kylaline.PyhaId);
            return View(kylaline);
        }

        _context.Add(kylaline);
        await _context.SaveChangesAsync();

        // Create ankeet response if participation status was set
        if (participationStatus != null)
        {
            var newAnkeet = new Ankeet
            {
                Nimi = kylaline.Nimi,
                Epost = kylaline.Email,
                PyhaId = kylaline.PyhaId,
                OnOsaleb = participationStatus == "willbe",
                LoomiseKupaev = DateTime.Today
            };
            _context.Ankeetid.Add(newAnkeet);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var kylaline = await _context.Kylalised.FindAsync(id);
        if (kylaline is null)
        {
            return NotFound();
        }

        var hasPyhad = await _context.Pyhad.AsNoTracking().AnyAsync();
        if (!hasPyhad)
        {
            TempData["Error"] = "Enne külalise muutmist lisa vähemalt üks püha.";
            return RedirectToAction(actionName: "Create", controllerName: "Pyhad");
        }

        // Get participation status from ankeet
        var ankeetResponse = await _context.Ankeetid
            .AsNoTracking()
            .Where(a => a.Epost.ToLower() == kylaline.Email.ToLower() && a.PyhaId == kylaline.PyhaId)
            .OrderByDescending(a => a.LoomiseKupaev)
            .FirstOrDefaultAsync();

        ViewData["ParticipationStatus"] = ankeetResponse?.OnOsaleb;

        await PopulatePyhadSelectList(kylaline.PyhaId);
        return View(kylaline);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nimi,Email,PyhaId")] Kylaline kylaline, string? participationStatus)
    {
        if (id != kylaline.Id)
        {
            return NotFound();
        }

        var pyhaExists = await _context.Pyhad.AsNoTracking().AnyAsync(p => p.Id == kylaline.PyhaId);
        if (!pyhaExists)
        {
            ModelState.AddModelError(nameof(Kylaline.PyhaId), _localizer["Validation_SelectHoliday"]);
        }

        if (!ModelState.IsValid)
        {
            await PopulatePyhadSelectList(kylaline.PyhaId);
            return View(kylaline);
        }

        try
        {
            _context.Update(kylaline);
            await _context.SaveChangesAsync();

            // Update or create ankeet response if participation status was set
            if (participationStatus != null)
            {
                var existingAnkeet = await _context.Ankeetid
                    .Where(a => a.Epost.ToLower() == kylaline.Email.ToLower() && a.PyhaId == kylaline.PyhaId)
                    .OrderByDescending(a => a.LoomiseKupaev)
                    .FirstOrDefaultAsync();

                bool onOsaleb = participationStatus == "willbe";

                if (existingAnkeet != null)
                {
                    existingAnkeet.OnOsaleb = onOsaleb;
                    _context.Update(existingAnkeet);
                }
                else
                {
                    var newAnkeet = new Ankeet
                    {
                        Nimi = kylaline.Nimi,
                        Epost = kylaline.Email,
                        PyhaId = kylaline.PyhaId,
                        OnOsaleb = onOsaleb,
                        LoomiseKupaev = DateTime.Today
                    };
                    _context.Ankeetid.Add(newAnkeet);
                }

                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!KylalineExists(kylaline.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var kylaline = await _context.Kylalised
            .AsNoTracking()
            .Include(k => k.Pyha)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (kylaline is null)
        {
            return NotFound();
        }

        // Get participation status from ankeet
        var ankeetResponse = await _context.Ankeetid
            .AsNoTracking()
            .Where(a => a.Epost.ToLower() == kylaline.Email.ToLower() && a.PyhaId == kylaline.PyhaId)
            .OrderByDescending(a => a.LoomiseKupaev)
            .FirstOrDefaultAsync();

        ViewData["ParticipationStatus"] = ankeetResponse?.OnOsaleb;

        return View(kylaline);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var kylaline = await _context.Kylalised.FindAsync(id);
        if (kylaline is null)
        {
            return RedirectToAction(nameof(Index));
        }

        _context.Kylalised.Remove(kylaline);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool KylalineExists(int id)
    {
        return _context.Kylalised.Any(e => e.Id == id);
    }

    private async Task PopulatePyhadSelectList(int? selectedId = null)
    {
        var pyhad = await _context.Pyhad
            .AsNoTracking()
            .OrderBy(p => p.Kuupaev)
            .ToListAsync();

        ViewData["PyhaId"] = new SelectList(pyhad, nameof(Pyha.Id), nameof(Pyha.Nimetus), selectedId);
    }
}

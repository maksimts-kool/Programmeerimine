using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tund11.Auth.Data;
using Tund11.Models;

namespace Tund11.Auth.Controllers;

public class KylalisedController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var query = _context.Kylalised
            .AsNoTracking()
            .Include(k => k.Pyha)
            .OrderBy(k => k.Nimi);

        return View(await query.ToListAsync());
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

        return View(kylaline);
    }

    public async Task<IActionResult> Create()
    {
        var hasPyhad = await _context.Pyhad.AsNoTracking().AnyAsync();
        if (!hasPyhad)
        {
            TempData["Error"] = "Enne külalise lisamist lisa vähemalt üks püha.";
            return RedirectToAction(actionName: "Create", controllerName: "Pyhad");
        }

        await PopulatePyhadSelectList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nimi,Email,OnKutse,PyhaId")] Kylaline kylaline)
    {
        var pyhaExists = await _context.Pyhad.AsNoTracking().AnyAsync(p => p.Id == kylaline.PyhaId);
        if (!pyhaExists)
        {
            ModelState.AddModelError(nameof(Kylaline.PyhaId), "Vali püha");
        }

        if (!ModelState.IsValid)
        {
            await PopulatePyhadSelectList(kylaline.PyhaId);
            return View(kylaline);
        }

        _context.Add(kylaline);
        await _context.SaveChangesAsync();
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

        await PopulatePyhadSelectList(kylaline.PyhaId);
        return View(kylaline);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nimi,Email,OnKutse,PyhaId")] Kylaline kylaline)
    {
        if (id != kylaline.Id)
        {
            return NotFound();
        }

        var pyhaExists = await _context.Pyhad.AsNoTracking().AnyAsync(p => p.Id == kylaline.PyhaId);
        if (!pyhaExists)
        {
            ModelState.AddModelError(nameof(Kylaline.PyhaId), "Vali püha");
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

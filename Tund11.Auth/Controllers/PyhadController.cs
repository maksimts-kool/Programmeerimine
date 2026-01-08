using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tund11.Auth.Data;
using Tund11.Models;

namespace Tund11.Auth.Controllers;

public class PyhadController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        return View(await _context.Pyhad.AsNoTracking().OrderBy(p => p.Kuupaev).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var pyha = await _context.Pyhad
            .AsNoTracking()
            .Include(p => p.Kylalised)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pyha is null)
        {
            return NotFound();
        }

        return View(pyha);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nimetus,Kuupaev")] Pyha pyha)
    {
        if (!ModelState.IsValid)
        {
            return View(pyha);
        }

        _context.Add(pyha);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var pyha = await _context.Pyhad.FindAsync(id);
        if (pyha is null)
        {
            return NotFound();
        }

        return View(pyha);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nimetus,Kuupaev")] Pyha pyha)
    {
        if (id != pyha.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(pyha);
        }

        try
        {
            _context.Update(pyha);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PyhaExists(pyha.Id))
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

        var pyha = await _context.Pyhad
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pyha is null)
        {
            return NotFound();
        }

        return View(pyha);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pyha = await _context.Pyhad.FindAsync(id);
        if (pyha is null)
        {
            return RedirectToAction(nameof(Index));
        }

        _context.Pyhad.Remove(pyha);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PyhaExists(int id)
    {
        return _context.Pyhad.Any(e => e.Id == id);
    }
}

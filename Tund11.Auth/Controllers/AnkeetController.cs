using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Tund11.Auth.Data;
using Tund11.Auth;
using Tund11.Models;

namespace Tund11.Auth.Controllers;

[Authorize]
public class AnkeetController(ApplicationDbContext context, IConfiguration configuration, IStringLocalizer<SharedResources> localizer) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly IStringLocalizer<SharedResources> _localizer = localizer;

    public IActionResult Index()
    {
        return RedirectToAction(nameof(Create));
    }

    public async Task<IActionResult> Create()
    {
        await PopulatePyhadSelectList();
        return View(new Ankeet { LoomiseKupaev = DateTime.Today, OnOsaleb = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nimi,LoomiseKupaev,Epost,PyhaId,OnOsaleb")] Ankeet ankeet)
    {
        if (!ModelState.IsValid)
        {
            await PopulatePyhadSelectList(ankeet.PyhaId);
            return View(ankeet);
        }

        // Validate email domain
        var emailDomainValid = await HomeController.ValidateEmailDomain(ankeet.Epost);
        if (!emailDomainValid)
        {
            ModelState.AddModelError("Epost", _localizer["Validation_EmailDomainInvalid"]);
            await PopulatePyhadSelectList(ankeet.PyhaId);
            return View(ankeet);
        }

        // Instead of just _context.Add(ankeet), check if they already responded
        var existingAnkeet = await _context.Ankeetid
            .FirstOrDefaultAsync(a => a.Epost.ToLower() == ankeet.Epost.ToLower() && a.PyhaId == ankeet.PyhaId);

        if (existingAnkeet != null)
        {
            existingAnkeet.OnOsaleb = ankeet.OnOsaleb;
            existingAnkeet.Nimi = ankeet.Nimi;
            existingAnkeet.LoomiseKupaev = DateTime.Today;
            _context.Update(existingAnkeet);
        }
        else
        {
            _context.Add(ankeet);
        }
        await _context.SaveChangesAsync();

        // Add to Kylalised if not already there
        var existingKylaline = await _context.Kylalised
            .FirstOrDefaultAsync(k => k.Email.ToLower() == ankeet.Epost.ToLower() && k.PyhaId == ankeet.PyhaId);

        if (existingKylaline is null)
        {
            var newKylaline = new Kylaline
            {
                Nimi = ankeet.Nimi,
                Email = ankeet.Epost,
                PyhaId = ankeet.PyhaId
            };
            _context.Kylalised.Add(newKylaline);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Thanks), routeValues: new { id = ankeet.Id });
    }

    public async Task<IActionResult> Thanks(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var ankeet = await _context.Ankeetid
            .AsNoTracking()
            .Include(a => a.Pyha)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (ankeet is null)
        {
            return NotFound();
        }

        // Send confirmation email
        await SendConfirmationEmail(ankeet);

        return View(ankeet);
    }

    private async Task SendConfirmationEmail(Ankeet ankeet)
    {
        try
        {
            var subject = $"Kinnitus: {ankeet.Pyha?.Nimetus}";
            var statusColor = ankeet.OnOsaleb ? "#28a745" : "#ffc107";
            var statusEmoji = ankeet.OnOsaleb ? "✅" : "ℹ️";
            var statusText = ankeet.OnOsaleb ? "Jah, tulen kindlasti" : "Ei, kahjuks ei saa tulla";

            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                </head>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='background-color: #667eea; padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                        <h1 style='color: white; margin: 0; font-size: 28px;'>Ankeet Kinnitatud!</h1>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px;'>
                        <h2 style='color: #667eea; margin-top: 0;'>Tere, {ankeet.Nimi}! 👋</h2>
                        <p style='font-size: 16px; margin-bottom: 25px;'>Täname vastamise eest ankeedile. Sinu vastus on edukalt salvestatud.</p>
                        
                        <div style='background-color: white; padding: 20px; border-radius: 8px; margin-bottom: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                            <h3 style='color: #667eea; margin-top: 0; font-size: 18px; border-bottom: 2px solid #667eea; padding-bottom: 10px;'>Ankeedi detailid</h3>
                            
                            <table style='width: 100%; border-collapse: collapse;'>
                                <tr>
                                    <td style='padding: 12px 0; border-bottom: 1px solid #e9ecef; font-size: 16px'><strong>🎉 Üritus:</strong></td>
                                    <td style='padding: 12px 0; border-bottom: 1px solid #e9ecef; text-align: right; font-size: 16px'>{ankeet.Pyha?.Nimetus}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 12px 0; border-bottom: 1px solid #e9ecef; font-size: 16px'><strong>📅 Kuupäev:</strong></td>
                                    <td style='padding: 12px 0; border-bottom: 1px solid #e9ecef; text-align: right; font-size: 16px'>{ankeet.Pyha?.Kuupaev:dd.MM.yyyy}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 12px 0; font-size: 16px'><strong>{statusEmoji} Sinu vastus:</strong></td>
                                    <td style='padding: 12px 0; text-align: right; font-size: 16px'>
                                        <span style='background-color: {statusColor}; color: white; padding: 5px 12px; border-radius: 20px; font-size: 14px; font-weight: bold;'>
                                            {statusText}
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        
                        <p style='color: #666; font-size: 14px; margin-top: 30px; padding-top: 20px; border-top: 1px solid #dee2e6;'>
                            Soovin sulle head päeva!<br>
                            <strong>Pikook Laod</strong>
                        </p>
                    </div>
                </body>
                </html>
            ";

            await HomeController.SendEmailAsync(_configuration, ankeet.Epost, subject, body);
        }
        catch (Exception)
        {
            // Log error but don't break the user flow
            // In production, you should log this properly
        }
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

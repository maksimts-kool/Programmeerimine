using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tund12.Data;
using Tund12.Models;
using Tund12.ViewModels;

namespace Tund12.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Kõigile sama avaleht - koolituste nimekiri
            var publicTrainings = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .Include(t => t.Registrations)
                .Where(t => t.AlgusKuupaev > DateTime.Now)
                .OrderBy(t => t.AlgusKuupaev)
                .Take(6)
                .ToListAsync();

            ViewBag.PublicTrainings = publicTrainings;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
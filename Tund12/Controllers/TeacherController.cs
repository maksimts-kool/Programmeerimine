using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tund12.Data;
using Tund12.Models;

namespace Tund12.Controllers
{
    [Authorize(Roles = "Opetaja")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeacherController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Teacher/Dashboard - Õpetaja töölaud
        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);

            // Leia õpetaja profiil
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.ApplicationUserId == userId);

            if (teacher == null)
            {
                return View("NoProfile");
            }

            // Leia õpetaja koolitused
            var trainings = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.User)
                .Where(t => t.TeacherId == teacher.Id)
                .OrderBy(t => t.AlgusKuupaev)
                .ToListAsync();

            ViewBag.TeacherName = teacher.Nimi;
            return View(trainings);
        }

        // GET: Teacher/Students/5 - Koolituse õpilased
        public async Task<IActionResult> Students(int id)
        {
            var userId = _userManager.GetUserId(User);
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.ApplicationUserId == userId);

            if (teacher == null) return NotFound();

            var training = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.TeacherId == teacher.Id);

            if (training == null) return NotFound();

            return View(training);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tund12.Data;
using Tund12.Models;

namespace Tund12.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Trainings - Avalik nimekiri
        public async Task<IActionResult> Index()
        {
            var trainings = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .Include(t => t.Registrations)
                .OrderBy(t => t.AlgusKuupaev)
                .ToListAsync();

            return View(trainings);
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var training = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (training == null) return NotFound();

            return View(training);
        }

        // GET: Trainings/Create - Ainult Admin
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Nimetus");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Nimi");
            return View();
        }

        // POST: Trainings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [Bind("CourseId,TeacherId,AlgusKuupaev,LoppKuupaev,Hind,MaxOsalejaid")]
            Training training)
        {
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Nimetus", training.CourseId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Nimi", training.TeacherId);
            return View(training);
        }

        // POST: Trainings/Register/5 - Õpilane registreerub
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Opilane")]
        public async Task<IActionResult> Register(int id)
        {
            var training = await _context.Trainings
                .Include(t => t.Registrations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            // Kontrolli, kas juba registreeritud
            var existingReg = await _context.Registrations
                .AnyAsync(r => r.TrainingId == id && r.ApplicationUserId == userId);

            if (existingReg)
            {
                TempData["Error"] = "Oled juba sellele koolitusele registreeritud!";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Kontrolli, kas grupp on täis
            if (training.Registrations.Count >= training.MaxOsalejaid)
            {
                TempData["Error"] = "Grupp on täis!";
                return RedirectToAction(nameof(Details), new { id });
            }

            var registration = new Registration
            {
                TrainingId = id,
                ApplicationUserId = userId!,
                Staatus = "Ootel",
                RegistreerimisAeg = DateTime.Now
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registreerimine õnnestus! Ootame kinnitust.";
            return RedirectToAction(nameof(MyTrainings));
        }

        // GET: Trainings/MyTrainings - Õpilase koolitused
        [Authorize(Roles = "Opilane")]
        public async Task<IActionResult> MyTrainings()
        {
            var userId = _userManager.GetUserId(User);
            var registrations = await _context.Registrations
                .Include(r => r.Training)
                    .ThenInclude(t => t.Course)
                .Include(r => r.Training)
                    .ThenInclude(t => t.Teacher)
                .Where(r => r.ApplicationUserId == userId)
                .OrderByDescending(r => r.RegistreerimisAeg)
                .ToListAsync();

            return View(registrations);
        }

        // POST: Trainings/CancelRegistration/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Opilane")]
        public async Task<IActionResult> CancelRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);

            if (registration == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (registration.ApplicationUserId != userId)
            {
                return Forbid();
            }

            registration.Staatus = "Tühistatud";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registreering tühistatud.";
            return RedirectToAction(nameof(MyTrainings));
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tund12.Data;
using Tund12.Models;
using Tund12.ViewModels;
using Tund12.Services;

namespace Tund12.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public AdminController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalCourses = await _context.Courses.CountAsync(),
                TotalTeachers = await _context.Teachers.CountAsync(),
                TotalTrainings = await _context.Trainings.CountAsync(),
                PendingRegistrations = await _context.Registrations
                    .CountAsync(r => r.Staatus == "Ootel"),
                TotalStudents = (await _userManager.GetUsersInRoleAsync("Opilane")).Count
            };

            ViewBag.Stats = stats;
            return View();
        }

        // GET: Admin/SendEmail/5
        [HttpGet]
        public async Task<IActionResult> SendEmail(int id)
        {
            var training = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Registrations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null) return NotFound();

            var studentCount = training.Registrations
                .Count(r => r.Staatus == "Kinnitatud");

            var model = new SendEmailViewModel
            {
                TrainingId = id,
                TrainingName = training.Course?.Nimetus ?? "Koolitus",
                StudentCount = studentCount
            };

            return View(model);
        }

        // POST: Admin/SendEmail/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(int id, SendEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var training = await _context.Trainings
                .Include(t => t.Course)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (training == null) return NotFound();

            // Leia kinnitatud õpilaste e-postid
            var studentEmails = training.Registrations
                .Where(r => r.Staatus == "Kinnitatud" && r.User != null)
                .Select(r => r.User.Email!)
                .ToList();

            if (!studentEmails.Any())
            {
                TempData["Error"] = "Sellel koolitusele pole kinnitatud õpilasi.";
                return RedirectToAction(nameof(Trainings));
            }

            try
            {
                // Loo HTML e-kiri
                var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #0066cc; color: white; padding: 20px; border-radius: 8px 8px 0 0; }}
        .content {{ background-color: #f9f9f9; padding: 20px; border-radius: 0 0 8px 8px; }}
        .footer {{ margin-top: 20px; padding-top: 20px; border-top: 1px solid #ddd; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>📧 {model.Subject}</h2>
            <p><strong>Kursus:</strong> {training.Course?.Nimetus}</p>
        </div>
        <div class=""content"">
            {model.Body.Replace("\n", "<br>")}
        </div>
        <div class=""footer"">
            <p>Keelekool | info@keelekool.ee</p>
            <p>See teade saadeti koolituse õpilastele: <strong>{training.Course?.Nimetus}</strong></p>
        </div>
    </div>
</body>
</html>";

                await _emailService.SendEmailsAsync(
                    studentEmails,
                    model.Subject,
                    htmlBody);

                TempData["Success"] = $"E-kiri edukalt saadeti {studentEmails.Count} õpilasele!";
                return RedirectToAction(nameof(Trainings));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Viga e-kirja saatmisel: {ex.Message}";
                return View(model);
            }
        }

        // GET: Admin/Registrations - Kinnita registreeringuid
        public async Task<IActionResult> Registrations()
        {
            var registrations = await _context.Registrations
                .Include(r => r.Training)
                    .ThenInclude(t => t.Course)
                .Include(r => r.Training)
                    .ThenInclude(t => t.Teacher)
                .Include(r => r.User)
                .OrderByDescending(r => r.RegistreerimisAeg)
                .ToListAsync();

            return View(registrations);
        }

        // POST: Admin/ConfirmRegistration/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null) return NotFound();

            registration.Staatus = "Kinnitatud";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registreering kinnitatud!";
            return RedirectToAction(nameof(Registrations));
        }

        // POST: Admin/RejectRegistration/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null) return NotFound();

            registration.Staatus = "Tühistatud";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registreering tühistatud!";
            return RedirectToAction(nameof(Registrations));
        }

        // GET: Admin/CreateTeacher
        public IActionResult CreateTeacher()
        {
            return View();
        }

        // POST: Admin/CreateTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(CreateTeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Loo kasutaja
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    FullName = model.Nimi
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Opetaja");

                    // Loo õpetaja profiil
                    var teacher = new Teacher
                    {
                        Nimi = model.Nimi,
                        Kvalifikatsioon = model.Kvalifikatsioon,
                        ApplicationUserId = user.Id
                    };

                    _context.Teachers.Add(teacher);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Õpetaja edukalt loodud!";
                    return RedirectToAction(nameof(Teachers));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Admin/Teachers
        public async Task<IActionResult> Teachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Trainings)
                .ToListAsync();

            return View(teachers);
        }

        // GET: Admin/Courses
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses
                .Include(c => c.Trainings)
                .ToListAsync();

            return View(courses);
        }

        // GET: Admin/CreateCourse
        public IActionResult CreateCourse()
        {
            return View();
        }

        // POST: Admin/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Kursus edukalt loodud!";
                return RedirectToAction(nameof(Courses));
            }
            return View(course);
        }
    }
}
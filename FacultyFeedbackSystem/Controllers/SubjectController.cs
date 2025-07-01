using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultyFeedbackSystem.Data;
using FacultyFeedbackSystem.Models;

namespace FacultyFeedbackSystem.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Faculty)
                .Where(s => s.IsActive)
                .ToListAsync();

            return View(subjects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Faculties = _context.Faculties.Where(f => f.IsActive).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.IsActive = true;
                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject added successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.Faculties = _context.Faculties.Where(f => f.IsActive).ToList();
            return View(subject);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                subject.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}

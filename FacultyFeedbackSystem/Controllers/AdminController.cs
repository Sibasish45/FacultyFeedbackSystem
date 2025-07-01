using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultyFeedbackSystem.Data;
using FacultyFeedbackSystem.Models;

namespace FacultyFeedbackSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboardData = new
            {
                TotalFaculties = _context.Faculties.Count(f => f.IsActive),
                TotalSubjects = _context.Subjects.Count(s => s.IsActive),
                TotalStudents = _context.Users.Count(u => u.Role == "Student" && u.IsActive),
                TotalFeedbacks = _context.Feedbacks.Count()
            };

            return View(dashboardData);
        }


        // Faculty Management
        public IActionResult Faculties()
        {
            var faculties = _context.Faculties.Where(f => f.IsActive).ToList();
            return View(faculties);
        }

        [HttpGet]
        public IActionResult CreateFaculty()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateFaculty(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Faculties.Add(faculty);
                _context.SaveChanges();
                TempData["Success"] = "Faculty added successfully!";
                return RedirectToAction("Faculties");
            }
            return View(faculty);
        }

        // Subject Management
        public IActionResult Subjects()
        {
            var subjects = _context.Subjects
                .Include(s => s.Faculty)
                .Where(s => s.IsActive)
                .ToList();
            return View(subjects);
        }
        // GET: CreateSubject
        [HttpGet]
        public IActionResult CreateSubject()
        {
            // ✅ Populate Faculty list for the dropdown
            ViewBag.Faculties = _context.Faculties.Where(f => f.IsActive).ToList();
            return View();
        }

        // POST: CreateSubject
        [HttpPost]
        public IActionResult CreateSubject(Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Subjects.Add(subject);
                _context.SaveChanges(); // 🔍 Add breakpoint here
                TempData["Success"] = "Subject added successfully!";
                return RedirectToAction("Subjects");
            }

            // If not valid, redisplay the form
            ViewBag.Faculties = _context.Faculties.Where(f => f.IsActive).ToList();
            return View(subject);
        }



        // Assign Faculty to Subject
        [HttpGet]
        public IActionResult AssignFaculty(int subjectId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject == null) return NotFound();

            ViewBag.Subject = subject;
            ViewBag.Faculties = _context.Faculties.Where(f => f.IsActive).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AssignFaculty(int subjectId, int facultyId)
        {
            var subject = _context.Subjects.Find(subjectId);
            if (subject != null)
            {
                subject.FacultyID = facultyId;
                _context.SaveChanges();
                TempData["Success"] = "Faculty assigned successfully!";
            }
            return RedirectToAction("Subjects");
        }

        // Reports
        public IActionResult Reports()
        {
            var reports = _context.Feedbacks
                .Include(f => f.Subject)
                .ThenInclude(s => s.Faculty)
                .GroupBy(f => new { f.Subject.Faculty.Name, f.Subject.SName })
                .Select(g => new
                {
                    FacultyName = g.Key.Name,
                    SubjectName = g.Key.SName,
                    TotalFeedbacks = g.Count(),
                    AverageRating = g.Average(f => (f.Discipline + f.Clarity + f.Teaching + f.Knowledge + f.Punctuality) / 5.0)
                })
                .ToList();

            return View(reports);
        }

    }
}

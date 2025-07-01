
using Microsoft.AspNetCore.Mvc;
using FacultyFeedbackSystem.Data;

namespace FacultyFeedbackSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboardStats = new
            {
                TotalFaculties = _context.Faculties.Count(f => f.IsActive),
                TotalSubjects = _context.Subjects.Count(s => s.IsActive),
                TotalFeedbacks = _context.Feedbacks.Count(),
                AverageRating = _context.Feedbacks.Any() ?
                    _context.Feedbacks.Average(f => (f.Discipline + f.Clarity + f.Teaching + f.Knowledge + f.Punctuality) / 5.0) : 0
            };

            return View(dashboardStats);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
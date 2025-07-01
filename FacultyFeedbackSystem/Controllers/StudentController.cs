using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultyFeedbackSystem.Data;
using FacultyFeedbackSystem.Models;
using FacultyFeedbackSystem.ViewModels;

namespace FacultyFeedbackSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var subjects = _context.Subjects
                .Include(s => s.Faculty)
                .Where(s => s.IsActive)
                .ToList();
            return View(subjects);
        }

        [HttpGet]
        public IActionResult SubmitFeedback(int subjectId)
        {
            var subject = _context.Subjects
                .Include(s => s.Faculty)
                .FirstOrDefault(s => s.SubjectID == subjectId);

            if (subject == null) return NotFound();

            var viewModel = new FeedbackViewModel
            {
                SubjectID = subject.SubjectID,
                SubjectName = subject.SName,
                FacultyName = subject.Faculty.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitFeedback(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                // In a real app, get student ID from session/authentication
                var studentId = 1; // Placeholder

                var feedback = new Feedback
                {
                    StudentID = studentId,
                    SubjectID = model.SubjectID,
                    Discipline = model.Discipline,
                    Clarity = model.Clarity,
                    Teaching = model.Teaching,
                    Knowledge = model.Knowledge,
                    Punctuality = model.Punctuality,
                    Comments = model.Comments
                };

                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();

                TempData["Success"] = "Feedback submitted successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult MyFeedbacks()
        {
            // In a real app, get student ID from session/authentication
            var studentId = 1; // Placeholder

            var feedbacks = _context.Feedbacks
                .Include(f => f.Subject)
                .ThenInclude(s => s.Faculty)
                .Where(f => f.StudentID == studentId)
                .OrderByDescending(f => f.SubmittedDate)
                .ToList();

            return View(feedbacks);
        }
    }
}

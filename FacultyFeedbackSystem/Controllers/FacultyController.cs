using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultyFeedbackSystem.Data;
using FacultyFeedbackSystem.ViewModels;

namespace FacultyFeedbackSystem.Controllers
{
    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // In a real app, get faculty ID from session/authentication
            var facultyId = 1; // Placeholder

            var subjects = _context.Subjects
                .Include(s => s.Faculty)
                .Where(s => s.FacultyID == facultyId && s.IsActive)
                .ToList();

            return View(subjects);
        }

        public IActionResult FeedbackReport(int subjectId)
        {
            var subject = _context.Subjects
                .Include(s => s.Faculty)
                .Include(s => s.Feedbacks)
                .FirstOrDefault(s => s.SubjectID == subjectId);

            if (subject == null) return NotFound();

            var feedbacks = subject.Feedbacks.ToList();

            var report = new FeedbackReportViewModel
            {
                FacultyName = subject.Faculty.Name,
                SubjectName = subject.SName,
                TotalFeedbacks = feedbacks.Count,
                AverageDiscipline = feedbacks.Any() ? feedbacks.Average(f => f.Discipline) : 0,
                AverageClarity = feedbacks.Any() ? feedbacks.Average(f => f.Clarity) : 0,
                AverageTeaching = feedbacks.Any() ? feedbacks.Average(f => f.Teaching) : 0,
                AverageKnowledge = feedbacks.Any() ? feedbacks.Average(f => f.Knowledge) : 0,
                AveragePunctuality = feedbacks.Any() ? feedbacks.Average(f => f.Punctuality) : 0,
                Comments = feedbacks.Where(f => !string.IsNullOrEmpty(f.Comments))
                                 .Select(f => f.Comments).ToList()
            };

            report.OverallAverage = (report.AverageDiscipline + report.AverageClarity +
                                   report.AverageTeaching + report.AverageKnowledge +
                                   report.AveragePunctuality) / 5.0;

            // Generate improvement suggestions based on low scores
            report.ImprovementSuggestions = GenerateImprovementSuggestions(report);

            return View(report);
        }

        private List<string> GenerateImprovementSuggestions(FeedbackReportViewModel report)
        {
            var suggestions = new List<string>();

            if (report.AverageClarity < 3.0)
                suggestions.Add("Consider improving explanation clarity and using more examples");

            if (report.AverageTeaching < 3.0)
                suggestions.Add("Explore different teaching methodologies and interactive approaches");

            if (report.AveragePunctuality < 3.0)
                suggestions.Add("Focus on time management and punctuality");

            if (report.AverageKnowledge < 3.0)
                suggestions.Add("Consider additional subject matter preparation");

            if (report.AverageDiscipline < 3.0)
                suggestions.Add("Work on classroom management and maintaining discipline");

            if (suggestions.Count == 0)
                suggestions.Add("Keep up the excellent work! Students are satisfied with your performance.");

            return suggestions;
        }

        public IActionResult AllReports()
        {
            var facultyId = 1; // Placeholder

            var reports = _context.Subjects
                .Include(s => s.Faculty)
                .Include(s => s.Feedbacks)
                .Where(s => s.FacultyID == facultyId && s.IsActive)
                .Select(s => new SubjectFeedbackSummaryViewModel
                {
                    SubjectName = s.SName,
                    TotalFeedbacks = s.Feedbacks.Count,
                    AverageRating = s.Feedbacks.Any() ?
                        s.Feedbacks.Average(f => (f.Discipline + f.Clarity + f.Teaching + f.Knowledge + f.Punctuality) / 5.0) : 0
                })
                .ToList();

            return View(reports); // ✔ Now it's a strongly typed list
        }

    }
}
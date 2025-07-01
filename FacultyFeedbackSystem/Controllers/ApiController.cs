using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacultyFeedbackSystem.Data;
using FacultyFeedbackSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FacultyFeedbackSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/api/subjects
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Faculty)
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    s.SubjectID,
                    s.SName,
                    s.Code,
                    FacultyName = s.Faculty.Name,
                    Department = s.Faculty.Department
                })
                .ToListAsync();

            return Ok(subjects);
        }

        // POST: api/api/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var feedback = new Feedback
            {
                SubjectID = model.SubjectID,
                Discipline = model.Discipline,
                Clarity = model.Clarity,
                Teaching = model.Teaching,
                Knowledge = model.Knowledge,
                Punctuality = model.Punctuality,
                Comments = model.Comments
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback submitted successfully!" });
        }

        // GET: api/api/report/{subjectId}
        [HttpGet("report/{subjectId}")]
        public async Task<IActionResult> GetFeedbackReport(int subjectId)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Subject)
                .ThenInclude(s => s.Faculty)
                .Where(f => f.SubjectID == subjectId)
                .ToListAsync();

            if (!feedbacks.Any())
                return NotFound("No feedback found for this subject");

            var first = feedbacks.First();

            var report = new
            {
                SubjectName = first.Subject.SName,
                FacultyName = first.Subject.Faculty.Name,
                TotalFeedbacks = feedbacks.Count,
                AverageScores = new
                {
                    Discipline = feedbacks.Average(f => f.Discipline),
                    Clarity = feedbacks.Average(f => f.Clarity),
                    Teaching = feedbacks.Average(f => f.Teaching),
                    Knowledge = feedbacks.Average(f => f.Knowledge),
                    Punctuality = feedbacks.Average(f => f.Punctuality)
                },
                Comments = feedbacks
                    .Where(f => !string.IsNullOrEmpty(f.Comments))
                    .Select(f => f.Comments)
                    .ToList()
            };

            return Ok(report);
        }

        // GET: api/api/charts/{subjectId}
        [HttpGet("charts/{subjectId}")]
        public async Task<IActionResult> GetChartData(int subjectId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.SubjectID == subjectId)
                .ToListAsync();

            if (!feedbacks.Any())
                return NotFound("No chart data available");

            var chartData = new
            {
                labels = new[] { "Discipline", "Clarity", "Teaching", "Knowledge", "Punctuality" },
                data = new[]
                {
                    feedbacks.Average(f => f.Discipline),
                    feedbacks.Average(f => f.Clarity),
                    feedbacks.Average(f => f.Teaching),
                    feedbacks.Average(f => f.Knowledge),
                    feedbacks.Average(f => f.Punctuality)
                }
            };

            return Ok(chartData);
        }
    }

    // ✅ DTO for Feedback Input (Validation-friendly)
    public class FeedbackInputModel
    {
        [Required]
        public int SubjectID { get; set; }

        [Range(1, 5, ErrorMessage = "Rate Discipline between 1 and 5")]
        public int Discipline { get; set; }

        [Range(1, 5, ErrorMessage = "Rate Clarity between 1 and 5")]
        public int Clarity { get; set; }

        [Range(1, 5, ErrorMessage = "Rate Teaching between 1 and 5")]
        public int Teaching { get; set; }

        [Range(1, 5, ErrorMessage = "Rate Knowledge between 1 and 5")]
        public int Knowledge { get; set; }

        [Range(1, 5, ErrorMessage = "Rate Punctuality between 1 and 5")]
        public int Punctuality { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }
    }
}

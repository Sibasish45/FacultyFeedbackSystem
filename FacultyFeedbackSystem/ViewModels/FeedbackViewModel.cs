using System.ComponentModel.DataAnnotations;

namespace FacultyFeedbackSystem.ViewModels
{
    public class FeedbackViewModel
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string FacultyName { get; set; }

        [Range(1, 5, ErrorMessage = "Please rate between 1 and 5")]
        public int Discipline { get; set; }

        [Range(1, 5, ErrorMessage = "Please rate between 1 and 5")]
        public int Clarity { get; set; }

        [Range(1, 5, ErrorMessage = "Please rate between 1 and 5")]
        public int Teaching { get; set; }

        [Range(1, 5, ErrorMessage = "Please rate between 1 and 5")]
        public int Knowledge { get; set; }

        [Range(1, 5, ErrorMessage = "Please rate between 1 and 5")]
        public int Punctuality { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }
    }
}
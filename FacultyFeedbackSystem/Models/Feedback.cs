
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacultyFeedbackSystem.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int SubjectID { get; set; }

        [Range(1, 5)]
        public int Discipline { get; set; }

        [Range(1, 5)]
        public int Clarity { get; set; }

        [Range(1, 5)]
        public int Teaching { get; set; }

        [Range(1, 5)]
        public int Knowledge { get; set; }
        
        [Range(1, 5)]
        public int Punctuality { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual User Student { get; set; }

        [ForeignKey("SubjectID")]
        public virtual Subject Subject { get; set; }
    }
}


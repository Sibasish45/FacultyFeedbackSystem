
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacultyFeedbackSystem.Models
{
    public class Subject
    {
        [Key]
        public int SubjectID { get; set; }

        [Required]
        [StringLength(100)]
        public string SName { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        public int FacultyID { get; set; }

        [StringLength(100)]
        public string Semester { get; set; }

        public int Credits { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("FacultyID")]
        public virtual Faculty Faculty { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace FacultyFeedbackSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // Admin, Faculty, Student

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = new DateTime(2024, 1, 1); // ✅ static
        public bool IsActive { get; set; } = true;
    }
}

using Microsoft.EntityFrameworkCore;
using FacultyFeedbackSystem.Models;

namespace FacultyFeedbackSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Faculty)
                .WithMany(f => f.Subjects)
                .HasForeignKey(s => s.FacultyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Student)
                .WithMany()
                .HasForeignKey(f => f.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Subject)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Name = "System Admin",
                    Email = "admin@university.edu",
                    Role = "Admin",
                    Password = "admin123" // In production, use proper hashing
                }
            );
        }
    }
}


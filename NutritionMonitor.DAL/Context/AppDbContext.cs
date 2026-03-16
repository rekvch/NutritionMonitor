namespace NutritionMonitor.DAL.Context;

using Microsoft.EntityFrameworkCore;
using NutritionMonitor.Models.Entities;
using System.Reflection.Emit;


/// <summary>
/// The single EF Core database context for the system.
/// Configured to use SQLite. All tables are defined here.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<MealLog> MealLogs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User table configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(200);
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        // Student table configuration
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.StudentNumber).IsUnique();
            entity.Property(s => s.StudentNumber).IsRequired().HasMaxLength(50);
            entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            entity.Ignore(s => s.FullName);   // computed, not stored
            entity.Ignore(s => s.AgeYears);   // computed, not stored
        });

        // MealLog table configuration
        modelBuilder.Entity<MealLog>(entity =>
        {
            entity.HasKey(m => m.Id);

            // Foreign key relationship: MealLog → Student
            entity.HasOne(m => m.Student)
                  .WithMany(s => s.MealLogs)
                  .HasForeignKey(m => m.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed a default Admin user on first run
        // Password: Admin@123 (BCrypt hashed)
        modelBuilder.Entity<User>().HasData(new User    
        {
            Id = 1,
            FullName = "System Administrator",
            Email = "admin@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
            Role = Models.Enums.UserRole.Admin,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsActive = true
        });
    }
}
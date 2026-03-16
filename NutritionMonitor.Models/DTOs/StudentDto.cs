namespace NutritionMonitor.Models.DTOs;

using NutritionMonitor.Models.Enums;

public class StudentDto
{
    public int Id { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; set; }
    public int AgeYears { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;
    public NutritionalStatus LatestStatus { get; set; }
}
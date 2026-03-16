namespace NutritionMonitor.Models.DTOs;

using NutritionMonitor.Models.Enums;

/// <summary>
/// Result returned from AuthService (BLL) back to the Login Form (UI).
/// Carries only what the UI needs — no sensitive data, no entity references.
/// </summary>
public class LoginResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
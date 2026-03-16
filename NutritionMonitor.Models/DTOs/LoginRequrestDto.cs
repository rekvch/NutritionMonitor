namespace NutritionMonitor.Models.DTOs;

/// <summary>
/// Data transferred from the Login Form (UI) to the AuthService (BLL).
/// The UI layer never passes raw entity objects — only DTOs.
/// </summary>
public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
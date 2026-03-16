namespace NutritionMonitor.BLL.Services;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.Models.DTOs;

/// <summary>
/// Handles authentication logic.
/// 
/// Layer communication:
///   LoginForm (UI) → AuthService.LoginAsync (BLL) → UserRepository.GetByEmailAsync (DAL) → SQLite
/// 
/// The BLL validates inputs, applies BCrypt verification, and returns a DTO.
/// It never returns raw User entities to the UI.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LoginResultDto> LoginAsync(LoginRequestDto request)
    {
        // BLL Validation — inputs must not be empty
        if (string.IsNullOrWhiteSpace(request.Email))
            return Fail("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return Fail("Password is required.");

        // BLL → DAL: fetch user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);

        // Generic failure message to prevent email enumeration attacks
        if (user is null)
            return Fail("Invalid email or password.");

        // BCrypt verification happens in BLL — not in UI, not in DAL
        bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
            return Fail("Invalid email or password.");

        return new LoginResultDto
        {
            IsSuccess = true,
            Message = "Login successful.",
            UserId = user.Id,
            FullName = user.FullName,
            Role = user.Role
        };
    }

    private static LoginResultDto Fail(string message) =>
        new() { IsSuccess = false, Message = message };
}
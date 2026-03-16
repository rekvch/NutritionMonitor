namespace NutritionMonitor.BLL.Interfaces;

using NutritionMonitor.Models.DTOs;

public interface IAuthService
{
    Task<LoginResultDto> LoginAsync(LoginRequestDto request);
}
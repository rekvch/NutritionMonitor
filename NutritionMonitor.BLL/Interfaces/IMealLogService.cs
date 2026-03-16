namespace NutritionMonitor.BLL.Interfaces;

using NutritionMonitor.Models.DTOs;

public interface IMealLogService
{
    Task<IEnumerable<MealLogDto>> GetLogsByStudentAsync(int studentId);
    Task<IEnumerable<MealLogDto>> GetLogsByStudentAndDateRangeAsync(
        int studentId, DateTime from, DateTime to);
    Task<MealLogDto?> GetLogByIdAsync(int id);
    Task<(bool Success, string Message)> AddLogAsync(MealLogDto dto);
    Task<(bool Success, string Message)> UpdateLogAsync(MealLogDto dto);
    Task<(bool Success, string Message)> DeleteLogAsync(int id);
    Task<IEnumerable<MealLogDto>> GetAllLogsAsync();
}
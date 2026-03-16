namespace NutritionMonitor.DAL.Interfaces;

using NutritionMonitor.Models.Entities;

public interface IMealLogRepository
{
    Task<IEnumerable<MealLog>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<MealLog>> GetByStudentIdAndDateRangeAsync(
        int studentId, DateTime from, DateTime to);
    Task<MealLog?> GetByIdAsync(int id);
    Task AddAsync(MealLog mealLog);
    Task UpdateAsync(MealLog mealLog);
    Task DeleteAsync(int id);
    Task<IEnumerable<MealLog>> GetAllAsync();
}
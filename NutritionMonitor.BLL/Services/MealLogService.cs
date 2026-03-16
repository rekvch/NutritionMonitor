namespace NutritionMonitor.BLL.Services;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.Models.DTOs;
using NutritionMonitor.Models.Entities;

/// <summary>
/// Handles all business logic for Meal Log management.
///
/// Layer communication:
///   MealLogForm (UI) → MealLogService (BLL) → MealLogRepository (DAL) → SQLite
///
/// Rules enforced here:
///   - All nutrient values must be non-negative
///   - Log date cannot be in the future
///   - Weight and height must be physiologically valid
///   - Entity ↔ DTO mapping stays in BLL
/// </summary>
public class MealLogService : IMealLogService
{
    private readonly IMealLogRepository _mealLogRepository;
    private readonly IStudentRepository _studentRepository;

    public MealLogService(
        IMealLogRepository mealLogRepository,
        IStudentRepository studentRepository)
    {
        _mealLogRepository = mealLogRepository;
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<MealLogDto>> GetLogsByStudentAsync(int studentId)
    {
        var logs = await _mealLogRepository.GetByStudentIdAsync(studentId);
        return logs.Select(MapToDto);
    }

    public async Task<IEnumerable<MealLogDto>> GetLogsByStudentAndDateRangeAsync(
        int studentId, DateTime from, DateTime to)
    {
        if (from > to)
            return Enumerable.Empty<MealLogDto>();

        var logs = await _mealLogRepository
            .GetByStudentIdAndDateRangeAsync(studentId, from, to);
        return logs.Select(MapToDto);
    }

    public async Task<MealLogDto?> GetLogByIdAsync(int id)
    {
        var log = await _mealLogRepository.GetByIdAsync(id);
        return log is null ? null : MapToDto(log);
    }

    public async Task<IEnumerable<MealLogDto>> GetAllLogsAsync()
    {
        var logs = await _mealLogRepository.GetAllAsync();
        return logs.Select(MapToDto);
    }

    public async Task<(bool Success, string Message)> AddLogAsync(MealLogDto dto)
    {
        var validation = await ValidateAsync(dto);
        if (!validation.Success) return validation;

        var entity = MapToEntity(dto);
        await _mealLogRepository.AddAsync(entity);
        return (true, "Meal log added successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateLogAsync(MealLogDto dto)
    {
        var validation = await ValidateAsync(dto);
        if (!validation.Success) return validation;

        var existing = await _mealLogRepository.GetByIdAsync(dto.Id);
        if (existing is null)
            return (false, "Meal log record not found.");

        // Update all fields on the tracked entity
        existing.LogDate = dto.LogDate;
        existing.MealType = dto.MealType;
        existing.Calories = dto.Calories;
        existing.ProteinGrams = dto.ProteinGrams;
        existing.CarbohydratesGrams = dto.CarbohydratesGrams;
        existing.FatGrams = dto.FatGrams;
        existing.IronMg = dto.IronMg;
        existing.CalciumMg = dto.CalciumMg;
        existing.VitaminAMcg = dto.VitaminAMcg;
        existing.VitaminCMg = dto.VitaminCMg;
        existing.WeightKg = dto.WeightKg;
        existing.HeightCm = dto.HeightCm;
        existing.Notes = dto.Notes;

        await _mealLogRepository.UpdateAsync(existing);
        return (true, "Meal log updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteLogAsync(int id)
    {
        var existing = await _mealLogRepository.GetByIdAsync(id);
        if (existing is null)
            return (false, "Meal log record not found.");

        await _mealLogRepository.DeleteAsync(id);
        return (true, "Meal log deleted successfully.");
    }

    // -------------------------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------------------------

    private async Task<(bool Success, string Message)> ValidateAsync(MealLogDto dto)
    {
        // Student must exist
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student is null)
            return (false, "Selected student does not exist.");

        // Date cannot be in the future
        if (dto.LogDate.Date > DateTime.Today)
            return (false, "Log date cannot be in the future.");

        // Meal type required
        if (string.IsNullOrWhiteSpace(dto.MealType))
            return (false, "Meal type is required.");

        // All nutrient values must be zero or positive
        if (dto.Calories < 0) return (false, "Calories cannot be negative.");
        if (dto.ProteinGrams < 0) return (false, "Protein cannot be negative.");
        if (dto.CarbohydratesGrams < 0) return (false, "Carbohydrates cannot be negative.");
        if (dto.FatGrams < 0) return (false, "Fat cannot be negative.");
        if (dto.IronMg < 0) return (false, "Iron cannot be negative.");
        if (dto.CalciumMg < 0) return (false, "Calcium cannot be negative.");
        if (dto.VitaminAMcg < 0) return (false, "Vitamin A cannot be negative.");
        if (dto.VitaminCMg < 0) return (false, "Vitamin C cannot be negative.");

        // Physiologically valid weight range (kg)
        if (dto.WeightKg <= 0 || dto.WeightKg > 300)
            return (false, "Weight must be between 0 and 300 kg.");

        // Physiologically valid height range (cm)
        if (dto.HeightCm <= 0 || dto.HeightCm > 250)
            return (false, "Height must be between 0 and 250 cm.");

        return (true, string.Empty);
    }

    /// <summary>
    /// Maps MealLog entity → MealLogDto.
    /// StudentName is resolved here so the UI never needs to join tables.
    /// </summary>
    private static MealLogDto MapToDto(MealLog m) => new()
    {
        Id = m.Id,
        StudentId = m.StudentId,
        StudentName = m.Student?.FullName ?? string.Empty,
        LogDate = m.LogDate,
        MealType = m.MealType,
        Calories = m.Calories,
        ProteinGrams = m.ProteinGrams,
        CarbohydratesGrams = m.CarbohydratesGrams,
        FatGrams = m.FatGrams,
        IronMg = m.IronMg,
        CalciumMg = m.CalciumMg,
        VitaminAMcg = m.VitaminAMcg,
        VitaminCMg = m.VitaminCMg,
        WeightKg = m.WeightKg,
        HeightCm = m.HeightCm,
        Notes = m.Notes
    };

    /// <summary>
    /// Maps MealLogDto (from UI) → MealLog entity (for DAL persistence).
    /// </summary>
    private static MealLog MapToEntity(MealLogDto dto) => new()
    {
        Id = dto.Id,
        StudentId = dto.StudentId,
        LogDate = dto.LogDate,
        MealType = dto.MealType,
        Calories = dto.Calories,
        ProteinGrams = dto.ProteinGrams,
        CarbohydratesGrams = dto.CarbohydratesGrams,
        FatGrams = dto.FatGrams,
        IronMg = dto.IronMg,
        CalciumMg = dto.CalciumMg,
        VitaminAMcg = dto.VitaminAMcg,
        VitaminCMg = dto.VitaminCMg,
        WeightKg = dto.WeightKg,
        HeightCm = dto.HeightCm,
        Notes = dto.Notes
    };
}
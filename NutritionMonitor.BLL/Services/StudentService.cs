namespace NutritionMonitor.BLL.Services;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.Models.DTOs;
using NutritionMonitor.Models.Entities;

/// <summary>
/// Handles all business logic for Student management.
///
/// Layer communication:
///   StudentForm (UI) → StudentService (BLL) → StudentRepository (DAL) → SQLite
///
/// Rules enforced here:
///   - Input validation
///   - Duplicate student number check
///   - Entity ↔ DTO mapping
///   - Soft delete enforcement
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToDto);
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return student is null ? null : MapToDto(student);
    }

    public async Task<IEnumerable<StudentDto>> SearchStudentsAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllStudentsAsync();

        var students = await _studentRepository.SearchAsync(keyword.Trim());
        return students.Select(MapToDto);
    }

    public async Task<(bool Success, string Message)> AddStudentAsync(StudentDto dto)
    {
        // BLL Validation
        var validation = ValidateStudentDto(dto);
        if (!validation.Success) return validation;

        // Business Rule: student number must be unique
        if (await _studentRepository.StudentNumberExistsAsync(dto.StudentNumber))
            return (false, $"Student number '{dto.StudentNumber}' already exists.");

        var entity = MapToEntity(dto);
        entity.EnrolledAt = DateTime.UtcNow;
        entity.IsActive = true;

        await _studentRepository.AddAsync(entity);
        return (true, "Student added successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateStudentAsync(StudentDto dto)
    {
        var validation = ValidateStudentDto(dto);
        if (!validation.Success) return validation;

        var existing = await _studentRepository.GetByIdAsync(dto.Id);
        if (existing is null)
            return (false, "Student not found.");

        // If student number changed, check it's not taken by another student
        if (existing.StudentNumber != dto.StudentNumber &&
            await _studentRepository.StudentNumberExistsAsync(dto.StudentNumber))
            return (false, $"Student number '{dto.StudentNumber}' already exists.");

        // Update fields
        existing.StudentNumber = dto.StudentNumber.Trim();
        existing.FirstName = dto.FirstName.Trim();
        existing.LastName = dto.LastName.Trim();
        existing.DateOfBirth = dto.DateOfBirth;
        existing.Gender = dto.Gender.Trim();
        existing.GradeLevel = dto.GradeLevel.Trim();

        await _studentRepository.UpdateAsync(existing);
        return (true, "Student updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteStudentAsync(int id)
    {
        var existing = await _studentRepository.GetByIdAsync(id);
        if (existing is null)
            return (false, "Student not found.");

        await _studentRepository.DeleteAsync(id);
        return (true, "Student deleted successfully.");
    }

    // -------------------------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------------------------

    private static (bool Success, string Message) ValidateStudentDto(StudentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.StudentNumber))
            return (false, "Student number is required.");

        if (string.IsNullOrWhiteSpace(dto.FirstName))
            return (false, "First name is required.");

        if (string.IsNullOrWhiteSpace(dto.LastName))
            return (false, "Last name is required.");

        if (string.IsNullOrWhiteSpace(dto.Gender))
            return (false, "Gender is required.");

        if (string.IsNullOrWhiteSpace(dto.GradeLevel))
            return (false, "Grade level is required.");

        if (dto.DateOfBirth >= DateTime.Today)
            return (false, "Date of birth must be in the past.");

        if (dto.DateOfBirth < new DateTime(1990, 1, 1))
            return (false, "Date of birth is not valid.");

        return (true, string.Empty);
    }

    /// <summary>
    /// Maps a Student entity (DAL/Models) to a StudentDto (UI-safe object).
    /// The UI never receives raw entity objects.
    /// </summary>
    private static StudentDto MapToDto(Student s) => new()
    {
        Id = s.Id,
        StudentNumber = s.StudentNumber,
        FirstName = s.FirstName,
        LastName = s.LastName,
        DateOfBirth = s.DateOfBirth,
        AgeYears = s.AgeYears,
        Gender = s.Gender,
        GradeLevel = s.GradeLevel
    };

    /// <summary>
    /// Maps a StudentDto (from UI) to a Student entity (for DAL persistence).
    /// </summary>
    private static Student MapToEntity(StudentDto dto) => new()
    {
        Id = dto.Id,
        StudentNumber = dto.StudentNumber.Trim(),
        FirstName = dto.FirstName.Trim(),
        LastName = dto.LastName.Trim(),
        DateOfBirth = dto.DateOfBirth,
        Gender = dto.Gender.Trim(),
        GradeLevel = dto.GradeLevel.Trim()
    };
}
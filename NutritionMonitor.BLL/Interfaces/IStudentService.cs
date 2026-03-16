namespace NutritionMonitor.BLL.Interfaces;

using NutritionMonitor.Models.DTOs;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto?> GetStudentByIdAsync(int id);
    Task<IEnumerable<StudentDto>> SearchStudentsAsync(string keyword);
    Task<(bool Success, string Message)> AddStudentAsync(StudentDto dto);
    Task<(bool Success, string Message)> UpdateStudentAsync(StudentDto dto);
    Task<(bool Success, string Message)> DeleteStudentAsync(int id);
}
namespace NutritionMonitor.DAL.Interfaces;

using NutritionMonitor.Models.Entities;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByStudentNumberAsync(string studentNumber);
    Task<IEnumerable<Student>> SearchAsync(string keyword);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
    Task<bool> StudentNumberExistsAsync(string studentNumber);
}
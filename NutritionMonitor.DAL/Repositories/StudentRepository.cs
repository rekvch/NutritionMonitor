namespace NutritionMonitor.DAL.Repositories;

using Microsoft.EntityFrameworkCore;
using NutritionMonitor.DAL.Context;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.Models.Entities;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.LastName)
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.MealLogs)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<Student?> GetByStudentNumberAsync(string studentNumber)
    {
        return await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber && s.IsActive);
    }

    public async Task<IEnumerable<Student>> SearchAsync(string keyword)
    {
        keyword = keyword.ToLower();
        return await _context.Students
            .AsNoTracking()
            .Where(s => s.IsActive
                && (s.FirstName.ToLower().Contains(keyword)
                 || s.LastName.ToLower().Contains(keyword)
                 || s.StudentNumber.ToLower().Contains(keyword)))
            .OrderBy(s => s.LastName)
            .ToListAsync();
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        // Soft delete — preserves historical data integrity
        var student = await _context.Students.FindAsync(id);
        if (student is not null)
        {
            student.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> StudentNumberExistsAsync(string studentNumber)
    {
        return await _context.Students
            .AnyAsync(s => s.StudentNumber == studentNumber);
    }
}
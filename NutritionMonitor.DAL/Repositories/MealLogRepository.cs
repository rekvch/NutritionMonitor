namespace NutritionMonitor.DAL.Repositories;

using Microsoft.EntityFrameworkCore;
using NutritionMonitor.DAL.Context;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.Models.Entities;

public class MealLogRepository : IMealLogRepository
{
    private readonly AppDbContext _context;

    public MealLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MealLog>> GetByStudentIdAsync(int studentId)
    {
        return await _context.MealLogs
            .AsNoTracking()
            .Where(m => m.StudentId == studentId)
            .OrderByDescending(m => m.LogDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MealLog>> GetByStudentIdAndDateRangeAsync(
        int studentId, DateTime from, DateTime to)
    {
        return await _context.MealLogs
            .AsNoTracking()
            .Where(m => m.StudentId == studentId
                     && m.LogDate.Date >= from.Date
                     && m.LogDate.Date <= to.Date)
            .OrderBy(m => m.LogDate)
            .ToListAsync();
    }

    public async Task<MealLog?> GetByIdAsync(int id)
    {
        return await _context.MealLogs
            .Include(m => m.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(MealLog mealLog)
    {
        await _context.MealLogs.AddAsync(mealLog);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MealLog mealLog)
    {
        _context.MealLogs.Update(mealLog);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var log = await _context.MealLogs.FindAsync(id);
        if (log is not null)
        {
            _context.MealLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<MealLog>> GetAllAsync()
    {
        return await _context.MealLogs
            .AsNoTracking()
            .Include(m => m.Student)
            .OrderByDescending(m => m.LogDate)
            .ToListAsync();
    }
}
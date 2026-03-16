namespace NutritionMonitor.DAL.Repositories;

using Microsoft.EntityFrameworkCore;
using NutritionMonitor.DAL.Context;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.Models.Entities;

/// <summary>
/// Handles all database operations for the User entity.
/// No business logic here — only data access.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()
                                   && u.IsActive);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
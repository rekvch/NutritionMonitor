namespace NutritionMonitor.DAL.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Points to the project root during design-time operations
        var dbPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..", "NutritionMonitor.UI", "nutrition.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new AppDbContext(optionsBuilder.Options);
    }
}
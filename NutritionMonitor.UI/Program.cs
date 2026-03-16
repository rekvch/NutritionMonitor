using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;
using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.BLL.Services;
using NutritionMonitor.DAL.Context;
using NutritionMonitor.DAL.Interfaces;
using NutritionMonitor.DAL.Repositories;
using NutritionMonitor.UI.Forms;
using Serilog;
using SerilogLog = Serilog.Log;

namespace NutritionMonitor.UI;

static class Program
{
    [STAThread]
    static void Main()
    {
        // Configure Serilog — rolling log file
        SerilogLog.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                path: "logs/nutrition-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        try
        {
            SerilogLog.Information("Application starting...");

            ApplicationConfiguration.Initialize();

            // Build DI container
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Apply EF Core migrations automatically on startup
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                SerilogLog.Information("Database migration applied successfully.");
            }

            // Launch Login Form via DI
            Application.Run(serviceProvider.GetRequiredService<LoginForm>());
        }
        catch (Exception ex)
        {
            SerilogLog.Fatal(ex, "Application terminated unexpectedly.");
        }
        finally
        {
            SerilogLog.CloseAndFlush();
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Database — SQLite, stored next to executable
        var dbPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "nutrition.db");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // DAL Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IMealLogRepository, MealLogRepository>();

        // BLL Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IMealLogService, MealLogService>();

        // UI Forms
        services.AddTransient<LoginForm>();
        services.AddTransient<MainDashboardForm>();
        services.AddTransient<StudentListForm>();
        services.AddTransient<StudentFormDialog>();
        services.AddTransient<MealLogListForm>();                // ← ADD
        services.AddTransient<MealLogFormDialog>();

        // UI Forms — registered as Transient so each open = new instance
        services.AddTransient<LoginForm>();
        services.AddTransient<MainDashboardForm>();
       
    }
}
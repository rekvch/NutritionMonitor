namespace NutritionMonitor.UI.Forms;

using Microsoft.VisualBasic.Logging;
using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.BLL.Services;
using NutritionMonitor.Models.DTOs;
using NutritionMonitor.Models.Enums;
using Serilog;
using SerilogLog = Serilog.Log;

/// <summary>
/// UI Layer — Login Form.
///
/// Responsibilities:
///   - Collect email and password input
///   - Call AuthService (BLL)
///   - Display result to user
///   - Navigate to Dashboard on success
///
/// What this form must NEVER do:
///   - Access the database
///   - Hash or verify passwords
///   - Contain business rules
/// </summary>
public partial class LoginForm : Form
{
    private readonly IAuthService _authService;
    private readonly IStudentService _studentService;
    private readonly IMealLogService _mealLogService;


    // Current logged-in user — passed to Dashboard after login
    public LoginResultDto? AuthenticatedUser { get; private set; }

    public LoginForm(IAuthService authService, IStudentService studentService, IMealLogService mealLogService)
    {
        _authService = authService;
        InitializeComponent();
        SetupFormStyle();
        _studentService = studentService;
        _mealLogService = mealLogService;

    }

    private void SetupFormStyle()
    {
        this.Text = "Student Nutrition Monitoring System — Login";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Size = new Size(420, 340);
        this.BackColor = Color.FromArgb(245, 247, 250);
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        // Disable button to prevent double-submission
        btnLogin.Enabled = false;
        lblError.Visible = false;
        
        try
        {
            // Build request DTO — UI only constructs DTOs, never entities
            var request = new LoginRequestDto
            {
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text
            };

            // UI → BLL: call authentication service
            var result = await _authService.LoginAsync(request);

            if (result.IsSuccess)
            {
                SerilogLog.Information("User {Email} logged in successfully. Role: {Role}",
                    request.Email, result.Role);

                AuthenticatedUser = result;

                // Open Dashboard, passing the authenticated user context
                var dashboard = new MainDashboardForm(result, _studentService, _mealLogService);
                dashboard.Show();
                this.Hide();

                // When dashboard closes, close login form too
                dashboard.FormClosed += (_, _) => this.Close();
            }
            else
            {
                ShowError(result.Message);
                SerilogLog.Warning("Failed login attempt for email: {Email}", request.Email);
            }
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Unexpected error during login.");
            ShowError("An unexpected error occurred. Please try again.");
        }
        finally
        {
            btnLogin.Enabled = true;
        }
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }

    private void txtPassword_KeyDown(object sender, KeyEventArgs e)
    {
        // Allow Enter key to trigger login
        if (e.KeyCode == Keys.Enter)
            btnLogin_Click(sender, e);
    }
}
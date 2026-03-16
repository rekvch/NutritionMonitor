namespace NutritionMonitor.UI.Forms;

using NutritionMonitor.BLL.Services;
using NutritionMonitor.Models.DTOs;
using NutritionMonitor.Models.Enums;
using NutritionMonitor.BLL.Interfaces;

/// <summary>
/// Main application shell after login.
/// Hosts navigation and module panels.
/// Role-based menu items are shown/hidden based on the authenticated user.
/// </summary>
public partial class MainDashboardForm : Form
{
    private readonly LoginResultDto _currentUser;
    private readonly IStudentService _studentService;
    private readonly IMealLogService _mealLogService;

    public MainDashboardForm(LoginResultDto currentUser, IStudentService studentService, IMealLogService mealLogService)
    {
        _currentUser = currentUser;
        _studentService = studentService;
        _mealLogService = mealLogService;
        InitializeComponent();
        ApplyRolePermissions();
        SetWelcomeMessage();
        _mealLogService = mealLogService;
    }

    private void SetWelcomeMessage()
    {
        lblWelcome.Text = $"Welcome, {_currentUser.FullName}  |  Role: {_currentUser.Role}";
    }

    private void ApplyRolePermissions()
    {
        // Nutritionists cannot access User Management
        bool isAdmin = _currentUser.Role == UserRole.Admin;
        btnUserManagement.Visible = isAdmin;
    }

    private void btnStudents_Click(object sender, EventArgs e)
    {
        // TODO: Phase 3 — open Student Management form
        var form = new StudentListForm(_studentService, _mealLogService);
        form.ShowDialog();
    }

    private void btnMealLogs_Click(object sender, EventArgs e)
    {
        var studentList = new StudentListForm(_studentService, _mealLogService);
        studentList.ShowDialog();
    }

    private void btnReports_Click(object sender, EventArgs e)
    {
        // TODO: Phase 8 — open Report form
        MessageBox.Show("Reports — Coming in Phase 8", "Navigation");
    }

    private void btnUserManagement_Click(object sender, EventArgs e)
    {
        // TODO: Admin only — User Management
        MessageBox.Show("User Management — Admin Only", "Navigation");
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        var confirm = MessageBox.Show(
            "Are you sure you want to logout?",
            "Confirm Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
            this.Close();
    }
}
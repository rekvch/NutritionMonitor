namespace NutritionMonitor.UI.Forms;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.BLL.Services;
using NutritionMonitor.Models.DTOs;
using SerilogLog = Serilog.Log;

/// <summary>
/// Reusable dialog for both Add and Edit operations.
/// 
/// When studentDto is null  → Add mode (empty form)
/// When studentDto has data → Edit mode (pre-filled form)
///
/// On Save: calls BLL service, returns DialogResult.OK to parent.
/// On Cancel: returns DialogResult.Cancel, no changes made.
/// </summary>
public partial class StudentFormDialog : Form
{
    private readonly IStudentService _studentService;
    private readonly StudentDto? _existingStudent;
    private readonly bool _isEditMode;

    public StudentFormDialog(IStudentService studentService, StudentDto? existingStudent)
    {
        _studentService = studentService;
        _existingStudent = existingStudent;
        _isEditMode = existingStudent is not null;

        InitializeComponent();
        PopulateDropdowns();

        if (_isEditMode)
            PreFillForm(existingStudent!);

        this.Text = _isEditMode ? "Edit Student" : "Add New Student";
        btnSave.Text = _isEditMode ? "Save Changes" : "Add Student";
    }

    private void PopulateDropdowns()
    {
        cmbGender.Items.AddRange(new[] { "Male", "Female" });
        cmbGender.SelectedIndex = 0;

        cmbGradeLevel.Items.AddRange(new[]
        {
            "Grade 1", "Grade 2", "Grade 3", "Grade 4",
            "Grade 5", "Grade 6", "Grade 7", "Grade 8",
            "Grade 9", "Grade 10", "Grade 11", "Grade 12"
        });
        cmbGradeLevel.SelectedIndex = 0;
    }

    private void PreFillForm(StudentDto dto)
    {
        txtStudentNumber.Text = dto.StudentNumber;
        txtFirstName.Text = dto.FirstName;
        txtLastName.Text = dto.LastName;
        dtpDateOfBirth.Value = dto.DateOfBirth;
        cmbGender.Text = dto.Gender;
        cmbGradeLevel.Text = dto.GradeLevel;

        // Student number cannot be changed in edit mode
        // to avoid accidental ID conflicts
        txtStudentNumber.ReadOnly = true;
        txtStudentNumber.BackColor = Color.FromArgb(230, 230, 230);
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        lblError.Visible = false;

        try
        {
            // Build DTO from form inputs — UI never builds entities
            var dto = new StudentDto
            {
                Id = _existingStudent?.Id ?? 0,
                StudentNumber = txtStudentNumber.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                DateOfBirth = dtpDateOfBirth.Value.Date,
                Gender = cmbGender.Text,
                GradeLevel = cmbGradeLevel.Text
            };

            // UI → BLL: delegate all validation and persistence logic
            var result = _isEditMode
                ? await _studentService.UpdateStudentAsync(dto)
                : await _studentService.AddStudentAsync(dto);

            if (result.Success)
            {
                SerilogLog.Information(
                    "{Action} student: {StudentNumber}",
                    _isEditMode ? "Updated" : "Added", dto.StudentNumber);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Text = result.Message;
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Error saving student.");
            lblError.Text = "An unexpected error occurred.";
            lblError.Visible = true;
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
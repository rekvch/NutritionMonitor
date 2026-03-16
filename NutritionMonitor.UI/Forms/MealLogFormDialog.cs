namespace NutritionMonitor.UI.Forms;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.Models.DTOs;
using SerilogLog = Serilog.Log;

/// <summary>
/// Reusable dialog for Add and Edit meal log operations.
///
/// existingLog = null  → Add mode
/// existingLog != null → Edit mode
///
/// Nutrient fields are grouped into tabs:
///   Tab 1 — Macronutrients + anthropometrics
///   Tab 2 — Micronutrients
/// This keeps the form readable without scrolling.
/// </summary>
public partial class MealLogFormDialog : Form
{
    private readonly IMealLogService _mealLogService;
    private readonly StudentDto _student;
    private readonly MealLogDto? _existingLog;
    private readonly bool _isEditMode;

    public MealLogFormDialog(
        IMealLogService mealLogService,
        StudentDto student,
        MealLogDto? existingLog)
    {
        _mealLogService = mealLogService;
        _student = student;
        _existingLog = existingLog;
        _isEditMode = existingLog is not null;

        InitializeComponent();
        PopulateMealTypes();

        if (_isEditMode)
            PreFillForm(existingLog!);

        this.Text = _isEditMode ? "Edit Meal Log" : "Add Meal Log";
        btnSave.Text = _isEditMode ? "Save Changes" : "Add Log";
        lblStudent.Text = $"Student: {_student.FullName} ({_student.StudentNumber})";
    }

    private void PopulateMealTypes()
    {
        cmbMealType.Items.AddRange(
            new[] { "Breakfast", "Morning Snack", "Lunch", "Afternoon Snack", "Dinner" });
        cmbMealType.SelectedIndex = 0;
        dtpLogDate.Value = DateTime.Today;
        dtpLogDate.MaxDate = DateTime.Today; // Prevent future dates in picker
    }

    private void PreFillForm(MealLogDto dto)
    {
        dtpLogDate.Value = dto.LogDate;
        cmbMealType.Text = dto.MealType;

        // Macros tab
        nudCalories.Value = (decimal)dto.Calories;
        nudProtein.Value = (decimal)dto.ProteinGrams;
        nudCarbohydrates.Value = (decimal)dto.CarbohydratesGrams;
        nudFat.Value = (decimal)dto.FatGrams;
        nudWeight.Value = (decimal)dto.WeightKg;
        nudHeight.Value = (decimal)dto.HeightCm;

        // Micros tab
        nudIron.Value = (decimal)dto.IronMg;
        nudCalcium.Value = (decimal)dto.CalciumMg;
        nudVitaminA.Value = (decimal)dto.VitaminAMcg;
        nudVitaminC.Value = (decimal)dto.VitaminCMg;

        txtNotes.Text = dto.Notes ?? string.Empty;
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        lblError.Visible = false;

        try
        {
            // Build DTO from form inputs
            var dto = new MealLogDto
            {
                Id = _existingLog?.Id ?? 0,
                StudentId = _student.Id,
                LogDate = dtpLogDate.Value.Date,
                MealType = cmbMealType.Text,
                Calories = (double)nudCalories.Value,
                ProteinGrams = (double)nudProtein.Value,
                CarbohydratesGrams = (double)nudCarbohydrates.Value,
                FatGrams = (double)nudFat.Value,
                IronMg = (double)nudIron.Value,
                CalciumMg = (double)nudCalcium.Value,
                VitaminAMcg = (double)nudVitaminA.Value,
                VitaminCMg = (double)nudVitaminC.Value,
                WeightKg = (double)nudWeight.Value,
                HeightCm = (double)nudHeight.Value,
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text)
                                        ? null : txtNotes.Text.Trim()
            };

            // UI → BLL
            var result = _isEditMode
                ? await _mealLogService.UpdateLogAsync(dto)
                : await _mealLogService.AddLogAsync(dto);

            if (result.Success)
            {
                SerilogLog.Information(
                    "{Action} meal log for student {StudentId} on {Date}",
                    _isEditMode ? "Updated" : "Added",
                    dto.StudentId,
                    dto.LogDate.ToShortDateString());

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
            SerilogLog.Error(ex, "Error saving meal log.");
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
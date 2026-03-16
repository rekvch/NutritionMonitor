namespace NutritionMonitor.UI.Forms;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.Models.DTOs;
using SerilogLog = Serilog.Log;

/// <summary>
/// Displays meal logs for a specific student.
/// Supports Add, Edit, Delete, and date-range filtering.
///
/// Opened from StudentListForm — receives the selected StudentDto.
/// UI → BLL flow only. No direct DAL or DB access.
/// </summary>
public partial class MealLogListForm : Form
{
    private readonly IMealLogService _mealLogService;
    private readonly IStudentService _studentService;
    private readonly StudentDto _student;
    private List<MealLogDto> _logs = new();

    public MealLogListForm(
        IMealLogService mealLogService,
        IStudentService studentService,
        StudentDto student)
    {
        _mealLogService = mealLogService;
        _studentService = studentService;
        _student = student;

        InitializeComponent();
        ConfigureGrid();

        // Display student context in the title bar
        this.Text = $"Meal Logs — {_student.FullName} ({_student.StudentNumber})";
        lblStudentInfo.Text =
            $"Student: {_student.FullName}  |  " +
            $"Grade: {_student.GradeLevel}  |  " +
            $"Age: {_student.AgeYears} yrs  |  " +
            $"Gender: {_student.Gender}";
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Default date filter: last 30 days
        dtpFrom.Value = DateTime.Today.AddDays(-30);
        dtpTo.Value = DateTime.Today;

        await LoadLogsAsync();
    }

    private async Task LoadLogsAsync()
    {
        try
        {
            SetLoadingState(true);

            _logs = (await _mealLogService.GetLogsByStudentAndDateRangeAsync(
                _student.Id,
                dtpFrom.Value.Date,
                dtpTo.Value.Date)).ToList();

            BindGrid(_logs);
            UpdateSummaryBar();
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Failed to load meal logs for student {Id}", _student.Id);
            MessageBox.Show("Failed to load meal logs. Check logs for details.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void btnFilter_Click(object sender, EventArgs e)
    {
        if (dtpFrom.Value.Date > dtpTo.Value.Date)
        {
            MessageBox.Show("'From' date cannot be after 'To' date.",
                "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        await LoadLogsAsync();
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var dialog = new MealLogFormDialog(_mealLogService, _student, null);
        if (dialog.ShowDialog() == DialogResult.OK)
            await LoadLogsAsync();
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        var selected = GetSelectedLog();
        if (selected is null)
        {
            MessageBox.Show("Please select a meal log to edit.",
                "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new MealLogFormDialog(_mealLogService, _student, selected);
        if (dialog.ShowDialog() == DialogResult.OK)
            await LoadLogsAsync();
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        var selected = GetSelectedLog();
        if (selected is null)
        {
            MessageBox.Show("Please select a meal log to delete.",
                "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"Delete meal log for {selected.LogDate:dd MMM yyyy} ({selected.MealType})?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            var result = await _mealLogService.DeleteLogAsync(selected.Id);

            MessageBox.Show(result.Message,
                result.Success ? "Success" : "Error",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (result.Success)
                await LoadLogsAsync();
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Failed to delete meal log {Id}", selected.Id);
            MessageBox.Show("An error occurred while deleting the log.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // -------------------------------------------------------------------------
    // Grid Helpers
    // -------------------------------------------------------------------------

    private void ConfigureGrid()
    {
        dgvLogs.AutoGenerateColumns = false;
        dgvLogs.ReadOnly = true;
        dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLogs.MultiSelect = false;
        dgvLogs.AllowUserToAddRows = false;
        dgvLogs.RowHeadersVisible = false;
        dgvLogs.BackgroundColor = Color.White;
        dgvLogs.BorderStyle = BorderStyle.None;
        dgvLogs.RowTemplate.Height = 34;

        dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 90);
        dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvLogs.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9F, FontStyle.Bold);
        dgvLogs.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvLogs.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(240, 245, 255);

        dgvLogs.Columns.AddRange(
            MakeCol("LogDate", "Date", 100, "dd MMM yyyy"),
            MakeCol("MealType", "Meal", 80),
            MakeCol("Calories", "Cal", 60, "N1"),
            MakeCol("ProteinGrams", "Protein(g)", 80, "N1"),
            MakeCol("CarbohydratesGrams", "Carbs(g)", 75, "N1"),
            MakeCol("FatGrams", "Fat(g)", 65, "N1"),
            MakeCol("IronMg", "Iron(mg)", 70, "N2"),
            MakeCol("CalciumMg", "Calcium(mg)", 85, "N1"),
            MakeCol("VitaminAMcg", "Vit-A(mcg)", 85, "N1"),
            MakeCol("VitaminCMg", "Vit-C(mg)", 80, "N1"),
            MakeCol("WeightKg", "Wt(kg)", 65, "N1"),
            MakeCol("HeightCm", "Ht(cm)", 65, "N1")
        );
    }

    private static DataGridViewTextBoxColumn MakeCol(
        string prop, string header, int width, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            DataPropertyName = prop,
            HeaderText = header,
            Width = width,
            SortMode = DataGridViewColumnSortMode.Automatic
        };
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        return col;
    }

    private void BindGrid(List<MealLogDto> logs)
    {
        dgvLogs.DataSource = null;
        dgvLogs.DataSource = logs;
        lblCount.Text = $"Records: {logs.Count}";
    }

    /// <summary>
    /// Shows daily totals of the currently filtered logs in the summary bar.
    /// All aggregation happens in the UI layer using already-fetched DTOs —
    /// no additional BLL/DAL call needed for a simple sum.
    /// </summary>
    private void UpdateSummaryBar()
    {
        if (_logs.Count == 0)
        {
            lblSummary.Text = "No records in selected range.";
            return;
        }

        double totalCalories = _logs.Sum(l => l.Calories);
        double totalProtein = _logs.Sum(l => l.ProteinGrams);
        double totalCarbs = _logs.Sum(l => l.CarbohydratesGrams);
        double totalFat = _logs.Sum(l => l.FatGrams);
        double avgWeight = _logs.Average(l => l.WeightKg);

        lblSummary.Text =
            $"Totals ({_logs.Count} entries) — " +
            $"Calories: {totalCalories:N0} kcal  |  " +
            $"Protein: {totalProtein:N1}g  |  " +
            $"Carbs: {totalCarbs:N1}g  |  " +
            $"Fat: {totalFat:N1}g  |  " +
            $"Avg Weight: {avgWeight:N1} kg";
    }

    private MealLogDto? GetSelectedLog()
    {
        if (dgvLogs.SelectedRows.Count == 0) return null;
        return dgvLogs.SelectedRows[0].DataBoundItem as MealLogDto;
    }

    private void SetLoadingState(bool loading)
    {
        btnAdd.Enabled = !loading;
        btnEdit.Enabled = !loading;
        btnDelete.Enabled = !loading;
        btnFilter.Enabled = !loading;
    }
}
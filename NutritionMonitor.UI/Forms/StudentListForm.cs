namespace NutritionMonitor.UI.Forms;

using NutritionMonitor.BLL.Interfaces;
using NutritionMonitor.BLL.Services;
using NutritionMonitor.Models.DTOs;
using SerilogLog = Serilog.Log;

/// <summary>
/// Displays all students in a DataGridView.
/// Supports Add, Edit, Delete, and Search.
///
/// UI → BLL flow:
///   This form calls IStudentService methods only.
///   It never touches repositories or DbContext directly.
/// </summary>
public partial class StudentListForm : Form

{
    private readonly IStudentService _studentService;
    public readonly IMealLogService _mealLogService;
        
    private List<StudentDto> _students = new();

    public StudentListForm(IStudentService studentService, IMealLogService mealLogService)
    {
        _studentService = studentService;
        _mealLogService = mealLogService;
        InitializeComponent();
        ConfigureGrid();
    }

    // Runs when the form first loads
    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadStudentsAsync();
    }

    private async Task LoadStudentsAsync()
    {
        try
        {
            SetLoadingState(true);
            _students = (await _studentService.GetAllStudentsAsync()).ToList();
            BindGrid(_students);
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Failed to load students.");
            MessageBox.Show("Failed to load students. Check logs for details.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            var keyword = txtSearch.Text.Trim();
            var results = await _studentService.SearchStudentsAsync(keyword);
            BindGrid(results.ToList());
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Search failed.");
        }
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        // Open Add dialog with an empty DTO
        using var dialog = new StudentFormDialog(_studentService, null);
        if (dialog.ShowDialog() == DialogResult.OK)
            await LoadStudentsAsync();
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        var selected = GetSelectedStudent();
        if (selected is null)
        {
            MessageBox.Show("Please select a student to edit.",
                "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new StudentFormDialog(_studentService, selected);
        if (dialog.ShowDialog() == DialogResult.OK)
            await LoadStudentsAsync();
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        var selected = GetSelectedStudent();
        if (selected is null)
        {
            MessageBox.Show("Please select a student to delete.",
                "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"Are you sure you want to delete {selected.FullName}?\nThis action cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            var result = await _studentService.DeleteStudentAsync(selected.Id);

            MessageBox.Show(result.Message,
                result.Success ? "Success" : "Error",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (result.Success)
                await LoadStudentsAsync();
        }
        catch (Exception ex)
        {
            SerilogLog.Error(ex, "Failed to delete student {Id}", selected.Id);
            MessageBox.Show("An error occurred while deleting the student.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            await SearchAsync();
    }

    private async Task SearchAsync()
    {
        var results = await _studentService.SearchStudentsAsync(txtSearch.Text.Trim());
        BindGrid(results.ToList());
    }

    // -------------------------------------------------------------------------
    // Grid Helpers
    // -------------------------------------------------------------------------

    private void ConfigureGrid()
    {
        dgvStudents.AutoGenerateColumns = false;
        dgvStudents.ReadOnly = true;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.MultiSelect = false;
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.RowHeadersVisible = false;
        dgvStudents.BackgroundColor = Color.White;
        dgvStudents.BorderStyle = BorderStyle.None;
        dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 90);
        dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        dgvStudents.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        dgvStudents.RowTemplate.Height = 36;
        dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
        dgvStudents.CellDoubleClick += DgvStudents_CellDoubleClick;

        dgvStudents.Columns.AddRange(
            MakeColumn("StudentNumber", "Student No.", 120),
            MakeColumn("FullName", "Full Name", 200),
            MakeColumn("Gender", "Gender", 80),
            MakeColumn("GradeLevel", "Grade", 80),
            MakeColumn("AgeYears", "Age", 60),
            MakeColumn("DateOfBirth", "Birthdate", 110, "dd MMM yyyy")
        );
    }

    private static DataGridViewTextBoxColumn MakeColumn(
        string property, string header, int width, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            DataPropertyName = property,
            HeaderText = header,
            Width = width,
            SortMode = DataGridViewColumnSortMode.Automatic
        };
        if (format is not null)
            col.DefaultCellStyle.Format = format;
        return col;
    }

    private void BindGrid(List<StudentDto> students)
    {
        dgvStudents.DataSource = null;
        dgvStudents.DataSource = students;
        lblCount.Text = $"Total Records: {students.Count}";
    }

    private StudentDto? GetSelectedStudent()
    {
        if (dgvStudents.SelectedRows.Count == 0) return null;
        return dgvStudents.SelectedRows[0].DataBoundItem as StudentDto;
    }

    private void SetLoadingState(bool loading)
    {
        btnAdd.Enabled = !loading;
        btnEdit.Enabled = !loading;
        btnDelete.Enabled = !loading;
        btnSearch.Enabled = !loading;
        lblCount.Text = loading ? "Loading..." : lblCount.Text;
    }

    private void DgvStudents_CellDoubleClick(object? sender,
    DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var student = dgvStudents.Rows[e.RowIndex].DataBoundItem as StudentDto;
        if (student is null) return;

        var mealLogForm = new MealLogListForm(_mealLogService, _studentService, student);
        mealLogForm.ShowDialog();
    }
}
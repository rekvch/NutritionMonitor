namespace NutritionMonitor.UI.Forms;

partial class StudentListForm
{
    private System.ComponentModel.IContainer components = null;

    private Panel pnlTop;
    private Panel pnlBottom;
    private DataGridView dgvStudents;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Label lblTitle;
    private Label lblCount;
    private Button btnViewLogs;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTop = new Panel();
        pnlBottom = new Panel();
        dgvStudents = new DataGridView();
        txtSearch = new TextBox();
        btnSearch = new Button();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        lblTitle = new Label();
        lblCount = new Label();
        SuspendLayout();
        // Form
        Text = "Student Management";
        Size = new Size(860, 600);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(245, 247, 250);
        // Top panel
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Height = 64;
        pnlTop.BackColor = Color.FromArgb(30, 60, 90);
        pnlTop.Padding = new Padding(16, 0, 16, 0);
        lblTitle.Text = "👥  Student Management";
        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        pnlTop.Controls.Add(lblTitle);
        // Bottom toolbar panel
        pnlBottom.Dock = DockStyle.Bottom;
        pnlBottom.Height = 56;
        pnlBottom.BackColor = Color.White;
        pnlBottom.Padding = new Padding(12, 10, 12, 10);
        // Search box
        txtSearch.Font = new Font("Segoe UI", 10F);
        txtSearch.SetBounds(12, 14, 260, 28);
        txtSearch.PlaceholderText = "🔍  Search by name or student no...";
        txtSearch.KeyDown += txtSearch_KeyDown;
        // Search button
        SetupButton(btnSearch, "Search", Color.FromArgb(30, 60, 90), 280, btnSearch_Click);
        SetupButton(btnAdd, "+ Add", Color.FromArgb(34, 139, 34), 370, btnAdd_Click);
        SetupButton(btnEdit, "✏ Edit", Color.FromArgb(200, 140, 0), 460, btnEdit_Click);
        SetupButton(btnDelete, "🗑 Delete", Color.FromArgb(180, 30, 30), 550, btnDelete_Click);
        // Record count label
        lblCount.Font = new Font("Segoe UI", 9F);
        lblCount.ForeColor = Color.Gray;
        lblCount.SetBounds(660, 18, 180, 20);
        lblCount.TextAlign = ContentAlignment.MiddleRight;
        pnlBottom.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnAdd, btnEdit, btnDelete, lblCount });
        // DataGridView
        dgvStudents.Dock = DockStyle.Fill;
        Controls.Add(dgvStudents);
        Controls.Add(pnlBottom);
        Controls.Add(pnlTop);
        ResumeLayout(false);
        btnViewLogs = new Button();
        SetupButton(btnViewLogs, "📋 Logs", Color.FromArgb(30, 100, 160), 640, btnViewLogs_Click);
        pnlBottom.Controls.Add(btnViewLogs);
    }

    private void SetupButton(Button btn, string text, Color color, int left, EventHandler handler)
    {
        btn.Text = text;
        btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btn.BackColor = color;
        btn.ForeColor = Color.White;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.SetBounds(left, 12, 82, 30);
        btn.Click += handler;
    }

    private void btnViewLogs_Click(object sender, EventArgs e)
    {
        var selected = GetSelectedStudent();
        if (selected is null)
        {
            MessageBox.Show("Please select a student first.",
                "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var mealLogForm = new MealLogListForm(_mealLogService, _studentService, selected);
        mealLogForm.ShowDialog();
    }

}
namespace NutritionMonitor.UI.Forms;

partial class MealLogListForm
{
    private System.ComponentModel.IContainer components = null;

    private Panel pnlTop;
    private Panel pnlFilter;
    private Panel pnlSummary;
    private Panel pnlToolbar;
    private DataGridView dgvLogs;
    private Label lblTitle;
    private Label lblStudentInfo;
    private Label lblFromDate;
    private Label lblToDate;
    private DateTimePicker dtpFrom;
    private DateTimePicker dtpTo;
    private Button btnFilter;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Label lblSummary;
    private Label lblCount;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.pnlTop = new Panel();
        this.pnlFilter = new Panel();
        this.pnlSummary = new Panel();
        this.pnlToolbar = new Panel();
        this.dgvLogs = new DataGridView();
        this.lblTitle = new Label();
        this.lblStudentInfo = new Label();
        this.lblFromDate = new Label();
        this.lblToDate = new Label();
        this.dtpFrom = new DateTimePicker();
        this.dtpTo = new DateTimePicker();
        this.btnFilter = new Button();
        this.btnAdd = new Button();
        this.btnEdit = new Button();
        this.btnDelete = new Button();
        this.lblSummary = new Label();
        this.lblCount = new Label();

        this.SuspendLayout();

        // Form
        this.Size = new Size(1100, 680);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.FromArgb(245, 247, 250);

        // ── Top Panel (title + student info) ─────────────────────────────────
        this.pnlTop.Dock = DockStyle.Top;
        this.pnlTop.Height = 70;
        this.pnlTop.BackColor = Color.FromArgb(30, 60, 90);
        this.pnlTop.Padding = new Padding(16, 6, 16, 6);

        this.lblTitle.Text = "🍽  Meal Logs";
        this.lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        this.lblTitle.ForeColor = Color.White;
        this.lblTitle.SetBounds(16, 6, 400, 26);

        this.lblStudentInfo.Font = new Font("Segoe UI", 9F);
        this.lblStudentInfo.ForeColor = Color.FromArgb(180, 210, 255);
        this.lblStudentInfo.SetBounds(16, 36, 900, 20);

        this.pnlTop.Controls.AddRange(new Control[] { lblTitle, lblStudentInfo });

        // ── Filter Panel ─────────────────────────────────────────────────────
        this.pnlFilter.Dock = DockStyle.Top;
        this.pnlFilter.Height = 46;
        this.pnlFilter.BackColor = Color.White;
        this.pnlFilter.Padding = new Padding(12, 8, 12, 8);

        this.lblFromDate.Text = "From:";
        this.lblFromDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblFromDate.SetBounds(12, 14, 36, 18);

        this.dtpFrom.Format = DateTimePickerFormat.Short;
        this.dtpFrom.SetBounds(52, 11, 120, 24);

        this.lblToDate.Text = "To:";
        this.lblToDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblToDate.SetBounds(182, 14, 24, 18);

        this.dtpTo.Format = DateTimePickerFormat.Short;
        this.dtpTo.SetBounds(210, 11, 120, 24);

        this.btnFilter.Text = "🔍 Filter";
        this.btnFilter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.btnFilter.BackColor = Color.FromArgb(30, 60, 90);
        this.btnFilter.ForeColor = Color.White;
        this.btnFilter.FlatStyle = FlatStyle.Flat;
        this.btnFilter.FlatAppearance.BorderSize = 0;
        this.btnFilter.SetBounds(340, 9, 90, 28);
        this.btnFilter.Click += new EventHandler(this.btnFilter_Click);

        this.pnlFilter.Controls.AddRange(new Control[]
            { lblFromDate, dtpFrom, lblToDate, dtpTo, btnFilter });

        // ── Summary Panel ────────────────────────────────────────────────────
        this.pnlSummary.Dock = DockStyle.Bottom;
        this.pnlSummary.Height = 36;
        this.pnlSummary.BackColor = Color.FromArgb(230, 240, 255);
        this.pnlSummary.Padding = new Padding(12, 0, 12, 0);

        this.lblSummary.Font = new Font("Segoe UI", 9F);
        this.lblSummary.ForeColor = Color.FromArgb(30, 60, 90);
        this.lblSummary.Dock = DockStyle.Fill;
        this.lblSummary.TextAlign = ContentAlignment.MiddleLeft;

        this.pnlSummary.Controls.Add(lblSummary);

        // ── Toolbar Panel ────────────────────────────────────────────────────
        this.pnlToolbar.Dock = DockStyle.Bottom;
        this.pnlToolbar.Height = 50;
        this.pnlToolbar.BackColor = Color.White;

        SetupButton(btnAdd, "+ Add Log", Color.FromArgb(34, 139, 34), 12, btnAdd_Click);
        SetupButton(btnEdit, "✏ Edit", Color.FromArgb(200, 140, 0), 110, btnEdit_Click);
        SetupButton(btnDelete, "🗑 Delete", Color.FromArgb(180, 30, 30), 208, btnDelete_Click);

        this.lblCount.Font = new Font("Segoe UI", 9F);
        this.lblCount.ForeColor = Color.Gray;
        this.lblCount.SetBounds(900, 16, 180, 18);
        this.lblCount.TextAlign = ContentAlignment.MiddleRight;

        this.pnlToolbar.Controls.AddRange(new Control[]
            { btnAdd, btnEdit, btnDelete, lblCount });

        // ── DataGridView ─────────────────────────────────────────────────────
        this.dgvLogs.Dock = DockStyle.Fill;

        this.Controls.Add(dgvLogs);
        this.Controls.Add(pnlToolbar);
        this.Controls.Add(pnlSummary);
        this.Controls.Add(pnlFilter);
        this.Controls.Add(pnlTop);

        this.ResumeLayout(false);
    }

    private void SetupButton(Button btn, string text, Color color, int left,
        EventHandler handler)
    {
        btn.Text = text;
        btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btn.BackColor = color;
        btn.ForeColor = Color.White;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.SetBounds(left, 10, 90, 30);
        btn.Click += handler;
    }
}
namespace NutritionMonitor.UI.Forms;

partial class StudentFormDialog
{
    private System.ComponentModel.IContainer components = null;

    private Label lblStudentNumber;
    private Label lblFirstName;
    private Label lblLastName;
    private Label lblDob;
    private Label lblGender;
    private Label lblGradeLevel;
    private Label lblError;
    private TextBox txtStudentNumber;
    private TextBox txtFirstName;
    private TextBox txtLastName;
    private DateTimePicker dtpDateOfBirth;
    private ComboBox cmbGender;
    private ComboBox cmbGradeLevel;
    private Button btnSave;
    private Button btnCancel;
    private Panel pnlButtons;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblStudentNumber = new Label();
        this.lblFirstName     = new Label();
        this.lblLastName      = new Label();
        this.lblDob           = new Label();
        this.lblGender        = new Label();
        this.lblGradeLevel    = new Label();
        this.lblError         = new Label();
        this.txtStudentNumber = new TextBox();
        this.txtFirstName     = new TextBox();
        this.txtLastName      = new TextBox();
        this.dtpDateOfBirth   = new DateTimePicker();
        this.cmbGender        = new ComboBox();
        this.cmbGradeLevel    = new ComboBox();
        this.btnSave          = new Button();
        this.btnCancel        = new Button();
        this.pnlButtons       = new Panel();

        this.SuspendLayout();

        this.Size = new Size(420, 460);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Padding = new Padding(24);

        int labelX  = 24;
        int inputX  = 24;
        int inputW  = 356;
        int rowH    = 54;
        int startY  = 20;

        void AddRow(Label lbl, Control input, string labelText, int row)
        {
            int y = startY + row * rowH;
            lbl.Text = labelText;
            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl.SetBounds(labelX, y, inputW, 18);
            input.Font = new Font("Segoe UI", 10F);
            input.SetBounds(inputX, y + 20, inputW, 28);
            this.Controls.Add(lbl);
            this.Controls.Add(input);
        }

        AddRow(lblStudentNumber, txtStudentNumber, "Student Number",  0);
        AddRow(lblFirstName,     txtFirstName,     "First Name",      1);
        AddRow(lblLastName,      txtLastName,       "Last Name",       2);
        AddRow(lblDob,           dtpDateOfBirth,    "Date of Birth",   3);

        // Dropdowns need DropDownStyle set
        cmbGender.DropDownStyle    = ComboBoxStyle.DropDownList;
        cmbGradeLevel.DropDownStyle = ComboBoxStyle.DropDownList;
        AddRow(lblGender,    cmbGender,    "Gender",      4);
        AddRow(lblGradeLevel, cmbGradeLevel, "Grade Level", 5);

        // Error label
        this.lblError.ForeColor = Color.Crimson;
        this.lblError.Font      = new Font("Segoe UI", 9F);
        this.lblError.SetBounds(24, 342, 356, 18);
        this.lblError.Visible   = false;
        this.Controls.Add(lblError);

        // Button panel
        this.pnlButtons.SetBounds(0, 368, 420, 60);
        this.pnlButtons.BackColor = Color.White;

        this.btnSave.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.btnSave.BackColor = Color.FromArgb(34, 139, 34);
        this.btnSave.ForeColor = Color.White;
        this.btnSave.FlatStyle = FlatStyle.Flat;
        this.btnSave.FlatAppearance.BorderSize = 0;
        this.btnSave.SetBounds(20, 12, 160, 36);
        this.btnSave.Click += new EventHandler(this.btnSave_Click);

        this.btnCancel.Text      = "Cancel";
        this.btnCancel.Font      = new Font("Segoe UI", 10F);
        this.btnCancel.BackColor = Color.FromArgb(120, 120, 120);
        this.btnCancel.ForeColor = Color.White;
        this.btnCancel.FlatStyle = FlatStyle.Flat;
        this.btnCancel.FlatAppearance.BorderSize = 0;
        this.btnCancel.SetBounds(200, 12, 160, 36);
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

        this.pnlButtons.Controls.AddRange(new Control[] { btnSave, btnCancel });
        this.Controls.Add(pnlButtons);

        this.ResumeLayout(false);
    }
}
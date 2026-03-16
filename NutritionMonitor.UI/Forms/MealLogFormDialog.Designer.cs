namespace NutritionMonitor.UI.Forms;

partial class MealLogFormDialog
{
    private System.ComponentModel.IContainer components = null;

    private Label          lblStudent;
    private Label          lblDate;
    private Label          lblMealType;
    private Label          lblError;
    private DateTimePicker dtpLogDate;
    private ComboBox       cmbMealType;
    private TabControl     tabNutrients;
    private TabPage        tabMacros;
    private TabPage        tabMicros;

    // Macros tab controls
    private Label          lblCalories;
    private Label          lblProtein;
    private Label          lblCarbohydrates;
    private Label          lblFat;
    private Label          lblWeight;
    private Label          lblHeight;
    private NumericUpDown  nudCalories;
    private NumericUpDown  nudProtein;
    private NumericUpDown  nudCarbohydrates;
    private NumericUpDown  nudFat;
    private NumericUpDown  nudWeight;
    private NumericUpDown  nudHeight;

    // Micros tab controls
    private Label          lblIron;
    private Label          lblCalcium;
    private Label          lblVitaminA;
    private Label          lblVitaminC;
    private NumericUpDown  nudIron;
    private NumericUpDown  nudCalcium;
    private NumericUpDown  nudVitaminA;
    private NumericUpDown  nudVitaminC;

    // Notes
    private Label          lblNotes;
    private TextBox        txtNotes;

    // Buttons
    private Button         btnSave;
    private Button         btnCancel;
    private Panel          pnlButtons;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // Instantiate all controls
        this.lblStudent        = new Label();
        this.lblDate           = new Label();
        this.lblMealType       = new Label();
        this.lblError          = new Label();
        this.dtpLogDate        = new DateTimePicker();
        this.cmbMealType       = new ComboBox();
        this.tabNutrients      = new TabControl();
        this.tabMacros         = new TabPage();
        this.tabMicros         = new TabPage();
        this.lblCalories       = new Label();
        this.lblProtein        = new Label();
        this.lblCarbohydrates  = new Label();
        this.lblFat            = new Label();
        this.lblWeight         = new Label();
        this.lblHeight         = new Label();
        this.nudCalories       = new NumericUpDown();
        this.nudProtein        = new NumericUpDown();
        this.nudCarbohydrates  = new NumericUpDown();
        this.nudFat            = new NumericUpDown();
        this.nudWeight         = new NumericUpDown();
        this.nudHeight         = new NumericUpDown();
        this.lblIron           = new Label();
        this.lblCalcium        = new Label();
        this.lblVitaminA       = new Label();
        this.lblVitaminC       = new Label();
        this.nudIron           = new NumericUpDown();
        this.nudCalcium        = new NumericUpDown();
        this.nudVitaminA       = new NumericUpDown();
        this.nudVitaminC       = new NumericUpDown();
        this.lblNotes          = new Label();
        this.txtNotes          = new TextBox();
        this.btnSave           = new Button();
        this.btnCancel         = new Button();
        this.pnlButtons        = new Panel();

        this.SuspendLayout();

        // ── Form ─────────────────────────────────────────────────────────────
        this.Size            = new Size(480, 590);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox     = false;
        this.MinimizeBox     = false;
        this.StartPosition   = FormStartPosition.CenterParent;
        this.BackColor       = Color.FromArgb(245, 247, 250);

        // ── Student label ─────────────────────────────────────────────────────
        this.lblStudent.Font      = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblStudent.ForeColor = Color.FromArgb(30, 60, 90);
        this.lblStudent.SetBounds(16, 14, 440, 18);

        // ── Date row ─────────────────────────────────────────────────────────
        this.lblDate.Text = "Log Date";
        this.lblDate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblDate.SetBounds(16, 42, 100, 18);

        this.dtpLogDate.Format = DateTimePickerFormat.Short;
        this.dtpLogDate.SetBounds(16, 62, 140, 26);

        // ── Meal type row ─────────────────────────────────────────────────────
        this.lblMealType.Text = "Meal Type";
        this.lblMealType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblMealType.SetBounds(180, 42, 100, 18);

        this.cmbMealType.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbMealType.Font          = new Font("Segoe UI", 10F);
        this.cmbMealType.SetBounds(180, 60, 270, 28);

        // ── TabControl ────────────────────────────────────────────────────────
        this.tabNutrients.SetBounds(12, 100, 448, 340);
        this.tabNutrients.Font = new Font("Segoe UI", 9F);

        // Macros Tab
        this.tabMacros.Text    = "Macronutrients & Measurements";
        this.tabMacros.Padding = new Padding(8);

        void AddMacroRow(Label lbl, NumericUpDown nud, string labelText,
            string unit, int row, decimal max, decimal dec = 1)
        {
            int y = 16 + row * 52;
            lbl.Text = $"{labelText} ({unit})";
            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl.SetBounds(8, y, 200, 18);
            nud.DecimalPlaces = (int)dec;
            nud.Maximum       = max;
            nud.Minimum       = 0;
            nud.Font          = new Font("Segoe UI", 10F);
            nud.SetBounds(8, y + 20, 140, 26);
            this.tabMacros.Controls.Add(lbl);
            this.tabMacros.Controls.Add(nud);
        }

        AddMacroRow(lblCalories,      nudCalories,      "Calories",      "kcal",  0, 9999);
        AddMacroRow(lblProtein,       nudProtein,       "Protein",       "g",     1,  999);
        AddMacroRow(lblCarbohydrates, nudCarbohydrates, "Carbohydrates", "g",     2,  999);
        AddMacroRow(lblFat,           nudFat,           "Fat",           "g",     3,  999);
        AddMacroRow(lblWeight,        nudWeight,        "Weight",        "kg",    4,  300);
        AddMacroRow(lblHeight,        nudHeight,        "Height",        "cm",    5,  250);

        // Micros Tab
        this.tabMicros.Text    = "Micronutrients";
        this.tabMicros.Padding = new Padding(8);

        void AddMicroRow(Label lbl, NumericUpDown nud, string labelText,
            string unit, int row, decimal max, decimal dec = 2)
        {
            int y = 16 + row * 52;
            lbl.Text = $"{labelText} ({unit})";
            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl.SetBounds(8, y, 200, 18);
            nud.DecimalPlaces = (int)dec;
            nud.Maximum       = max;
            nud.Minimum       = 0;
            nud.Font          = new Font("Segoe UI", 10F);
            nud.SetBounds(8, y + 20, 140, 26);
            this.tabMicros.Controls.Add(lbl);
            this.tabMicros.Controls.Add(nud);
        }

        AddMicroRow(lblIron,     nudIron,     "Iron",      "mg",  0,  100);
        AddMicroRow(lblCalcium,  nudCalcium,  "Calcium",   "mg",  1, 9999, 1);
        AddMicroRow(lblVitaminA, nudVitaminA, "Vitamin A", "mcg", 2, 9999, 1);
        AddMicroRow(lblVitaminC, nudVitaminC, "Vitamin C", "mg",  3, 9999, 1);

        this.tabNutrients.TabPages.Add(tabMacros);
        this.tabNutrients.TabPages.Add(tabMicros);

        // ── Notes ─────────────────────────────────────────────────────────────
        this.lblNotes.Text = "Notes (optional)";
        this.lblNotes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        this.lblNotes.SetBounds(16, 450, 200, 18);

        this.txtNotes.Font      = new Font("Segoe UI", 9F);
        this.txtNotes.Multiline = false;
        this.txtNotes.SetBounds(16, 470, 440, 26);

        // ── Error label ───────────────────────────────────────────────────────
        this.lblError.ForeColor = Color.Crimson;
        this.lblError.Font      = new Font("Segoe UI", 9F);
        this.lblError.SetBounds(16, 500, 440, 18);
        this.lblError.Visible   = false;

        // ── Button panel ──────────────────────────────────────────────────────
        this.pnlButtons.SetBounds(0, 520, 480, 52);
        this.pnlButtons.BackColor = Color.White;

        this.btnSave.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.btnSave.BackColor = Color.FromArgb(34, 139, 34);
        this.btnSave.ForeColor = Color.White;
        this.btnSave.FlatStyle = FlatStyle.Flat;
        this.btnSave.FlatAppearance.BorderSize = 0;
        this.btnSave.SetBounds(16, 10, 180, 34);
        this.btnSave.Click += new EventHandler(this.btnSave_Click);

        this.btnCancel.Text      = "Cancel";
        this.btnCancel.Font      = new Font("Segoe UI", 10F);
        this.btnCancel.BackColor = Color.FromArgb(120, 120, 120);
        this.btnCancel.ForeColor = Color.White;
        this.btnCancel.FlatStyle = FlatStyle.Flat;
        this.btnCancel.FlatAppearance.BorderSize = 0;
        this.btnCancel.SetBounds(210, 10, 180, 34);
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

        this.pnlButtons.Controls.AddRange(new Control[] { btnSave, btnCancel });

        // ── Add all to form ───────────────────────────────────────────────────
        this.Controls.AddRange(new Control[]
        {
            lblStudent, lblDate, dtpLogDate,
            lblMealType, cmbMealType,
            tabNutrients, lblNotes, txtNotes,
            lblError, pnlButtons
        });

        this.ResumeLayout(false);
    }
}
namespace NutritionMonitor.UI.Forms;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitle;
    private Label lblEmail;
    private Label lblPasswordLabel;
    private TextBox txtEmail;
    private TextBox txtPassword;
    private Button btnLogin;
    private Label lblError;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblTitle = new Label();
        this.lblEmail = new Label();
        this.lblPasswordLabel = new Label();
        this.txtEmail = new TextBox();
        this.txtPassword = new TextBox();
        this.btnLogin = new Button();
        this.lblError = new Label();

        this.SuspendLayout();

        // Title
        this.lblTitle.Text = "🥗 Nutrition Monitor";
        this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        this.lblTitle.ForeColor = Color.FromArgb(34, 139, 34);
        this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        this.lblTitle.SetBounds(20, 20, 370, 40);

        // Email label
        this.lblEmail.Text = "Email Address";
        this.lblEmail.Font = new Font("Segoe UI", 10F);
        this.lblEmail.SetBounds(50, 80, 120, 22);

        // Email textbox
        this.txtEmail.Font = new Font("Segoe UI", 11F);
        this.txtEmail.SetBounds(50, 104, 310, 30);

        // Password label
        this.lblPasswordLabel.Text = "Password";
        this.lblPasswordLabel.Font = new Font("Segoe UI", 10F);
        this.lblPasswordLabel.SetBounds(50, 148, 120, 22);

        // Password textbox
        this.txtPassword.Font = new Font("Segoe UI", 11F);
        this.txtPassword.PasswordChar = '●';
        this.txtPassword.SetBounds(50, 172, 310, 30);
        this.txtPassword.KeyDown += new KeyEventHandler(this.txtPassword_KeyDown);

        // Error label
        this.lblError.ForeColor = Color.Crimson;
        this.lblError.Font = new Font("Segoe UI", 9F);
        this.lblError.SetBounds(50, 212, 310, 20);
        this.lblError.Visible = false;

        // Login button
        this.btnLogin.Text = "Sign In";
        this.btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        this.btnLogin.BackColor = Color.FromArgb(34, 139, 34);
        this.btnLogin.ForeColor = Color.White;
        this.btnLogin.FlatStyle = FlatStyle.Flat;
        this.btnLogin.FlatAppearance.BorderSize = 0;
        this.btnLogin.SetBounds(50, 240, 310, 42);
        this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

        // Add controls
        this.Controls.AddRange(new Control[]
        {
            lblTitle, lblEmail, txtEmail,
            lblPasswordLabel, txtPassword,
            lblError, btnLogin
        });

        this.ResumeLayout(false);
    }
}
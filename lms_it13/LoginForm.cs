using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13
{
    public partial class LoginForm : Form
    {
        private Panel panelCard;
        private Label lblLogo;
        private Label lblSubtitle;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm()
        {
            BuildLoginUI();
        }

        private void BuildLoginUI()
        {
            // Form Settings
            this.Text = "Shelves - Login";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Card Panel
            panelCard = new Panel();
            panelCard.Size = new Size(400, 450);
            panelCard.BackColor = Color.White;
            panelCard.Location = new Point(
                (this.ClientSize.Width - panelCard.Width) / 2,
                (this.ClientSize.Height - panelCard.Height) / 2
            );
            panelCard.Anchor = AnchorStyles.None;
            this.Controls.Add(panelCard);

            // Logo Text
            lblLogo = new Label();
            lblLogo.Text = "Shelves";
            lblLogo.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblLogo.ForeColor = ColorTranslator.FromHtml("#355872");
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            lblLogo.Dock = DockStyle.Top;
            lblLogo.Height = 100;
            panelCard.Controls.Add(lblLogo);

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Library Management System";
            lblSubtitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Height = 30;
            panelCard.Controls.Add(lblSubtitle);

            // Username
            txtUsername = new TextBox();
            txtUsername.PlaceholderText = "Username";
            txtUsername.Font = new Font("Segoe UI", 10);
            txtUsername.Size = new Size(250, 35);
            txtUsername.Location = new Point(75, 180);
            panelCard.Controls.Add(txtUsername);

            // Password
            txtPassword = new TextBox();
            txtPassword.PlaceholderText = "Password";
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.Size = new Size(250, 35);
            txtPassword.Location = new Point(75, 230);
            txtPassword.PasswordChar = '*';
            panelCard.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "LOGIN";
            btnLogin.Size = new Size(250, 40);
            btnLogin.Location = new Point(75, 290);
            btnLogin.BackColor = ColorTranslator.FromHtml("#355872");
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;
            panelCard.Controls.Add(btnLogin);

            // Hover effect
            btnLogin.MouseEnter += (s, e) =>
            {
                btnLogin.BackColor = ColorTranslator.FromHtml("#7AAACE");
            };

            btnLogin.MouseLeave += (s, e) =>
            {
                btnLogin.BackColor = ColorTranslator.FromHtml("#355872");
            };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "SELECT Role FROM Users WHERE Username = @username AND Password = @password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string roleString = result.ToString();

                        UserRole role = Enum.Parse<UserRole>(roleString);

                        this.Hide();
                        MainDashboard dashboard = new MainDashboard(role, username);
                        dashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid credentials.", "Login Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
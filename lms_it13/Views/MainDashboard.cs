using lms_it13.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13
{
    public partial class MainDashboard : Form
    {
        private UserRole currentRole;
        private string loggedInUser;
        private string currentUsername;

        public MainDashboard(UserRole role, string username)
        {
            InitializeComponent();

            currentRole = role;
            loggedInUser = username;
            this.MinimumSize = new Size(1100, 650);

            lblLoggedUser.Text = $"Logged in as: {loggedInUser}";

            ApplyTheme();
            LoadUserControl(new DashboardControl());
            ApplyRolePermissions();
        }

        private void LoadUserControl(UserControl control)
        {
            panelContent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelContent.Controls.Add(control);
        }

        private void ApplyRolePermissions()
        {
            btnDashboard.Visible = true;
            btnManageBooks.Visible = true;
            btnBrowseBooks.Visible = true;
            btnUsers.Visible = true;
            btnReports.Visible = true;
            btnSales.Visible = true;
            btnPayments.Visible = true;
            btnFines.Visible = true;
            btnTNC.Visible = true;
            btnManageMyBooks.Visible = true;

            switch (currentRole)
            {
                case UserRole.Member:
                    btnManageBooks.Visible = false;
                    btnUsers.Visible = false;
                    btnReports.Visible = false;
                    btnSales.Visible = false;
                    break;

                case UserRole.Librarian:
                    btnBrowseBooks.Visible = false;
                    btnUsers.Visible = false;
                    btnManageMyBooks.Visible = false;
                    btnReports.Visible = false;
                    break;

                case UserRole.Admin:
                    btnBrowseBooks.Visible = false;
                    btnTNC.Visible = false;
                    btnManageMyBooks.Visible = false;
                    break;

                case UserRole.SuperAdmin:
                    break;
            }
        }

        private void ApplyTheme()
        {
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");
            panelSidebar.BackColor = ColorTranslator.FromHtml("#355872");
            panelTop.BackColor = ColorTranslator.FromHtml("#7AAACE");
            panelContent.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            StyleSidebarButtons();
        }



        private void StyleSidebarButtons()
        {
            foreach (Control ctrl in panelSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.ForeColor = Color.White;
                    btn.BackColor = ColorTranslator.FromHtml("#355872");
                    btn.Font = new Font("Segoe UI", 10);
                    btn.Height = 45;

                    btn.MouseEnter += (s, e) =>
                        btn.BackColor = ColorTranslator.FromHtml("#7AAACE");

                    btn.MouseLeave += (s, e) =>
                        btn.BackColor = ColorTranslator.FromHtml("#355872");
                }
            }
        }

        // Navigation Buttons
        private void btnDashboard_Click(object sender, EventArgs e)
            => LoadUserControl(new DashboardControl());

        private void btnManageBooks_Click(object sender, EventArgs e)
            => LoadUserControl(new ManageBooksControl());

        private void btnBrowseBooks_Click(object sender, EventArgs e)
            => LoadUserControl(new BrowseBooksControl(loggedInUser));

        private void btnMyBorrowedBooks_Click(object sender, EventArgs e)
            => LoadUserControl(new ManageMyBooksControl(loggedInUser));

        private void btnReports_Click(object sender, EventArgs e)
            => LoadUserControl(new ReportsControl());

        private void btnTNC_Click(object sender, EventArgs e)
            => LoadUserControl(new TermsControl());

        private void btnPayments_Click(object sender, EventArgs e)
            => LoadUserControl(new PaymentsControl(loggedInUser));

        private void btnUsers_Click(object sender, EventArgs e)
            => LoadUserControl(new UserManagementControl(currentRole));

        private void btnFines_Click(object sender, EventArgs e)
          => LoadUserControl(new FinesControl());

        private void btnSales_Click(object sender, EventArgs e)
          => LoadUserControl(new SalesControl());
    

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
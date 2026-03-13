using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public class UserManagementControl : UserControl
    {
        private DataGridView dgvUsers;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbRole;
        private Button btnAdd;
        private Button btnDelete;

        private TextBox txtNewPassword;
        private Button btnChangePassword;

        private string currentUsername;
        private string currentRole;

        public UserManagementControl(string username, string role)
        {
            currentUsername = username;
            currentRole = role;

            BuildUI();
            LoadUsers();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120
            };

            // 🔹 Add User Controls
            txtUsername = new TextBox
            {
                PlaceholderText = "Username",
                Location = new Point(20, 20),
                Width = 150
            };

            txtPassword = new TextBox
            {
                PlaceholderText = "Password",
                Location = new Point(180, 20),
                Width = 150
            };

            cmbRole = new ComboBox
            {
                Location = new Point(340, 20),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbRole.Items.AddRange(new string[]
            {
                "Member",
                "Librarian",
                "Admin",
                "SuperAdmin"
            });

            btnAdd = new Button
            {
                Text = "Add User",
                Location = new Point(500, 18),
                Width = 100,
                BackColor = ColorTranslator.FromHtml("#355872"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnDelete = new Button
            {
                Text = "Delete User",
                Location = new Point(610, 18),
                Width = 110,
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;

            // 🔹 Change Password Controls
            txtNewPassword = new TextBox
            {
                PlaceholderText = "New Password",
                Location = new Point(20, 60),
                Width = 150,
                PasswordChar = '*'
            };

            btnChangePassword = new Button
            {
                Text = "Change Password",
                Location = new Point(180, 58),
                Width = 150,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnChangePassword.Click += BtnChangePassword_Click;

            topPanel.Controls.Add(txtUsername);
            topPanel.Controls.Add(txtPassword);
            topPanel.Controls.Add(cmbRole);
            topPanel.Controls.Add(btnAdd);
            topPanel.Controls.Add(btnDelete);
            topPanel.Controls.Add(txtNewPassword);
            topPanel.Controls.Add(btnChangePassword);

            // 🔹 DataGridView
            dgvUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };

            dgvUsers.Columns.Add("Id", "Id");
            dgvUsers.Columns["Id"].Visible = false;
            dgvUsers.Columns.Add("Username", "Username");
            dgvUsers.Columns.Add("Role", "Role");

            this.Controls.Add(dgvUsers);
            this.Controls.Add(topPanel);

            // 🔒 Member Restrictions
            if (currentRole.Equals("Member", StringComparison.OrdinalIgnoreCase))
            {
                txtUsername.Visible = false;
                txtPassword.Visible = false;
                cmbRole.Visible = false;
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                txtNewPassword.Visible = false;
                btnChangePassword.Visible = false;
            }
        }

        private void LoadUsers()
        {
            dgvUsers.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "SELECT Id, Username, Role FROM Users";

                // If Member → restrict results
                if (currentRole.Equals("Member", StringComparison.OrdinalIgnoreCase))
                {
                    query += " WHERE Username = @username";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // ALWAYS add parameter safely (even if not used)
                    cmd.Parameters.AddWithValue("@username", currentUsername ?? "");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvUsers.Rows.Add(
                                reader["Id"],
                                reader["Username"],
                                reader["Role"]
                            );
                        }
                    }
                }
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Fill all fields.");
                return;
            }

            string newRole = cmbRole.SelectedItem.ToString();

            // 🔒 Role Restrictions
            if (currentRole == "Librarian" && newRole == "Admin")
            {
                MessageBox.Show("Librarian cannot create Admin.");
                return;
            }

            if (currentRole == "Admin" && newRole == "SuperAdmin")
            {
                MessageBox.Show("Admin cannot create SuperAdmin.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@r", newRole);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("User added.");
            LoadUsers();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
                return;

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);
            string selectedRole = dgvUsers.SelectedRows[0].Cells["Role"].Value.ToString();
            string selectedUsername = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

            if (selectedRole == "SuperAdmin")
            {
                MessageBox.Show("Cannot delete SuperAdmin.");
                return;
            }

            if (selectedUsername == currentUsername)
            {
                MessageBox.Show("You cannot delete your own account.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string query = "DELETE FROM Users WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("User deleted.");
            LoadUsers();
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Enter new password.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "UPDATE Users SET Password = @p WHERE Username = @u";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // ✅ ALWAYS use AddWithValue for safety
                    cmd.Parameters.AddWithValue("@p", txtNewPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@u", currentUsername);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Password updated successfully!");
            txtNewPassword.Clear();
        }
    }
}
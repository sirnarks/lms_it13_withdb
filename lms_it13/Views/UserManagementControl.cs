using lms_it13.Models;
using lms_it13.Repositories;

namespace lms_it13.Views
{
    public partial class UserManagementControl : UserControl
    {
        private UserRole currentRole;

        public UserManagementControl(UserRole role)
        {
            InitializeComponent();
            currentRole = role;
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvUsers.Columns[e.ColumnIndex].Name == "Delete")
            {
                string userName = dgvUsers.Rows[e.RowIndex]
                    .Cells["Name"].Value.ToString();

                UserRepository.Users.RemoveAll(u => u.Name == userName);

                LoadUsers();
            }
        }
        private void UserManagementControl_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgvUsers.Columns.Clear();
            dgvUsers.Rows.Clear();

            dgvUsers.ColumnCount = 2;
            dgvUsers.Columns[0].Name = "Name";
            dgvUsers.Columns[1].Name = "Role";

            dgvUsers.AllowUserToAddRows = false;

            foreach (var user in UserRepository.Users)
            {
                dgvUsers.Rows.Add(user.Name, user.Role);
            }

            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            deleteButton.Name = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            dgvUsers.Columns.Add(deleteButton);

            // Restrict Member
            if (currentRole == UserRole.Member)
            {
                btnAddUser.Visible = false;
                dgvUsers.Columns["Delete"].Visible = false;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please enter user name and role.");
                return;
            }

            UserRepository.Users.Add(new User
            {
                Name = txtUserName.Text,
                Role = cmbRole.SelectedItem.ToString()
            });

            txtUserName.Clear();
            cmbRole.SelectedIndex = -1;

            LoadUsers();

        }
        // your logic methods here
    }
}
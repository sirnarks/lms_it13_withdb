namespace lms_it13.Views
{
    partial class UserManagementControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            usermanagement = new Label();
            txtUserName = new TextBox();
            cmbRole = new ComboBox();
            btnAddUser = new Button();
            dgvUsers = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            SuspendLayout();
            // 
            // usermanagement
            // 
            usermanagement.Dock = DockStyle.Top;
            usermanagement.Font = new Font("Segoe UI", 20F);
            usermanagement.Location = new Point(0, 0);
            usermanagement.Name = "usermanagement";
            usermanagement.Size = new Size(778, 50);
            usermanagement.TabIndex = 0;
            usermanagement.Text = "User Management";
            // 
            // txtUserName
            // 
            txtUserName.Location = new Point(3, 53);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(100, 23);
            txtUserName.TabIndex = 1;
            // 
            // cmbRole
            // 
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "Member", "Librarian", "Admin", "SuperAdmin" });
            cmbRole.Location = new Point(109, 53);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(121, 23);
            cmbRole.TabIndex = 2;
            cmbRole.Text = "Role";
            // 
            // btnAddUser
            // 
            btnAddUser.Location = new Point(236, 53);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(75, 23);
            btnAddUser.TabIndex = 3;
            btnAddUser.Text = "Add User";
            btnAddUser.UseVisualStyleBackColor = true;
            btnAddUser.Click += btnAddUser_Click;
            // 
            // dgvUsers
            // 
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Dock = DockStyle.Bottom;
            dgvUsers.Location = new Point(0, 159);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.Size = new Size(778, 347);
            dgvUsers.TabIndex = 4;
            dgvUsers.CellContentClick += dgvUsers_CellContentClick;
            // 
            // UserManagementControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvUsers);
            Controls.Add(btnAddUser);
            Controls.Add(cmbRole);
            Controls.Add(txtUserName);
            Controls.Add(usermanagement);
            Name = "UserManagementControl";
            Size = new Size(778, 506);
            Load += UserManagementControl_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label usermanagement;
        private TextBox txtUserName;
        private ComboBox cmbRole;
        private Button btnAddUser;
        private DataGridView dgvUsers;
    }
}

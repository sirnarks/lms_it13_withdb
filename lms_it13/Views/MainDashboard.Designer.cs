namespace lms_it13
{
    partial class MainDashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelContent = new Panel();
            panelSidebar = new Panel();
            btnManageMyBooks = new Button();
            btnBrowseBooks = new Button();
            btnTNC = new Button();
            btnReports = new Button();
            btnFines = new Button();
            btnSales = new Button();
            btnPayments = new Button();
            btnUsers = new Button();
            btnManageBooks = new Button();
            lblLoggedUser = new Label();
            btnLogout = new Button();
            panelTop = new Panel();
            panelSidebar.SuspendLayout();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(200, 33);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(600, 473);
            panelContent.TabIndex = 2;
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = SystemColors.ControlDarkDark;
            panelSidebar.Controls.Add(btnManageMyBooks);
            panelSidebar.Controls.Add(btnBrowseBooks);
            panelSidebar.Controls.Add(btnTNC);
            panelSidebar.Controls.Add(btnReports);
            panelSidebar.Controls.Add(btnFines);
            panelSidebar.Controls.Add(btnSales);
            panelSidebar.Controls.Add(btnPayments);
            panelSidebar.Controls.Add(btnUsers);
            panelSidebar.Controls.Add(btnManageBooks);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.ForeColor = SystemColors.ControlText;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(200, 506);
            panelSidebar.TabIndex = 0;
            // 
            // btnManageMyBooks
            // 
            btnManageMyBooks.Dock = DockStyle.Top;
            btnManageMyBooks.Location = new Point(0, 360);
            btnManageMyBooks.Name = "btnManageMyBooks";
            btnManageMyBooks.Size = new Size(200, 45);
            btnManageMyBooks.TabIndex = 9;
            btnManageMyBooks.Text = "Manage My Books";
            btnManageMyBooks.UseVisualStyleBackColor = true;
            btnManageMyBooks.Click += btnMyBorrowedBooks_Click;
            // 
            // btnBrowseBooks
            // 
            btnBrowseBooks.Dock = DockStyle.Top;
            btnBrowseBooks.Location = new Point(0, 315);
            btnBrowseBooks.Name = "btnBrowseBooks";
            btnBrowseBooks.Size = new Size(200, 45);
            btnBrowseBooks.TabIndex = 8;
            btnBrowseBooks.Text = "Browse Books";
            btnBrowseBooks.UseVisualStyleBackColor = true;
            btnBrowseBooks.Click += btnBrowseBooks_Click;
            // 
            // btnTNC
            // 
            btnTNC.Dock = DockStyle.Top;
            btnTNC.Location = new Point(0, 270);
            btnTNC.Name = "btnTNC";
            btnTNC.Size = new Size(200, 45);
            btnTNC.TabIndex = 7;
            btnTNC.Text = "Terms and Conditions";
            btnTNC.UseVisualStyleBackColor = true;
            btnTNC.Click += btnTNC_Click;
            // 
            // btnReports
            // 
            btnReports.Dock = DockStyle.Top;
            btnReports.Location = new Point(0, 225);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(200, 45);
            btnReports.TabIndex = 6;
            btnReports.Text = "Reports";
            btnReports.UseVisualStyleBackColor = true;
            btnReports.Click += btnReports_Click;
            // 
            // btnFines
            // 
            btnFines.Dock = DockStyle.Top;
            btnFines.Location = new Point(0, 180);
            btnFines.Name = "btnFines";
            btnFines.Size = new Size(200, 45);
            btnFines.TabIndex = 5;
            btnFines.Text = "Fines";
            btnFines.UseVisualStyleBackColor = true;
            btnFines.Click += btnFines_Click;
            // 
            // btnSales
            // 
            btnSales.Dock = DockStyle.Top;
            btnSales.Location = new Point(0, 135);
            btnSales.Name = "btnSales";
            btnSales.Size = new Size(200, 45);
            btnSales.TabIndex = 4;
            btnSales.Text = "Sales";
            btnSales.UseVisualStyleBackColor = true;
            btnSales.Click += btnSales_Click;
            // 
            // btnPayments
            // 
            btnPayments.Dock = DockStyle.Top;
            btnPayments.Location = new Point(0, 90);
            btnPayments.Name = "btnPayments";
            btnPayments.Size = new Size(200, 45);
            btnPayments.TabIndex = 3;
            btnPayments.Text = "Payments";
            btnPayments.UseVisualStyleBackColor = true;
            btnPayments.Click += btnPayments_Click;
            // 
            // btnUsers
            // 
            btnUsers.Dock = DockStyle.Top;
            btnUsers.Location = new Point(0, 45);
            btnUsers.Name = "btnUsers";
            btnUsers.Size = new Size(200, 45);
            btnUsers.TabIndex = 2;
            btnUsers.Text = "Users Management";
            btnUsers.UseVisualStyleBackColor = true;
            btnUsers.Click += btnUsers_Click;
            // 
            // btnManageBooks
            // 
            btnManageBooks.Dock = DockStyle.Top;
            btnManageBooks.Location = new Point(0, 0);
            btnManageBooks.Name = "btnManageBooks";
            btnManageBooks.Size = new Size(200, 45);
            btnManageBooks.TabIndex = 1;
            btnManageBooks.Text = "ManageBooks";
            btnManageBooks.UseVisualStyleBackColor = true;
            btnManageBooks.Click += btnManageBooks_Click;
            // 
            // lblLoggedUser
            // 
            lblLoggedUser.AutoSize = true;
            lblLoggedUser.Location = new Point(6, 9);
            lblLoggedUser.Name = "lblLoggedUser";
            lblLoggedUser.Size = new Size(71, 15);
            lblLoggedUser.TabIndex = 0;
            lblLoggedUser.Text = "logged in as";
            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Right;
            btnLogout.Location = new Point(525, 0);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(75, 33);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.DimGray;
            panelTop.Controls.Add(btnLogout);
            panelTop.Controls.Add(lblLoggedUser);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(200, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(600, 33);
            panelTop.TabIndex = 1;
            // 
            // MainDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 506);
            Controls.Add(panelContent);
            Controls.Add(panelTop);
            Controls.Add(panelSidebar);
            Name = "MainDashboard";
            Text = "Form1";
            panelSidebar.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelContent;
        private Panel panelTop;
        private Panel panelSidebar;
        private Button btnTNC;
        private Button btnReports;
        private Button btnFines;
        private Button btnSales;
        private Button btnPayments;
        private Button btnUsers;
        private Button btnManageBooks;
        private Button btnBrowseBooks;
        private Button btnManageMyBooks;
        private Label lblLoggedUser;
        private Button btnLogout;

    
       

    }
}

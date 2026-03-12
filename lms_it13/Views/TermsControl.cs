using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public class TermsControl : UserControl
    {
        public TermsControl()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // ================= HEADER =================
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 90;
            headerPanel.BackColor = ColorTranslator.FromHtml("#355872");

            Label lblTitle = new Label();
            lblTitle.Text = "Terms & Conditions";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 25);

            headerPanel.Controls.Add(lblTitle);

            // ================= BOTTOM =================
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 70;
            bottomPanel.Padding = new Padding(0, 10, 40, 10);
            bottomPanel.BackColor = Color.WhiteSmoke;

            Button btnAgree = new Button();
            btnAgree.Text = "I Agree";
            btnAgree.Size = new Size(130, 40);
            btnAgree.BackColor = ColorTranslator.FromHtml("#355872");
            btnAgree.ForeColor = Color.White;
            btnAgree.FlatStyle = FlatStyle.Flat;
            btnAgree.FlatAppearance.BorderSize = 0;

            btnAgree.Click += (s, e) =>
            {
                MessageBox.Show("Thank you for accepting the terms.",
                    "Accepted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            };

            bottomPanel.Controls.Add(btnAgree);

            bottomPanel.Resize += (s, e) =>
            {
                btnAgree.Left = bottomPanel.Width - btnAgree.Width - 20;
                btnAgree.Top = (bottomPanel.Height - btnAgree.Height) / 2;
            };

            // ================= CONTENT =================
            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(40, 30, 40, 20);
            contentPanel.AutoScroll = true;
            contentPanel.BackColor = Color.White;

            Label lblContent = new Label();
            lblContent.MaximumSize = new Size(900, 0);
            lblContent.AutoSize = true;
            lblContent.Font = new Font("Segoe UI", 11);
            lblContent.ForeColor = ColorTranslator.FromHtml("#333333");

            lblContent.Text =
                "1. Borrowing Policy\n\n" +
                "• Members may borrow books for up to 7 days.\n" +
                "• Books must be returned on or before the due date.\n\n" +

                "2. Late Returns\n\n" +
                "• A fine may be charged for overdue books.\n" +
                "• Repeated late returns may result in suspension.\n\n" +

                "3. Book Condition\n\n" +
                "• Members are responsible for lost or damaged books.\n" +
                "• Replacement fees may apply.\n\n" +

                "4. User Responsibilities\n\n" +
                "• Members must use valid login credentials.\n" +
                "• Sharing accounts is prohibited.\n\n" +

                "5. Library Rights\n\n" +
                "• The library reserves the right to update policies at any time.\n" +
                "• Misuse of the system may result in account termination.\n\n" +

                "By using this system, you agree to follow all rules and policies stated above.";

            contentPanel.Controls.Add(lblContent);

            // 🔥 ADD PANELS IN CORRECT ORDER
            this.Controls.Add(contentPanel);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(headerPanel);
        }
    }
}
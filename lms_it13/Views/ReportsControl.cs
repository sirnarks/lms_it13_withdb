using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public class ReportsControl : UserControl
    {
        private Label lblTotalBooks;
        private Label lblTotalMembers;
        private Label lblTotalBorrowed;
        private Label lblTotalDue;

        private DataGridView dgvReports;

        private ComboBox cmbFilter;
        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;
        private Button btnApplyFilter;

        public ReportsControl()
        {
            BuildUI();
            LoadDashboardData();
        }
        private Button btnMarkLost;

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // ===== DASHBOARD =====
            FlowLayoutPanel dashboardPanel = new FlowLayoutPanel();
            dashboardPanel.Dock = DockStyle.Top;
            dashboardPanel.Height = 130;
            dashboardPanel.Padding = new Padding(20);

            lblTotalBooks = CreateCard("Total Books");
            lblTotalMembers = CreateCard("Total Members");
            lblTotalBorrowed = CreateCard("Total Borrowed");
            lblTotalDue = CreateCard("Total Due Books");

            dashboardPanel.Controls.Add(lblTotalBooks.Parent);
            dashboardPanel.Controls.Add(lblTotalMembers.Parent);
            dashboardPanel.Controls.Add(lblTotalBorrowed.Parent);
            dashboardPanel.Controls.Add(lblTotalDue.Parent);

            // ===== FILTER PANEL =====
            Panel filterPanel = new Panel();
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Height = 70;

            cmbFilter = new ComboBox();
            cmbFilter.Items.AddRange(new string[]
            {
                "All",
                "Only Due",
                "Only Returned",
                "Only Not Returned"
            });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.Width = 150;
            cmbFilter.Location = new Point(20, 20);

            dtFrom = new DateTimePicker();
            dtFrom.Width = 120;
            dtFrom.Format = DateTimePickerFormat.Short;
            dtFrom.Location = new Point(200, 20);

            dtTo = new DateTimePicker();
            dtTo.Width = 120;
            dtTo.Format = DateTimePickerFormat.Short;
            dtTo.Location = new Point(340, 20);

            btnApplyFilter = new Button();
            btnApplyFilter.Text = "Apply Filter";
            btnApplyFilter.Width = 120;
            btnApplyFilter.Location = new Point(540, 18);
            btnApplyFilter.BackColor = ColorTranslator.FromHtml("#355872");
            btnApplyFilter.ForeColor = Color.White;
            btnApplyFilter.FlatStyle = FlatStyle.Flat;
            btnApplyFilter.Click += BtnApplyFilter_Click;

            filterPanel.Controls.Add(cmbFilter);
            filterPanel.Controls.Add(dtFrom);
            filterPanel.Controls.Add(dtTo);
            filterPanel.Controls.Add(btnApplyFilter);

            // ===== TABLE =====
            dgvReports = new DataGridView();
            dgvReports.Dock = DockStyle.Fill;
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.ReadOnly = true;
            dgvReports.RowHeadersVisible = false;

            dgvReports.Columns.Add("BorrowId", "BorrowId");
            dgvReports.Columns["BorrowId"].Visible = false;

            dgvReports.Columns.Add("Username", "Username");
            dgvReports.Columns.Add("Title", "Book Title");
            dgvReports.Columns.Add("BorrowDate", "Borrow Date");
            dgvReports.Columns.Add("DueDate", "Due Date");
            dgvReports.Columns.Add("Returned", "Returned");


            this.Controls.Add(dgvReports);
            this.Controls.Add(filterPanel);
            this.Controls.Add(dashboardPanel);

            btnMarkLost = new Button();
            btnMarkLost.Text = "Mark as Lost";
            btnMarkLost.Width = 150;
            btnMarkLost.Height = 40;
            btnMarkLost.BackColor = Color.DarkRed;
            btnMarkLost.ForeColor = Color.White;
            btnMarkLost.FlatStyle = FlatStyle.Flat;
            btnMarkLost.Dock = DockStyle.Bottom;

            btnMarkLost.Click += BtnMarkLost_Click;

            this.Controls.Add(btnMarkLost);
        }
        private void BtnMarkLost_Click(object sender, EventArgs e)
        {
            if (dgvReports.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a record first.");
                return;
            }

            int borrowId = Convert.ToInt32(
                dgvReports.SelectedRows[0].Cells["BorrowId"].Value
            );

            string username = dgvReports.SelectedRows[0].Cells["Username"].Value.ToString();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                // 🔒 Prevent double marking
                string checkLost = "SELECT Lost FROM BorrowedBooks WHERE Id = @id";
                using (SqlCommand checkCmd = new SqlCommand(checkLost, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", borrowId);

                    object result = checkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Record not found.");
                        return;
                    }

                    bool isLost = Convert.ToBoolean(result);

                    if (isLost)
                    {
                        MessageBox.Show("Already marked as lost.");
                        return;
                    }
                }

                // 1️⃣ Mark as lost
                string updateBorrow = @"
            UPDATE BorrowedBooks
            SET Lost = 1, Returned = 1
            WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(updateBorrow, conn))
                {
                    cmd.Parameters.AddWithValue("@id", borrowId);
                    cmd.ExecuteNonQuery();
                }

                // 2️⃣ Insert fine
                string insertFine = @"
            INSERT INTO Fines (Username, BorrowId, FineType, Amount, Reason, Paid)
            VALUES (@username, @borrowId, 'Lost', 500, 'Lost Book', 0)";

                using (SqlCommand cmd = new SqlCommand(insertFine, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@borrowId", borrowId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Book marked as lost. Fine added.");
            LoadDashboardData();
        }

        private Label CreateCard(string title)
        {
            Panel card = new Panel();
            card.Width = 200;
            card.Height = 80;
            card.Margin = new Padding(15);
            card.BackColor = ColorTranslator.FromHtml("#355872");

            Label lblNumber = new Label();
            lblNumber.ForeColor = Color.White;
            lblNumber.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblNumber.Dock = DockStyle.Top;
            lblNumber.TextAlign = ContentAlignment.MiddleCenter;
            lblNumber.Height = 40;
            lblNumber.Text = "0";

            Label lblTitle = new Label();
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 9);
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.TopCenter;
            lblTitle.Text = title;

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblNumber);

            return lblNumber;
        }

        private void LoadDashboardData()
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Books", conn))
                    lblTotalBooks.Text = cmd.ExecuteScalar().ToString();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'Member'", conn))
                    lblTotalMembers.Text = cmd.ExecuteScalar().ToString();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM BorrowedBooks WHERE Returned = 0", conn))
                    lblTotalBorrowed.Text = cmd.ExecuteScalar().ToString();

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM BorrowedBooks WHERE Returned = 0 AND DueDate < GETDATE()", conn))
                    lblTotalDue.Text = cmd.ExecuteScalar().ToString();
            }

            LoadTableData();
        }

        private void LoadTableData()
        {
            dgvReports.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = @"
SELECT bb.Id AS BorrowId,
       bb.Username,
       b.Title,
       bb.BorrowDate,
       bb.DueDate,
       bb.Returned
FROM BorrowedBooks bb 
JOIN Books b ON bb.BookId = b.Id
WHERE bb.BorrowDate BETWEEN @from AND @to
";

                string filter = cmbFilter.SelectedItem.ToString();

                if (filter == "Only Due")
                    query += " AND bb.Returned = 0 AND bb.DueDate < GETDATE()";
                else if (filter == "Only Returned")
                    query += " AND bb.Returned = 1";
                else if (filter == "Only Not Returned")
                    query += " AND bb.Returned = 0";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@from", dtFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@to", dtTo.Value.Date.AddDays(1));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvReports.Rows.Add(
                                reader["BorrowId"],
                                reader["Username"],
                                reader["Title"],
                                Convert.ToDateTime(reader["BorrowDate"]).ToShortDateString(),
                                Convert.ToDateTime(reader["DueDate"]).ToShortDateString(),
                                Convert.ToBoolean(reader["Returned"]) ? "Yes" : "No"
                            );
                        }
                    }
                }
            }
        }

        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            LoadTableData();
        }
        
    }
}
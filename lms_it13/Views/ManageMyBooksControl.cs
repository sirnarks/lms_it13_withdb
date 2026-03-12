using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public partial class ManageMyBooksControl : UserControl
    {
        private DataGridView dgvMyBooks;
        private Button btnReturn;
        private Panel bottomPanel;
        private string currentUsername;

        public ManageMyBooksControl(string username)
        {
            currentUsername = username;
            BuildUI();
            LoadBorrowedBooks();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // ===== DATAGRID =====
            dgvMyBooks = new DataGridView();
            dgvMyBooks.Dock = DockStyle.Fill;
            dgvMyBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMyBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMyBooks.AllowUserToAddRows = false;
            dgvMyBooks.MultiSelect = false;
            dgvMyBooks.RowHeadersVisible = false;

            dgvMyBooks.Columns.Add("BorrowId", "BorrowId");
            dgvMyBooks.Columns["BorrowId"].Visible = false;

            dgvMyBooks.Columns.Add("Title", "Title");
            dgvMyBooks.Columns.Add("BorrowDate", "Borrow Date");
            dgvMyBooks.Columns.Add("DueDate", "Due Date");

            // ===== BOTTOM PANEL =====
            bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 70;
            bottomPanel.BackColor = Color.WhiteSmoke;

            // ===== RETURN BUTTON =====
            btnReturn = new Button();
            btnReturn.Text = "Return Selected Book";
            btnReturn.Width = 200;
            btnReturn.Height = 40;
            btnReturn.BackColor = ColorTranslator.FromHtml("#355872");
            btnReturn.ForeColor = Color.White;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.FlatAppearance.BorderSize = 0;

            // Center button
            btnReturn.Location = new Point(
                (bottomPanel.Width - btnReturn.Width) / 2,
                (bottomPanel.Height - btnReturn.Height) / 2
            );

            btnReturn.Anchor = AnchorStyles.None;

            btnReturn.Click += BtnReturn_Click;

            bottomPanel.Controls.Add(btnReturn);

            // ===== ADD TO CONTROL =====
            this.Controls.Add(dgvMyBooks);
            this.Controls.Add(bottomPanel);
        }

        private void LoadBorrowedBooks()
        {
            dgvMyBooks.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = @"
                    SELECT bb.Id, b.Title, bb.BorrowDate, bb.DueDate
                    FROM BorrowedBooks bb
                    JOIN Books b ON bb.BookId = b.Id
                    WHERE bb.Username = @username AND bb.Returned = 0";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvMyBooks.Rows.Add(
                                reader["Id"],
                                reader["Title"],
                                Convert.ToDateTime(reader["BorrowDate"]).ToShortDateString(),
                                Convert.ToDateTime(reader["DueDate"]).ToShortDateString()
                            );
                        }
                    }
                }
            }
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dgvMyBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to return.");
                return;
            }

            int borrowId = Convert.ToInt32(
                dgvMyBooks.SelectedRows[0].Cells["BorrowId"].Value
            );

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                // Mark as returned
                string returnQuery = "UPDATE BorrowedBooks SET Returned = 1 WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(returnQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", borrowId);
                    cmd.ExecuteNonQuery();
                }

                // Make book available again
                string updateBook = @"
                    UPDATE Books 
                    SET Status = 'Available'
                    WHERE Id = (SELECT BookId FROM BorrowedBooks WHERE Id = @id)";

                using (SqlCommand cmd = new SqlCommand(updateBook, conn))
                {
                    cmd.Parameters.AddWithValue("@id", borrowId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Book returned successfully!");
            LoadBorrowedBooks();
        }
    }
}
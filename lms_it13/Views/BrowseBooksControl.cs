using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public partial class BrowseBooksControl : UserControl
    {
        private DataGridView dgvBrowse;
        private TextBox txtSearch;
        private Button btnSearch;

        private string currentUsername;

        public BrowseBooksControl(string username)
        {
            currentUsername = username;
            BuildUI();
            LoadBooks();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // ===== SEARCH BAR =====
            txtSearch = new TextBox()
            {
                PlaceholderText = "Search by title, author, or ISBN...",
                Width = 300
            };

            btnSearch = new Button()
            {
                Text = "Search",
                Width = 100,
                BackColor = ColorTranslator.FromHtml("#355872"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSearch.Click += BtnSearch_Click;

            FlowLayoutPanel topPanel = new FlowLayoutPanel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 60;
            topPanel.Padding = new Padding(20, 15, 0, 0);
            topPanel.Controls.Add(txtSearch);
            topPanel.Controls.Add(btnSearch);

            // ===== DATAGRID =====
            dgvBrowse = new DataGridView();
            dgvBrowse.Dock = DockStyle.Fill;
            dgvBrowse.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBrowse.AllowUserToAddRows = false;
            dgvBrowse.ReadOnly = true;
            dgvBrowse.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBrowse.RowHeadersVisible = false;

            dgvBrowse.Columns.Add("ISBN", "ISBN");
            dgvBrowse.Columns.Add("Title", "Title");
            dgvBrowse.Columns.Add("Author", "Author");
            dgvBrowse.Columns.Add("Section", "Section");
            dgvBrowse.Columns.Add("AvailableCopies", "Available Copies");

            DataGridViewButtonColumn borrowBtn = new DataGridViewButtonColumn();
            borrowBtn.Name = "Borrow";
            borrowBtn.Text = "Borrow";
            borrowBtn.UseColumnTextForButtonValue = true;
            dgvBrowse.Columns.Add(borrowBtn);

            dgvBrowse.CellContentClick += DgvBrowse_CellContentClick;

            this.Controls.Add(dgvBrowse);
            this.Controls.Add(topPanel);
        }

        private void LoadBooks(string search = "")
        {
            dgvBrowse.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = @"
                    SELECT ISBN, Title, Author, Section, AvailableCopies
                    FROM Books
                    WHERE (@search = '' 
                        OR ISBN LIKE '%' + @search + '%'
                        OR Title LIKE '%' + @search + '%'
                        OR Author LIKE '%' + @search + '%')";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", search);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvBrowse.Rows.Add(
                                reader["ISBN"],
                                reader["Title"],
                                reader["Author"],
                                reader["Section"],
                                reader["AvailableCopies"]
                            );
                        }
                    }
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadBooks(txtSearch.Text.Trim());
        }

        private void DgvBrowse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvBrowse.Columns[e.ColumnIndex].Name == "Borrow")
            {
                string isbn = dgvBrowse.Rows[e.RowIndex].Cells["ISBN"].Value.ToString();
                int available = Convert.ToInt32(
                    dgvBrowse.Rows[e.RowIndex].Cells["AvailableCopies"].Value
                );

                if (available <= 0)
                {
                    MessageBox.Show("No copies available.");
                    return;
                }

                BorrowBook(isbn);
            }
        }

        private void BorrowBook(string isbn)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                int bookId = 0;

                // Get BookId
                string getBook = "SELECT Id FROM Books WHERE ISBN = @isbn";

                using (SqlCommand cmd = new SqlCommand(getBook, conn))
                {
                    cmd.Parameters.AddWithValue("@isbn", isbn);
                    bookId = (int)cmd.ExecuteScalar();
                }

                // Insert borrow record
                string insertBorrow = @"
                    INSERT INTO BorrowedBooks
                    (Username, BookId, BorrowDate, DueDate, Returned)
                    VALUES
                    (@username, @bookId, GETDATE(), DATEADD(day, 7, GETDATE()), 0)";

                using (SqlCommand cmd = new SqlCommand(insertBorrow, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    cmd.ExecuteNonQuery();
                }

                // Reduce available copies
                string updateBook = @"
                    UPDATE Books
                    SET AvailableCopies = AvailableCopies - 1
                    WHERE Id = @bookId";

                using (SqlCommand cmd = new SqlCommand(updateBook, conn))
                {
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Book borrowed successfully!");
            LoadBooks();
        }
    }
}
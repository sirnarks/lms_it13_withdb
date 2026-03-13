using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public class ManageBooksControl : UserControl
    {
        private DataGridView dgvBooks;

        private TextBox txtISBN;
        private TextBox txtTitle;
        private TextBox txtAuthor;
        private TextBox txtSection;
        private TextBox txtCopies;

        private Button btnAdd;

        public ManageBooksControl()
        {
            BuildUI();
            LoadBooksFromDatabase();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // 🔹 INPUT FIELDS
            txtISBN = new TextBox() { PlaceholderText = "ISBN", Width = 100 };
            txtTitle = new TextBox() { PlaceholderText = "Title", Width = 150 };
            txtAuthor = new TextBox() { PlaceholderText = "Author", Width = 150 };
            txtSection = new TextBox() { PlaceholderText = "Section", Width = 120 };
            txtCopies = new TextBox() { PlaceholderText = "Copies", Width = 80 };

            btnAdd = new Button()
            {
                Text = "Add Book",
                Width = 120,
                BackColor = ColorTranslator.FromHtml("#355872"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnAdd.Click += BtnAdd_Click;

            FlowLayoutPanel topPanel = new FlowLayoutPanel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 70;
            topPanel.Padding = new Padding(20, 20, 0, 0);

            topPanel.Controls.Add(txtISBN);
            topPanel.Controls.Add(txtTitle);
            topPanel.Controls.Add(txtAuthor);
            topPanel.Controls.Add(txtSection);
            topPanel.Controls.Add(txtCopies);
            topPanel.Controls.Add(btnAdd);

            // 🔹 TABLE
            dgvBooks = new DataGridView();
            dgvBooks.Dock = DockStyle.Fill;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.MultiSelect = false;
            dgvBooks.ReadOnly = true;

            dgvBooks.Columns.Add("Id", "Id");
            dgvBooks.Columns["Id"].Visible = false;

            dgvBooks.Columns.Add("ISBN", "ISBN");
            dgvBooks.Columns.Add("Title", "Title");
            dgvBooks.Columns.Add("Author", "Author");
            dgvBooks.Columns.Add("Section", "Section");
            dgvBooks.Columns.Add("AvailableCopies", "Available Copies");

            this.Controls.Add(dgvBooks);
            this.Controls.Add(topPanel);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtISBN.Text) ||
                string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtSection.Text) ||
                string.IsNullOrWhiteSpace(txtCopies.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!int.TryParse(txtCopies.Text.Trim(), out int copies) || copies < 0)
            {
                MessageBox.Show("Copies must be a valid number.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = @"INSERT INTO Books
                                (ISBN, Title, Author, Section, AvailableCopies)
                                VALUES
                                (@isbn, @title, @author, @section, @copies)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@isbn", txtISBN.Text.Trim());
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@author", txtAuthor.Text.Trim());
                    cmd.Parameters.AddWithValue("@section", txtSection.Text.Trim());
                    cmd.Parameters.AddWithValue("@copies", copies);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Book added successfully!");

            ClearInputs();
            LoadBooksFromDatabase();
        }

        private void LoadBooksFromDatabase()
        {
            dgvBooks.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "SELECT Id, ISBN, Title, Author, Section, AvailableCopies FROM Books";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvBooks.Rows.Add(
                            reader["Id"],
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

        private void ClearInputs()
        {
            txtISBN.Clear();
            txtTitle.Clear();
            txtAuthor.Clear();
            txtSection.Clear();
            txtCopies.Clear();
        }
    }
}
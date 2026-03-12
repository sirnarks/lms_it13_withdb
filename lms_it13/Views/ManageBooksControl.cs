using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public partial class ManageBooksControl : UserControl
    {
        private DataGridView dgvBooks;
        private Button btnAdd, btnToggleStatus;
        private TextBox txtISBN, txtTitle, txtAuthor, txtSection;
        private ComboBox cmbStatus;

        public ManageBooksControl()
        {
            BuildUI();
            LoadBooksFromDatabase();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // 🔹 TOP FORM
            txtISBN = new TextBox() { PlaceholderText = "ISBN", Width = 120 };
            txtTitle = new TextBox() { PlaceholderText = "Title", Width = 150 };
            txtAuthor = new TextBox() { PlaceholderText = "Author", Width = 150 };
            txtSection = new TextBox() { PlaceholderText = "Section", Width = 120 };

            cmbStatus = new ComboBox() { Width = 130 };
            cmbStatus.Items.AddRange(new string[] { "Available", "Unavailable" });
            cmbStatus.SelectedIndex = 0;

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
            topPanel.Height = 60;
            topPanel.Padding = new Padding(20, 15, 0, 0);

            topPanel.Controls.Add(txtISBN);
            topPanel.Controls.Add(txtTitle);
            topPanel.Controls.Add(txtAuthor);
            topPanel.Controls.Add(txtSection);
            topPanel.Controls.Add(cmbStatus);
            topPanel.Controls.Add(btnAdd);

            // 🔹 TABLE
            dgvBooks = new DataGridView();
            dgvBooks.Dock = DockStyle.Fill;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.MultiSelect = false;

            dgvBooks.Columns.Add("Id", "Id");
            dgvBooks.Columns["Id"].Visible = false;

            dgvBooks.Columns.Add("ISBN", "ISBN");
            dgvBooks.Columns.Add("Title", "Title");
            dgvBooks.Columns.Add("Author", "Author");
            dgvBooks.Columns.Add("Section", "Section");
            dgvBooks.Columns.Add("AvailableCopies", "Available Copies");

            // 🔹 Toggle Button
            btnToggleStatus = new Button()
            {
                Text = "Toggle Status",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#7AAACE"),
                FlatStyle = FlatStyle.Flat
            };
            btnToggleStatus.Click += BtnToggleStatus_Click;

            this.Controls.Add(dgvBooks);
            this.Controls.Add(btnToggleStatus);
            this.Controls.Add(topPanel);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtISBN.Text == "" || txtTitle.Text == "" ||
                txtAuthor.Text == "" || txtSection.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = @"INSERT INTO Books 
                                (ISBN, Title, Author, Section, Status) 
                                VALUES 
                                (@isbn, @title, @author, @section, @status)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@isbn", txtISBN.Text);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@author", txtAuthor.Text);
                    cmd.Parameters.AddWithValue("@section", txtSection.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Book added successfully!");

            ClearInputs();
            LoadBooksFromDatabase();
        }

        private void BtnToggleStatus_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a book first.");
                return;
            }

            int id = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["Id"].Value);
            string currentStatus = dgvBooks.SelectedRows[0].Cells["Status"].Value.ToString();
            string newStatus = currentStatus == "Available" ? "Unavailable" : "Available";

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "UPDATE Books SET Status = @status WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

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
            cmbStatus.SelectedIndex = 0;
        }
    }
}
using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public partial class SalesControl : UserControl
    {
        private DataGridView dgvSales;
        private TextBox txtClient, txtAmount;
        private Button btnAddClient, btnMarkPaid;
        private Label lblTotal;

        public SalesControl()
        {
            BuildUI();
            LoadSales();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            txtClient = new TextBox() { PlaceholderText = "Client Name", Width = 150 };
            txtAmount = new TextBox() { PlaceholderText = "Amount", Width = 100 };

            btnAddClient = new Button()
            {
                Text = "Add Client",
                Width = 120,
                BackColor = ColorTranslator.FromHtml("#355872"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddClient.Click += BtnAddClient_Click;

            FlowLayoutPanel topPanel = new FlowLayoutPanel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 60;
            topPanel.Padding = new Padding(20, 15, 0, 0);
            topPanel.Controls.Add(txtClient);
            topPanel.Controls.Add(txtAmount);
            topPanel.Controls.Add(btnAddClient);

            dgvSales = new DataGridView();
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.AllowUserToAddRows = false;

            dgvSales.Columns.Add("Id", "Id");
            dgvSales.Columns["Id"].Visible = false;
            dgvSales.Columns.Add("ClientName", "Client Name");
            dgvSales.Columns.Add("Amount", "Amount");
            dgvSales.Columns.Add("Paid", "Paid");
            dgvSales.Columns.Add("PaymentDate", "Payment Date");

            btnMarkPaid = new Button()
            {
                Text = "Mark as Paid",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#7AAACE"),
                FlatStyle = FlatStyle.Flat
            };
            btnMarkPaid.Click += BtnMarkPaid_Click;

            lblTotal = new Label()
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            this.Controls.Add(dgvSales);
            this.Controls.Add(lblTotal);
            this.Controls.Add(btnMarkPaid);
            this.Controls.Add(topPanel);
        }

        private void BtnAddClient_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "INSERT INTO Sales (ClientName, Amount, Paid) VALUES (@name, @amount, 0)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", txtClient.Text);
                    cmd.Parameters.AddWithValue("@amount", decimal.Parse(txtAmount.Text));
                    cmd.ExecuteNonQuery();
                }
            }

            LoadSales();
        }

        private void BtnMarkPaid_Click(object sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(dgvSales.SelectedRows[0].Cells["Id"].Value);

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "UPDATE Sales SET Paid = 1, PaymentDate = GETDATE() WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadSales();
        }

        private void LoadSales()
        {
            dgvSales.Rows.Clear();
            decimal total = 0;

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Sales";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool paid = Convert.ToBoolean(reader["Paid"]);
                        decimal amount = Convert.ToDecimal(reader["Amount"]);

                        if (paid)
                            total += amount;

                        dgvSales.Rows.Add(
                            reader["Id"],
                            reader["ClientName"],
                            amount,
                            paid ? "Yes" : "No",
                            reader["PaymentDate"] == DBNull.Value ? "" :
                                Convert.ToDateTime(reader["PaymentDate"]).ToShortDateString()
                        );
                    }
                }
            }

            lblTotal.Text = $"Total Revenue: ₱{total}";
        }
    }
}
using lms_it13.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace lms_it13.Views
{
    public class PaymentsControl : UserControl
    {
        private Label lblAmount;
        private Label lblStatus;
        private Label lblDate;

        public PaymentsControl(string username)
        {

            BuildUI();
            LoadPaymentInfo(username);
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            lblAmount = new Label();
            lblAmount.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblAmount.Location = new Point(40, 60);
            lblAmount.AutoSize = true;

            lblStatus = new Label();
            lblStatus.Font = new Font("Segoe UI", 14);
            lblStatus.Location = new Point(40, 120);
            lblStatus.AutoSize = true;

            lblDate = new Label();
            lblDate.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            lblDate.Location = new Point(40, 160);
            lblDate.AutoSize = true;

            this.Controls.Add(lblAmount);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblDate);
        }

        private void LoadPaymentInfo(string username)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Sales WHERE ClientName = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal amount = Convert.ToDecimal(reader["Amount"]);
                            bool paid = Convert.ToBoolean(reader["Paid"]);

                            lblAmount.Text = $"System License Fee: ₱{amount}";
                            lblStatus.Text = paid ? "Status: PAID" : "Status: NOT PAID";

                            if (paid && reader["PaymentDate"] != DBNull.Value)
                                lblDate.Text = $"Paid On: {Convert.ToDateTime(reader["PaymentDate"]).ToShortDateString()}";
                        }
                        else
                        {
                            lblAmount.Text = "No payment record found.";
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using lms_it13.Repositories;
using lms_it13.Models;

namespace lms_it13.Views
{
    public class FinesControl : UserControl
    {
        private DataGridView gridFines;
        private Label lblTotalFine;

        private const decimal FinePerDay = 10m;

        public FinesControl()
        {
            BuildUI();
            LoadFines();
        }

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            // HEADER
            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 80;
            header.BackColor = ColorTranslator.FromHtml("#355872");

            Label lblTitle = new Label();
            lblTitle.Text = "My Fines";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);

            header.Controls.Add(lblTitle);

            // GRID
            gridFines = new DataGridView();
            gridFines.Dock = DockStyle.Fill;
            gridFines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridFines.AllowUserToAddRows = false;
            gridFines.RowHeadersVisible = false;
            gridFines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridFines.MultiSelect = false;

            gridFines.Columns.Add("Title", "Book Title");
            gridFines.Columns.Add("BorrowDate", "Borrow Date");
            gridFines.Columns.Add("DueDate", "Due Date");
            gridFines.Columns.Add("DaysLate", "Days Late");
            gridFines.Columns.Add("FineAmount", "Fine Amount");

            // FOOTER
            Panel footer = new Panel();
            footer.Dock = DockStyle.Bottom;
            footer.Height = 60;
            footer.BackColor = Color.WhiteSmoke;

            lblTotalFine = new Label();
            lblTotalFine.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTotalFine.ForeColor = ColorTranslator.FromHtml("#355872");
            lblTotalFine.AutoSize = true;
            lblTotalFine.Location = new Point(30, 18);

            footer.Controls.Add(lblTotalFine);

            // ADD IN ORDER
            this.Controls.Add(gridFines);
            this.Controls.Add(footer);
            this.Controls.Add(header);
        }

        private void LoadFines()
        {
            gridFines.Rows.Clear();
            decimal totalFine = 0;

            var overdueBooks = BorrowRepository.BorrowedBooks
                .Where(r => !r.Returned && r.DueDate < DateTime.Now)
                .ToList();

            foreach (var record in overdueBooks)
            {
                int daysLate = (DateTime.Now - record.DueDate).Days;
                decimal fine = daysLate * FinePerDay;
                totalFine += fine;

                gridFines.Rows.Add(
                    record.Title,
                    record.BorrowDate.ToShortDateString(),
                    record.DueDate.ToShortDateString(),
                    daysLate,
                    $"₱ {fine}"
                );
            }

            lblTotalFine.Text = $"Total Fine: ₱ {totalFine}";
        }
    }
}
using lms_it13.Models;
using lms_it13.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace lms_it13
{
    public partial class ReportsControl : UserControl
    {
        public ReportsControl()
        {
            InitializeComponent();
            LoadReport();
        }
    
    private void LoadReport()
        {
            dgvReports.Columns.Clear();
            dgvReports.Rows.Clear();

            dgvReports.ColumnCount = 6;

            dgvReports.Columns[0].Name = "Member";
            dgvReports.Columns[1].Name = "Title";
            dgvReports.Columns[2].Name = "Borrow Date";
            dgvReports.Columns[3].Name = "Due Date";
            dgvReports.Columns[4].Name = "Status";
            dgvReports.Columns[5].Name = "Fine";

            dgvReports.AllowUserToAddRows = false;

            foreach (var record in BorrowRepository.BorrowedBooks)
            {
                string status;
                decimal fine = 0;

                if (record.Returned)
                {
                    status = "Returned";
                }
                else if (DateTime.Now > record.DueDate)
                {
                    status = "Overdue";

                    int overdueDays = (DateTime.Now - record.DueDate).Days;
                    fine = overdueDays * LibraryPolicy.FinePerDay;
                }
                else
                {
                    status = "Borrowed";
                }

                dgvReports.Rows.Add(
                    record.MemberName,
                    record.Title,
                    record.BorrowDate.ToShortDateString(),
                    record.DueDate.ToShortDateString(),
                    status,
                    fine
                );
            }
        }
    }
}

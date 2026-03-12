using System.Collections.Generic;
using lms_it13.Models;

namespace lms_it13.Repositories
{
    public static class SalesRepository
    {
        public static List<SaleRecord> Sales = new List<SaleRecord>()
        {
            new SaleRecord
            {
                ClientName = "superadmin",
                Amount = 5000,
                Paid = true,
                PaymentDate = DateTime.Now.AddDays(-5)
            },
            new SaleRecord
            {
                ClientName = "admin",
                Amount = 5000,
                Paid = false,
                PaymentDate = null
            }
        };
    }
}
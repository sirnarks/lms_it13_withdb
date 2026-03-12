using System;

namespace lms_it13.Models
{
    public class SaleRecord
    {
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
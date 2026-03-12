using System;

namespace lms_it13
{
    public class PaymentRecord
    {
        public string MemberName { get; set; }
        public string Description { get; set; } // Fine or Membership
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
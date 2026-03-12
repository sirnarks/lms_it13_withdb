namespace lms_it13.Models
{
    public class PaymentRecord
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string Description { get; set; }  

        public string MemberName { get; set; }   
    }
}
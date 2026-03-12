using System;

namespace lms_it13.Models
{
    public class BorrowRecord
    {
        public string MemberName { get; set; }
        public string BookId { get; set; }
        public string Title { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool Returned { get; set; }
    }
}
using System;
using lms_it13.Models;
using lms_it13.Repositories;

namespace lms_it13.Controllers
{
    public class BorrowController
    {
        public void BorrowBook(string memberName, string bookId, string title)
        {
            BorrowRepository.BorrowedBooks.Add(new BorrowRecord
            {
                MemberName = memberName,
                BookId = bookId,
                Title = title,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(LibraryPolicy.BorrowDays),
                Returned = false
            });
        }
    }
}
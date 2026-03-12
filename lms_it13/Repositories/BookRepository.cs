using System.Collections.Generic;
using lms_it13.Models;

namespace lms_it13.Repositories
{
    public static class BookRepository
    {
        public static List<Book> Books = new List<Book>
        {
            new Book { BookId = 1, Title = "C# Basics", Author = "John Doe", Quantity = 5 },
            new Book { BookId = 2, Title = "OOP Concepts", Author = "Jane Smith", Quantity = 3 }
        };
    }
}
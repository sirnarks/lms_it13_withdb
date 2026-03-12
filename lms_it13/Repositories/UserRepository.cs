using lms_it13.Models;
using System.Collections.Generic;

namespace lms_it13.Repositories
{
    public static class UserRepository
    {
        public static List<User> Users = new List<User>
{
    new User { Name = "Member1", Role = "Member", Password = "123" },
    new User { Name = "Librarian1", Role = "Librarian", Password = "123" },
    new User { Name = "Admin1", Role = "Admin", Password = "admin" },
    new User { Name = "SuperAdmin1", Role = "SuperAdmin", Password = "super" }
};
    }
}
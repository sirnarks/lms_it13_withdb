using Microsoft.Data.SqlClient;

namespace lms_it13
{
    public static class DatabaseHelper
    {
        public static string ConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=LMS_DB;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
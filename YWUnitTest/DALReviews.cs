using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Data;
using System.Data.SqlClient;

namespace ywBookStoreLIB
{
    public class DALReviews
    {
        private string connStr = Properties.Settings.Default.ywConnectionString;

        public DataTable GetReviewsByISBN(string isbn)
        {
            string query = @"SELECT UserID, Rating, ReviewText, CreatedAt
                             FROM Reviews
                             WHERE ISBN = @isbn
                             ORDER BY CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@isbn", isbn);

                DataTable dt = new DataTable();
                conn.Open();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        public void AddReview(string isbn, int userId, int rating, string review)
        {
            string query = @"INSERT INTO Reviews (ISBN, UserID, Rating, ReviewText)
                             VALUES (@isbn, @userId, @rating, @review)";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@isbn", isbn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@review", review);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

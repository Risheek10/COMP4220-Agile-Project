using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // 1. Rating must be 1-5
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating),
                    "Rating must be between 1 and 5");

            // 2. Review can't be empty
            if (string.IsNullOrWhiteSpace(review))
                throw new ArgumentException("Review text cannot be empty", nameof(review));

            // 3. ISBN must be 10 chars
            if (string.IsNullOrWhiteSpace(isbn) || isbn.Trim().Length != 10)
                throw new ArgumentException("ISBN must be 10 characters", nameof(isbn));

            // 4. UserID must be positive
            if (userId <= 0)
                throw new ArgumentException("Invalid UserID", nameof(userId));


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

        public DataTable GetReviews()
        {
            string query = @"SELECT ReviewID, ISBN, UserID, Rating, ReviewText, CreatedAt
                             FROM Reviews
                             ORDER BY CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                DataTable dt = new DataTable();
                conn.Open();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        public bool UpdateReview(int reviewId, int rating, string reviewText)
        {
            string query = @"UPDATE Reviews
                             SET Rating = @rating, ReviewText = @reviewText
                             WHERE ReviewID = @reviewId";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@reviewId", reviewId);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@reviewText", reviewText);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool DeleteReview(int reviewId)
        {
            string query = @"DELETE FROM Reviews WHERE ReviewID = @reviewId";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@reviewId", reviewId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}

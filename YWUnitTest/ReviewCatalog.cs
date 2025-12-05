using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace ywBookStoreLIB
{
    public class ReviewCatalog
    {
        private DALReviews dal = new DALReviews();

        public DataTable GetReviews(string isbn)
        {
            return dal.GetReviewsByISBN(isbn);
        }

        public void AddReview(string isbn, int userId, int rating, string text)
        {
            dal.AddReview(isbn, userId, rating, text);
        }
    }
}

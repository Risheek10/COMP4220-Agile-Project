using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ywBookStoreLIB;

namespace BookStoreTests
{
    [TestClass]
    public class BookRecommenderTests
    {
        private BookRecommender recommender;

        [TestInitialize]
        public void Setup()
        {
            var bookCat = new BookCatalog();
            recommender = new BookRecommender(bookCat);
        }

        // ---------- Test Cases ----------

        [TestMethod]
        public void Test_GetGuessYouLikeBooks_WithExistingUser()
        {
            int userId = 1;

            var result = recommender.GetGuessYouLikeBooks(userId);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsLessThanOrEqualTo(3, result.Count, "Should return at most 3 books");
        }

        [TestMethod]
        public void Test_GetGuessYouLikeBooks_WithNoPurchaseHistory()
        {
            int fakeUserId = -999;

            var result = recommender.GetGuessYouLikeBooks(fakeUserId);

            Assert.IsNotNull(result, "Fallback should still return something");
            Assert.IsTrue(result.Count > 0, "Should return fallback random books");
        }

        [TestMethod]
        public void Test_GetMostPopularBooks_ReturnsTop3()
        {
            var result = recommender.GetMostPopularBooks();

            Assert.IsNotNull(result);
            Assert.IsLessThanOrEqualTo(3, result.Count, "Should return at most 3 books");
        }
    }
}

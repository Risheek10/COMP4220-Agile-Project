using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using BookStoreGUI;
using ywBookStoreLIB;

namespace BookStoreTests
{
    [TestClass]
    public class RecommendBookWindowTests
    {
        private RecommendBookWindow window;

        [TestInitialize]
        public void Setup()
        {
            window = new RecommendBookWindow();

            var bookCat = new BookCatalog();
            var ds = bookCat.GetBookInfo();

            SetPrivateField(window, "dsBookCat", ds);
            SetPrivateField(window, "bookCat", bookCat);
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                field.SetValue(obj, value);
        }

        private object CallPrivateMethod(string methodName, params object[] args)
        {
            var method = typeof(RecommendBookWindow).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(window, args);
        }

        // ---------- Test Cases ----------

        [TestMethod]
        public void Test_GetGuessYouLikeBooks_WithExistingUser()
        {
            int userId = 1;

            var result = CallPrivateMethod("GetGuessYouLikeBooks", userId) as List<dynamic>;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsLessThanOrEqualTo(3, result.Count, "Should return at most 3 books");
        }

        [TestMethod]
        public void Test_GetGuessYouLikeBooks_WithNoPurchaseHistory()
        {
            int fakeUserId = -999;

            var result = CallPrivateMethod("GetGuessYouLikeBooks", fakeUserId) as List<dynamic>;

            Assert.IsNotNull(result, "Fallback should still return something");
            Assert.IsNotEmpty(result, "Should return fallback random books");
        }

        [TestMethod]
        public void Test_GetMostPopularBooks_ReturnsTop3()
        {
            var result = CallPrivateMethod("GetMostPopularBooks") as List<dynamic>;

            Assert.IsNotNull(result);
            Assert.IsLessThanOrEqualTo(3, result.Count, "Should return at most 3 books");
        }
    }
}

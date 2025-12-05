using Microsoft.VisualStudio.TestTools.UnitTesting;
using ywBookStoreLIB;
using System.Data;
using System;

namespace YWUnitTest
{
    [TestClass]
    public class AdminPanelTests
    {
        private static readonly object dbLock = new object();
        private DALBookCatalog bookCatalog;
        private DALUserInfo userInfo;
        private DALOrder order; // Added for analytics tests
        private Book testBook;
        private UserData testUser;

        [TestInitialize]
        public void TestInitialize()
        {
            lock (dbLock)
            {
                bookCatalog = new DALBookCatalog();
                userInfo = new DALUserInfo();
                order = new DALOrder(); // Initialize DALOrder

                testBook = new Book
                {
                    ISBN = "1234567890",
                    CategoryID = 1,
                    Title = "Test Book",
                    Author = "Test Author",
                    Price = 10.00m,
                    Year = 2023,
                    Edition = "1E",
                    Publisher = "Test Publisher"
                };

                testUser = new UserData
                {
                    UserID = new Random().Next(10000, 99999),
                    UserName = "testuser",
                    Password = "password",
                    Type = "CU",
                    Manager = false
                };
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            lock (dbLock)
            {
                bookCatalog.DeleteBook(testBook.ISBN);
                userInfo.DeleteUser(testUser.UserID);
            }
        }

        [TestMethod]
        public void TestAddBook()
        {
            bool result = bookCatalog.AddBook(testBook);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUpdateBook()
        {
            bookCatalog.AddBook(testBook);
            testBook.Title = "Updated Test Book";
            bool result = bookCatalog.UpdateBook(testBook);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteBook()
        {
            bookCatalog.AddBook(testBook);
            bool result = bookCatalog.DeleteBook(testBook.ISBN);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAddUser()
        {
            bool result = userInfo.AddUser(testUser);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            userInfo.AddUser(testUser);
            testUser.UserName = "updateduser";
            bool result = userInfo.UpdateUser(testUser);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            userInfo.AddUser(testUser);
            bool result = userInfo.DeleteUser(testUser.UserID);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSearchUsers()
        {
            userInfo.AddUser(testUser); // Add a test user
            DataSet ds = userInfo.GetUsers("testuser", "All"); // Search for the test user
            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables["Users"].Rows.Count > 0);
            Assert.AreEqual("testuser", ds.Tables["Users"].Rows[0]["UserName"]);
            userInfo.DeleteUser(testUser.UserID); // Clean up
        }

        [TestMethod]
        public void TestFilterUsers()
        {
            userInfo.AddUser(testUser); // Add a test user
            DataSet ds = userInfo.GetUsers("", "CU"); // Filter for customer users
            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables["Users"].Rows.Count > 0);
            Assert.AreEqual("CU", ds.Tables["Users"].Rows[0]["Type"]);
            userInfo.DeleteUser(testUser.UserID); // Clean up
        }

        [TestMethod]
        public void TestResetUserPassword()
        {
            userInfo.AddUser(testUser); // Add a test user
            string newPassword = "newpassword";
            testUser.Password = newPassword;
            bool updateResult = userInfo.UpdateUser(testUser); // Update password
            Assert.IsTrue(updateResult);

            int loginResult = userInfo.LogIn(testUser.UserName, newPassword); // Try logging in with new password
            Assert.IsTrue(loginResult > 0);
            userInfo.DeleteUser(testUser.UserID); // Clean up
        }

        [TestMethod]
        public void TestGetTotalUsers()
        {
            int totalUsers = userInfo.GetTotalUsers();
            Assert.IsTrue(totalUsers >= 0);
        }

        [TestMethod]
        public void TestGetTotalSales()
        {
            decimal totalSales = order.GetTotalSales();
            Assert.IsTrue(totalSales >= 0m);
        }

        [TestMethod]
        public void TestGetTotalOrders()
        {
            int totalOrders = order.GetTotalOrders();
            Assert.IsTrue(totalOrders >= 0);
        }

        [TestMethod]
        public void TestGetTotalBooksInStock()
        {
            int totalBooks = bookCatalog.GetTotalBooksInStock();
            Assert.IsTrue(totalBooks >= 0);
        }

        [TestMethod]
        public void TestGetSalesDataForLast30Days()
        {
            DataTable salesData = order.GetSalesDataForLast30Days();
            Assert.IsNotNull(salesData);
            // Assert.IsTrue(salesData.Rows.Count >= 0); // Data may not exist
        }

        [TestMethod]
        public void TestGetTop10BestSellingBooks()
        {
            DataTable topSellers = bookCatalog.GetTop10BestSellingBooks();
            Assert.IsNotNull(topSellers);
            // Assert.IsTrue(topSellers.Rows.Count >= 0); // Data may not exist
        }

        [TestMethod]
        public void TestSettingsManager()
        {
            // Test boolean settings
            SettingsManager.EnableUserRegistration = true;
            Assert.IsTrue(SettingsManager.EnableUserRegistration);
            SettingsManager.EnableUserRegistration = false;
            Assert.IsFalse(SettingsManager.EnableUserRegistration);

            SettingsManager.RequireEmailVerification = true;
            Assert.IsTrue(SettingsManager.RequireEmailVerification);
            SettingsManager.RequireEmailVerification = false;
            Assert.IsFalse(SettingsManager.RequireEmailVerification);

            // Test string settings
            SettingsManager.SmtpServer = "test.smtp.com";
            Assert.AreEqual("test.smtp.com", SettingsManager.SmtpServer);

            // Test int settings
            SettingsManager.SmtpPort = 123;
            Assert.AreEqual(123, SettingsManager.SmtpPort);
        }
    }
}
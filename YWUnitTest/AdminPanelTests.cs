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
        private Book testBook;
        private UserData testUser;

        [TestInitialize]
        public void TestInitialize()
        {
            lock (dbLock)
            {
                bookCatalog = new DALBookCatalog();
                userInfo = new DALUserInfo();

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
        public void TestExportUserList()
        {
            AdminWindow adminWindow = new AdminWindow();
            // Assuming UsersDataGrid is populated, which it should be on Load
            // Since ExportUserList_Click uses SaveFileDialog, we cannot directly test file content.
            // We can only assert that no exception is thrown during the process.
            try
            {
                adminWindow.ExportUserList_Click(null, null);
                Assert.IsTrue(true, "ExportUserList_Click did not throw an exception.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"ExportUserList_Click threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestSearchBooks()
        {
            bookCatalog.AddBook(testBook); // Add a test book
            DataSet ds = bookCatalog.GetBookInfo("Test Book", "All"); // Search for the test book
            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables["Books"].Rows.Count > 0);
            Assert.AreEqual("Test Book", ds.Tables["Books"].Rows[0]["Title"]);
            bookCatalog.DeleteBook(testBook.ISBN); // Clean up
        }

        [TestMethod]
        public void TestFilterBooksByStockStatus()
        {
            bookCatalog.AddBook(testBook); // Add a test book
            DataSet ds = bookCatalog.GetBookInfo("", "Low Stock"); // Filter for low stock books
            Assert.IsNotNull(ds);
            Assert.IsTrue(ds.Tables["Books"].Rows.Count > 0);
            Assert.IsTrue((int)ds.Tables["Books"].Rows[0]["InStock"] <= 10);
            bookCatalog.DeleteBook(testBook.ISBN); // Clean up
        }

        [TestMethod]
        public void TestBulkUpload()
        {
            AdminWindow adminWindow = new AdminWindow();
            // Since BulkUpload_Click uses OpenFileDialog, we cannot directly test file content.
            // We can only assert that no exception is thrown during the process.
            try
            {
                adminWindow.BulkUpload_Click(null, null);
                Assert.IsTrue(true, "BulkUpload_Click did not throw an exception.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"BulkUpload_Click threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestLowStockAlert()
        {
            AdminWindow adminWindow = new AdminWindow();
            // Since LowStockAlert_Click displays a MessageBox, we cannot directly test its output.
            // We can only assert that no exception is thrown during the process.
            try
            {
                adminWindow.LowStockAlert_Click(null, null);
                Assert.IsTrue(true, "LowStockAlert_Click did not throw an exception.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"LowStockAlert_Click threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestExportInventory()
        {
            AdminWindow adminWindow = new AdminWindow();
            // Since ExportInventory_Click uses SaveFileDialog, we cannot directly test file content.
            // We can only assert that no exception is thrown during the process.
            try
            {
                adminWindow.ExportInventory_Click(null, null);
                Assert.IsTrue(true, "ExportInventory_Click did not throw an exception.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"ExportInventory_Click threw an exception: {ex.Message}");
            }
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
            decimal totalSales = new DALOrder().GetTotalSales();
            Assert.IsTrue(totalSales >= 0m);
        }

        [TestMethod]
        public void TestGetTotalOrders()
        {
            int totalOrders = new DALOrder().GetTotalOrders();
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
            DataTable salesData = new DALOrder().GetSalesDataForLast30Days();
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

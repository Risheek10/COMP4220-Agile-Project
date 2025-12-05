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
    }
}

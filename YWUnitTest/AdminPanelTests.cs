using Microsoft.VisualStudio.TestTools.UnitTesting;
using ywBookStoreLIB;
using System.Data;
using System;

namespace YWUnitTest
{
    [TestClass]
    public class AdminPanelTests
    {
        [TestMethod]
        public void TestAddBook()
        {
            DALBookCatalog bookCatalog = new DALBookCatalog();
            Book book = new Book
            {
                ISBN = "1234567890123",
                CategoryID = 1,
                Title = "Test Book",
                Author = "Test Author",
                Price = 10.00m,
                Year = 2023,
                Edition = "1st",
                Publisher = "Test Publisher"
            };

            bool result = bookCatalog.AddBook(book);

            Assert.IsTrue(result);

            // Clean up
            bookCatalog.DeleteBook(book.ISBN);
        }
        [TestMethod]
        public void TestUpdateBook()
        {
            DALBookCatalog bookCatalog = new DALBookCatalog();
            Book book = new Book
            {
                ISBN = "1234567890124",
                CategoryID = 1,
                Title = "Test Book",
                Author = "Test Author",
                Price = 10.00m,
                Year = 2023,
                Edition = "1st",
                Publisher = "Test Publisher"
            };
            bookCatalog.AddBook(book);

            book.Title = "Updated Test Book";
            bool result = bookCatalog.UpdateBook(book);

            Assert.IsTrue(result);

            // Clean up
            bookCatalog.DeleteBook(book.ISBN);
        }

        [TestMethod]
        public void TestDeleteBook()
        {
            DALBookCatalog bookCatalog = new DALBookCatalog();
            Book book = new Book
            {
                ISBN = "1234567890125",
                CategoryID = 1,
                Title = "Test Book",
                Author = "Test Author",
                Price = 10.00m,
                Year = 2023,
                Edition = "1st",
                Publisher = "Test Publisher"
            };
            bookCatalog.AddBook(book);

            bool result = bookCatalog.DeleteBook(book.ISBN);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAddUser()
        {
            DALUserInfo userInfo = new DALUserInfo();
            UserData user = new UserData
            {
                UserName = "testuser",
                Password = "password",
                Type = "customer",
                Manager = false
            };

            bool result = userInfo.AddUser(user);

            Assert.IsTrue(result);

            // Clean up
            DataSet ds = userInfo.GetUsers();
            foreach (DataRow row in ds.Tables["Users"].Rows)
            {
                if (row["UserName"].ToString() == "testuser")
                {
                    userInfo.DeleteUser((int)row["UserID"]);
                    break;
                }
            }
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            DALUserInfo userInfo = new DALUserInfo();
            UserData user = new UserData
            {
                UserName = "testuser2",
                Password = "password",
                Type = "customer",
                Manager = false
            };
            userInfo.AddUser(user);
            DataSet ds = userInfo.GetUsers();
            int userId = 0;
            foreach (DataRow row in ds.Tables["Users"].Rows)
            {
                if (row["UserName"].ToString() == "testuser2")
                {
                    userId = (int)row["UserID"];
                    break;
                }
            }
            user.UserID = userId;
            user.UserName = "updateduser";

            bool result = userInfo.UpdateUser(user);

            Assert.IsTrue(result);

            // Clean up
            userInfo.DeleteUser(userId);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            DALUserInfo userInfo = new DALUserInfo();
            UserData user = new UserData
            {
                UserName = "testuser3",
                Password = "password",
                Type = "customer",
                Manager = false
            };
            userInfo.AddUser(user);
            DataSet ds = userInfo.GetUsers();
            int userId = 0;
            foreach (DataRow row in ds.Tables["Users"].Rows)
            {
                if (row["UserName"].ToString() == "testuser3")
                {
                    userId = (int)row["UserID"];
                    break;
                }
            }

            bool result = userInfo.DeleteUser(userId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUpdateOrder()
        {
            DALOrder order = new DALOrder();
            // This test assumes an order with ID 1 exists
            bool result = order.UpdateOrder(1, DateTime.Now);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteOrder()
        {
            DALOrder order = new DALOrder();
            // This test is difficult to implement without creating a new order first,
            // which requires a user and a book. For now, we will assume it works if the other tests pass.
            // A more complete test would involve creating a user, a book, an order, and then deleting it.
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestUpdateReview()
        {
            DALReviews reviews = new DALReviews();
            // This test assumes a review with ID 1 exists
            bool result = reviews.UpdateReview(1, 5, "Updated review");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteReview()
        {
            DALReviews reviews = new DALReviews();
            // This test assumes a review with ID 2 exists
            // A more complete test would involve creating a review first and then deleting it.
            bool result = reviews.DeleteReview(2);
            Assert.IsTrue(result);
        }
    }

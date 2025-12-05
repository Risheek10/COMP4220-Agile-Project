using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ywBookStoreLIB
{
    [TestClass]
    [TestCategory("Database")]
    public class AdminLoginTests
    {
        private UserData userdata;

        [TestInitialize]
        public void Setup()
        {
            userdata = new UserData();
        }

        [TestMethod]
        public void Login_SAUser_SetsRoleToAdmin()
        {
            // dclark in seed data: Type = "SA", Manager = 0
            var loggedIn = userdata.LogIn("dclark", "dc1234");

            Assert.IsTrue(loggedIn, "Expected login to succeed for SA user.");
            Assert.AreEqual(1, userdata.UserID, "Expected UserID 1 for dclark.");
            Assert.AreEqual("Admin", userdata.Role, true, "SA user should have Admin role.");
        }

        [TestMethod]
        public void Login_RGUser_SetsRoleToRegular()
        {
            // klink in seed data: Type = "RG", Manager = 0
            var loggedIn = userdata.LogIn("klink", "kl1234");

            Assert.IsTrue(loggedIn, "Expected login to succeed for RG user.");
            Assert.AreEqual("Regular", userdata.Role, true, "RG user should have Regular role.");
        }

        [TestMethod]
        public void Login_ManagerUser_SetsRoleToAdmin()
        {
            // mjones in seed data has Manager = 1 (and Type = "SA"), should be Admin
            var loggedIn = userdata.LogIn("mjones", "mj1234");

            Assert.IsTrue(loggedIn, "Expected login to succeed for manager user.");
            Assert.AreEqual("Admin", userdata.Role, true, "Manager flag should result in Admin role.");
        }
    }
}
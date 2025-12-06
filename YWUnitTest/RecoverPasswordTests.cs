using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using ywBookStoreLIB;

namespace ywBookStoreLIB
{
    /// <summary>
    /// Tests for the password recovery flow (class-partitioned).
    /// These tests exercise DAL user lookup and credential update that the
    /// GUI recoverPasswordDialog uses to identify an account and persist a new password.
    /// Tests touch the real database referenced by Settings.Default.ywConnectionString.
    /// They attempt to restore state after modification.
    /// </summary>
    [TestClass]
    public class RecoverPasswordTests
    {
        private readonly string _connectionString =
            ywBookStoreLIB.Properties.Settings.Default.ywConnectionString;

        private string ReadPasswordFromDb(string userName)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new SqlCommand("SELECT [Password] FROM [UserData] WHERE [UserName] = @UserName", conn);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    conn.Open();
                    var val = cmd.ExecuteScalar();
                    return val == null || val == DBNull.Value ? null : (string)val;
                }
                catch
                {
                    return null;
                }
            }
        }

        // Class: DALUserInfo (corresponds to GUI: username lookup used in SendCode)
        [TestMethod]
        public void DAL_GetUserIdByUserName_ExistingAndNonExisting()
        {
            var dal = new DALUserInfo();

            // existing user from sample data
            int idValid = dal.GetUserIdByUserName("dclark");
            Assert.IsGreaterThan(0, idValid, "Expected to find user 'dclark' in database.");

            // non-existing user
            int idNotValid = dal.GetUserIdByUserName("this_user_should_not_exist_123");
            Assert.IsLessThanOrEqualTo(0, idNotValid, "Expected non-existing username to return <= 0.");
        }

        // Class: DALUserInfo.UpdateCredentials (corresponds to GUI: VerifyButton updates password)
        [TestMethod]
        public void DAL_UpdateCredentials_SucceedsAndRestoresOriginal()
        {
            var dal = new DALUserInfo();
            var userData = new UserData();

            string userName = "dclark";

            // Ensure user exists
            int userId = dal.GetUserIdByUserName(userName);
            Assert.IsGreaterThan(0, userId, $"Test requires existing user '{userName}' in DB.");

            // Read original password so we can restore it
            string original = ReadPasswordFromDb(userName);
            if (original == null)
                Assert.Inconclusive("Could not read original password from DB; aborting test.");

            // Choose a temporary password different from original
            string tempPassword = original == "tmpPass$1" ? "tmpPass$2" : "tmpPass$1";

            bool changed = false;
            try
            {
                // Update to temp
                bool rc = dal.UpdateCredentials(userId, userName, tempPassword);
                Assert.IsTrue(rc, "UpdateCredentials should return true when updating an existing user.");

                // Verify login works with new password via UserData.LogIn
                bool loginOk = userData.LogIn(userName, tempPassword);
                Assert.IsTrue(loginOk && userData.UserID == userId, "User should be able to log in with the new password.");

                changed = true;
            }
            finally
            {
                // if password successfully changed, restore original
                if (changed)
                {
                    bool restored = dal.UpdateCredentials(userId, userName, original);
                    if (!restored)
                        Assert.Fail("Failed to restore original password after test run. Manual DB restore may be required.");
                }
            }
        }

        // invalid user id update attempt
        [TestMethod]
        public void DAL_UpdateCredentials_InvalidUserId_Fails()
        {
            var dal = new DALUserInfo();
            bool rc = dal.UpdateCredentials(-9999, "nonexistent", "irrelevant");
            Assert.IsFalse(rc, "UpdateCredentials should return false when userId does not exist.");
        }

        // High level: simulate the minimal checks the GUI does prior to UpdateCredentials:
        // - username must exist
        // - email must be syntactically valid (we test the regex used in GUI)
        [TestMethod]
        public void RecoverFlow_InputValidation_Partitioned()
        {
            // username existence
            var dal = new DALUserInfo();
            int uid = dal.GetUserIdByUserName("dclark");
            Assert.IsGreaterThan(0, uid, "Expected 'dclark' to exist for validation partition.");

            int uidMissing = dal.GetUserIdByUserName("no_such_user_9999");
            Assert.IsLessThanOrEqualTo(0, uidMissing, "Expected missing user id for non-existent username.");

            // email format checks (mirror GUI regex)
            string[] validEmails = { "user@example.com", "john.doe@domain.co", "a@b.cc" };
            string[] invalidEmails = { "", "dclarkathotmail.com", "dclark@hotmail", "dclark @ hotmail.com", "a@b.c" };

            foreach (var e in validEmails)
                Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(e, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"), $"Expected '{e}' to be treated as valid.");

            foreach (var e in invalidEmails)
                Assert.IsFalse(System.Text.RegularExpressions.Regex.IsMatch(e, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"), $"Expected '{e}' to be treated as invalid.");
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ywBookStoreLIB;

namespace ywBookStoreLIB
{
    [TestClass]
    public class ModifyPasswordTest 
    {
        [TestMethod]
        public void TestPasswordLength()
        {
            // The class of AccountValidationHelper is assumed to be a class of the software under 
            // development, but it is not available yet as a practice of TDD.
            AccountValidationHelper validator = new AccountValidationHelper();

            string inputPassword;
            bool expectedReturn;
            bool actualReturn;

            // specify the value of test inputs
            inputPassword = "short";

            // specify the value of expected outputs
            expectedReturn = false;

            // obtain the actual outputs by calling the method under testing
            actualReturn = validator.ValidatePasswordStrength(inputPassword);

            // verify the result
            Assert.AreEqual(expectedReturn, actualReturn);
        }

        [TestMethod]
        public void TestPasswordComplexity()
        {
            AccountValidationHelper validator = new AccountValidationHelper();

            string inputPassword;
            bool expectedReturn;
            bool actualReturn;

            // specify the value of test inputs
            inputPassword = "longpasswordlowercase";

            // specify the value of expected outputs
            expectedReturn = false;

            // obtain the actual outputs by calling the method under testing
            actualReturn = validator.ValidatePasswordStrength(inputPassword);

            // verify the result
            Assert.AreEqual(expectedReturn, actualReturn);
        }

        [TestMethod]
        public void TestValidPassword()
        {
            AccountValidationHelper validator = new AccountValidationHelper();

            string inputPassword;
            bool expectedReturn;
            bool actualReturn;

            // specify the value of test inputs
            inputPassword = "StrongPassword123";

            // specify the value of expected outputs
            expectedReturn = true;

            // obtain the actual outputs by calling the method under testing
            actualReturn = validator.ValidatePasswordStrength(inputPassword);

            // verify the result
            Assert.AreEqual(expectedReturn, actualReturn);
        }
    }
}
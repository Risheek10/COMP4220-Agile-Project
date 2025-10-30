using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ywBookStoreGUI
{
    [TestClass]
    public class SignUpValidatorTests
    {
        private SignUpValidator validator;

        [TestInitialize]
        public void Setup()
        {
            validator = new SignUpValidator();
        }

        #region Email Validation Tests

        [TestMethod]
        public void TestIsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Test valid email with @ and domain
            bool result = validator.IsValidEmail("user@example.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsValidEmail_MissingAtSymbol_ReturnsFalse()
        {
            // Test email without @ symbol
            bool result = validator.IsValidEmail("userexample.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidEmail_MissingDomain_ReturnsFalse()
        {
            // Test email without domain
            bool result = validator.IsValidEmail("user@");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidEmail_EmptyEmail_ReturnsFalse()
        {
            // Test empty email
            bool result = validator.IsValidEmail("");
            Assert.IsFalse(result);
        }

        #endregion

        #region Phone Validation Tests

        [TestMethod]
        public void TestIsValidPhone_TenDigits_ReturnsTrue()
        {
            // Test valid 10-digit phone number
            bool result = validator.IsValidPhone("1234567890");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsValidPhone_FormattedPhone_ReturnsTrue()
        {
            // Test formatted phone number (should still work)
            bool result = validator.IsValidPhone("(123) 456-7890");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsValidPhone_TooShort_ReturnsFalse()
        {
            // Test phone number with less than 10 digits
            bool result = validator.IsValidPhone("12345");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidPhone_TooLong_ReturnsFalse()
        {
            // Test phone number with more than 10 digits
            bool result = validator.IsValidPhone("12345678901");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidPhone_Empty_ReturnsFalse()
        {
            // Test empty phone number
            bool result = validator.IsValidPhone("");
            Assert.IsFalse(result);
        }

        #endregion

        #region Password Validation Tests

        [TestMethod]
        public void TestIsValidPassword_ValidPassword_ReturnsTrue()
        {
            // Test valid password (6+ chars, starts with letter, alphanumeric)
            bool result = validator.IsValidPassword("abc123");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsValidPassword_TooShort_ReturnsFalse()
        {
            // Test password shorter than 6 characters
            bool result = validator.IsValidPassword("abc12");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidPassword_StartsWithNumber_ReturnsFalse()
        {
            // Test password starting with number
            bool result = validator.IsValidPassword("123abc");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidPassword_ContainsSpecialChar_ReturnsFalse()
        {
            // Test password with special characters
            bool result = validator.IsValidPassword("abc@123");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsValidPassword_Empty_ReturnsFalse()
        {
            // Test empty password
            bool result = validator.IsValidPassword("");
            Assert.IsFalse(result);
        }

        #endregion

        #region Full Form Validation Tests

        [TestMethod]
        public void TestValidateSignUpForm_AllValidData_NoErrors()
        {
            // Test complete valid form data
            List<string> errors = validator.ValidateSignUpForm(
                "John", "Doe", "johndoe", "password123", "password123",
                "john@example.com", "1234567890", "123 Main St", "Toronto", "ON - Ontario"
            );

            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void TestValidateSignUpForm_EmptyFirstName_HasError()
        {
            // Test with empty first name
            List<string> errors = validator.ValidateSignUpForm(
                "", "Doe", "johndoe", "password123", "password123",
                "john@example.com", "1234567890", "123 Main St", "Toronto", "ON - Ontario"
            );

            Assert.IsTrue(errors.Count > 0);
            Assert.IsTrue(errors.Contains("First Name is required."));
        }

        [TestMethod]
        public void TestValidateSignUpForm_MismatchedPasswords_HasError()
        {
            // Test with mismatched passwords
            List<string> errors = validator.ValidateSignUpForm(
                "John", "Doe", "johndoe", "password123", "different123",
                "john@example.com", "1234567890", "123 Main St", "Toronto", "ON - Ontario"
            );

            Assert.IsTrue(errors.Count > 0);
            Assert.IsTrue(errors.Contains("Passwords do not match."));
        }

        [TestMethod]
        public void TestValidateSignUpForm_InvalidEmail_HasError()
        {
            // Test with invalid email
            List<string> errors = validator.ValidateSignUpForm(
                "John", "Doe", "johndoe", "password123", "password123",
                "invalid-email", "1234567890", "123 Main St", "Toronto", "ON - Ontario"
            );

            Assert.IsTrue(errors.Count > 0);
            Assert.IsTrue(errors.Contains("Email must contain @ and a domain."));
        }

        [TestMethod]
        public void TestValidateSignUpForm_ShortUsername_HasError()
        {
            // Test with username too short
            List<string> errors = validator.ValidateSignUpForm(
                "John", "Doe", "ab", "password123", "password123",
                "john@example.com", "1234567890", "123 Main St", "Toronto", "ON - Ontario"
            );

            Assert.IsTrue(errors.Count > 0);
            Assert.IsTrue(errors.Contains("Username must be 3-20 characters long."));
        }

        #endregion
    }
}
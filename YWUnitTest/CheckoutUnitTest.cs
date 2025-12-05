using BookStoreGUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace ywBookStoreLIB
{
    [TestClass]
    public class CheckoutUnitTest
    {
        private static string FutureExpiryMonthsAhead(int monthsAhead)
        {
            var dt = DateTime.Today.AddMonths(monthsAhead);
            return dt.ToString("MM/yy");
        }

        private static string PastExpiryMonthsAgo(int monthsAgo)
        {
            var dt = DateTime.Today.AddMonths(-monthsAgo);
            return dt.ToString("MM/yy");
        }

        private static string CurrentMonth()
        {
            var dt = DateTime.Today;
            return dt.ToString("MM/yy");
        }

        // Shipping tests
        [TestMethod]
        public void Shipping_Valid_ReturnsTrue()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("John Doe", "123 Street", "City", "A1B2C3");
            Assert.IsTrue(ok, err);
        }
        [TestMethod]
        public void Shipping_EmptyName_ReturnsTrue()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("", "123 Street", "City", "12345");
            Assert.IsFalse(ok);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(err));
        }


        [TestMethod]
        public void Shipping_InvalidPostal_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("John", "123 Street", "City", "A");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "postal");
        }

        [TestMethod]
        public void Shipping_LongPostal_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("John", "123 Street", "City", "A1B2C3A1BA1B2C3A1B");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "postal");
        }

        [TestMethod]
        public void Shipping_MissingAddress_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("John", "", "City", "A1B2C3A1B");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "address");
        }

        // Payment tests

        [TestMethod]
        public void Payment_Valid_ReturnsTrue()
        {
            var expiry = FutureExpiryMonthsAhead(6);
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John Card", "4242424242424242", expiry, "123");
            Assert.IsTrue(ok, err);
        }

        [TestMethod]
        public void Payment_MissingFields_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("", "4242424242424242", FutureExpiryMonthsAhead(3), "123");
            Assert.IsFalse(ok);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(err));
        }

        [TestMethod]
        public void Payment_InvalidCardNumber_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "abcd-1234", FutureExpiryMonthsAhead(3), "123");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "card number");
        }

        [TestMethod]
        public void Payment_InvalidLength_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "424242424242", FutureExpiryMonthsAhead(1), "123");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "card number");
        }


        [TestMethod]
        public void Payment_InvalidExpiryFormat_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424242", "2025-12", "123");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "MM/YY");
        }

        [TestMethod]
        public void Payment_ExpiredCard_ReturnsFalse()
        {
            var expiry = PastExpiryMonthsAgo(1);
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424242", expiry, "123");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "expired");
        }

        [TestMethod]
        public void Payment_ExpiryCurrentMonth_ReturnsTrue()
        {
            var expiry = CurrentMonth();
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424242", expiry, "123");
            Assert.IsTrue(ok, err);
        }

        [TestMethod]
        public void Payment_InvalidCvv_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424242", FutureExpiryMonthsAhead(3), "12");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "CVV");
        }

        [TestMethod]
        public void Payment_InvalidCvvLetters_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424242", FutureExpiryMonthsAhead(3), "12A");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "CVV");
        }

        [TestMethod]
        public void Payment_LongName_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("very long name / address / title", "4242424242424242", FutureExpiryMonthsAhead(3), "12A");
            Debug.WriteLine(err);
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "too long");
        }

        [TestMethod]
        public void Payment_IncludesSpaces_ReturnsTrue()
        {
            var expiry = FutureExpiryMonthsAhead(1);
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242 4242 4242 4242", expiry, "123");
            Assert.IsTrue(ok, err);
        }

    }
}

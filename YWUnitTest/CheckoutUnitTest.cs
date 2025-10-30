using BookStoreGUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

        [TestMethod]
        public void TestMethod1()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("John Doe", "123 Street", "City", "A1B2C3");
            Assert.IsTrue(ok, err);
        }
        [TestMethod]
        public void Shipping_Valid_ReturnsTrue()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("", "123 Street", "City", "12345");
            Assert.IsFalse(ok);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(err));
        }


        [TestMethod]
        public void Shipping_InvalidPostal_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidateShippingInput("John", "123", "City", "A");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "Postal");
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
        public void Payment_InvalidExpiryFormat_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424", "2025-12", "123");
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
        public void Payment_InvalidCvv_ReturnsFalse()
        {
            var (ok, err) = CheckoutValidator.ValidatePaymentInput("John", "4242424242424242", FutureExpiryMonthsAhead(3), "12");
            Assert.IsFalse(ok);
            StringAssert.Contains(err, "CVV");
        }

    }
}

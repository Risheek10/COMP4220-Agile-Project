using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookStoreGUI
{
    // UI-free validator so it can be unit tested.
    public static class CheckoutValidator
    {
        // Returns (IsValid, ErrorMessage). ErrorMessage is empty on success.
        public static (bool IsValid, string Error) ValidateShippingInput(string fullName, string streetName, string city, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(streetName) ||
                string.IsNullOrWhiteSpace(city) ||
                string.IsNullOrWhiteSpace(postalCode))
            {
                return (false, "Please fill in all Shipping Address fields.");
            }

            var postal = postalCode.Trim();
            if (postal.Length < 3 || postal.Length > 10)
            {
                return (false, "Please enter a valid Postal Code.");
            }

            return (true, string.Empty);
        }

        public static (bool IsValid, string Error) ValidatePaymentInput(string cardName, string cardNum, string expiryT, string cvvT)
        {
            if (string.IsNullOrWhiteSpace(cardName) ||
                string.IsNullOrWhiteSpace(cardNum) ||
                string.IsNullOrWhiteSpace(expiryT) ||
                string.IsNullOrWhiteSpace(cvvT))
            {
                return (false, "Please fill in all payment fields.");
            }

            var cardNumber = cardNum.Replace(" ", "").Replace("-", "");
            if (!cardNumber.All(char.IsDigit) || cardNumber.Length < 13 || cardNumber.Length > 19)
            {
                return (false, "Please enter a valid card number (digits only).");
            }

            var expiry = expiryT.Trim();
            if (!Regex.IsMatch(expiry, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                return (false, "Expiry date must be in MM/YY format.");
            }

            try
            {
                int month = int.Parse(expiry.Substring(0, 2));
                int year = 2000 + int.Parse(expiry.Substring(3, 2));
                var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                if (lastDay < DateTime.Today)
                {
                    return (false, "Card has expired.");
                }
            }
            catch
            {
                return (false, "Invalid expiry date.");
            }

            var cvv = cvvT.Trim();
            if (!cvv.All(char.IsDigit) || (cvv.Length != 3 && cvv.Length != 4))
            {
                return (false, "Please enter a valid CVV (3 or 4 digits).");
            }

            return (true, string.Empty);
        }
    }
}
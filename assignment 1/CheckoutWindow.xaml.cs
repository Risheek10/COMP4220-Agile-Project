using ywBookStoreLIB;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows;
    using ywBookStoreLIB;
    public partial class CheckoutWindow : Window
        {
            public double Subtotal { get; set; } = 0.0;
            private const double TaxRate = 0.10;
            BookOrder bookOrder;

        public CheckoutWindow(BookOrder BO)
            {
                InitializeComponent();
                Subtotal = BO.GetOrderTotal();
                UpdateTotals();
                bookOrder = BO;
        }

            
            /*public Boolean ValidatePaymentInput(String cardName, String cardNum, String expiryT, String cvvT)
            {
                if (string.IsNullOrWhiteSpace(cardName) ||
                       string.IsNullOrWhiteSpace(cardNum) ||
                       string.IsNullOrWhiteSpace(expiryT) ||
                       string.IsNullOrWhiteSpace(cvvT))
                {
                    MessageBox.Show("Please fill in all payment fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Card number: digits only after removing spaces, length 13-19 (typical ranges)
                var cardNumber = CardNumberTextBox.Text.Replace(" ", "").Replace("-", "");
                if (!cardNumber.All(char.IsDigit) || cardNumber.Length < 13 || cardNumber.Length > 19)
                {
                    MessageBox.Show("Please enter a valid card number (digits only).", "Invalid Card Number", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Expiry: MM/YY and not expired
                var expiry = ExpiryTextBox.Text.Trim();
                if (!Regex.IsMatch(expiry, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                {
                    MessageBox.Show("Expiry date must be in MM/YY format.", "Invalid Expiry", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                try
                {
                    int month = int.Parse(expiry.Substring(0, 2));
                    int year = 2000 + int.Parse(expiry.Substring(3, 2));
                    // last day of expiry month
                    var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    if (lastDay < DateTime.Today)
                    {
                        MessageBox.Show("Card has expired. Please use a valid card.", "Card Expired", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid expiry date.", "Invalid Expiry", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // CVV: 3 or 4 digits
                var cvv = CvvBox.Password.Trim();
                if (!cvv.All(char.IsDigit) || (cvv.Length != 3 && cvv.Length != 4))
                {
                    MessageBox.Show("Please enter a valid CVV (3 or 4 digits).", "Invalid CVV", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

            return true;
        }
            public Boolean ValidateShippingInput(String fullName, String streetName, String city, String postalCode)
            {
                if (string.IsNullOrWhiteSpace(fullName) ||
                      string.IsNullOrWhiteSpace(streetName) ||
                      string.IsNullOrWhiteSpace(city) ||
                      string.IsNullOrWhiteSpace(postalCode))
                {
                    MessageBox.Show("Please fill in all Shipping Address fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Optional: basic postal code sanity (letters/numbers, 3-10 chars)
                var postal = postalCode.Trim();
                if (postal.Length < 3 || postal.Length > 10)
                {
                    MessageBox.Show("Please enter a valid Postal Code.", "Invalid Postal Code", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;

            }*/

        private Boolean FeildsValidated()
            {
            // Validate billing/shipping when BillingSection is visible
            if (BillingSection.Visibility == Visibility.Visible)
            {
                var(ok, err) = CheckoutValidator.ValidateShippingInput(FullNameTextBox.Text, StreetTextBox.Text, CityTextBox.Text, PostalTextBox.Text);

                if (err != "")
                {
                    MessageBox.Show(err, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            // Validate payment when PaymentSection is visible
            if (PaymentSection.Visibility == Visibility.Visible)
            {
                var(ok, err) = CheckoutValidator.ValidatePaymentInput(CardNameTextBox.Text, CardNumberTextBox.Text, ExpiryTextBox.Text, CvvBox.Password);
                
                if (err != "")
                {
                    MessageBox.Show(err, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

            }

            return true;


        }
        
            private void UpdateTotals()
            {
                double tax = Subtotal * TaxRate;
                double total = Subtotal + tax;

                SubtotalText.Text = $"${Subtotal:F2}";
                TaxText.Text = $"${tax:F2}";
                TotalText.Text = $"${total:F2}";
            }

            private void ConfirmButton_Click(object sender, RoutedEventArgs e)
            {

                if (!FeildsValidated())
                {
                    return;
                }

                Debug.WriteLine("Order confirmed:");
                Debug.WriteLine(bookOrder.OrderItemList);
                int orderId;
                orderId = bookOrder.PlaceOrder(1);
                MessageBox.Show("Your order has been placed. Your order id is " +
                orderId.ToString());

                //MessageBox.Show("Order confirmed! Thank you for your purchase.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }

            private void NextButton_Click(object sender, RoutedEventArgs e)
            {
                if (!FeildsValidated())
                {
                    return;
                }

                BillingSection.Visibility = Visibility.Collapsed;
                PaymentSection.Visibility = Visibility.Visible;

                ConfirmButton.Visibility = Visibility.Visible;
                NextButton.Visibility = Visibility.Collapsed;

            }

            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        }

}

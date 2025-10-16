using ywBookStoreLIB;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    using System;
    using System.Diagnostics;
    using System.Windows;
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

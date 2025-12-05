using System;
using System.Windows;

namespace ywBookStoreGUI
{
    public partial class OrderDialog : Window
    {
        public DateTime OrderDate { get; private set; }

        public OrderDialog(int orderId, DateTime orderDate)
        {
            InitializeComponent();
            OrderIDTextBox.Text = orderId.ToString();
            OrderDatePicker.SelectedDate = orderDate;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatePicker.SelectedDate.HasValue)
            {
                OrderDate = OrderDatePicker.SelectedDate.Value;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a date.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

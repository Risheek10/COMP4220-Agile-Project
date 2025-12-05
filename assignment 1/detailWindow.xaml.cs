using BookStoreGUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ywBookStoreLIB;

namespace ywBookstoreGUI
{
    /// <summary>
    /// Interaction logic for detailWindow.xaml
    /// </summary>
    public partial class detailWindow : Window
    {
        BookOrder bookOrder;
        public detailWindow(BookOrder bookOrder)
        {
            InitializeComponent();
            this.bookOrder = bookOrder;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            orderItemDialog.isbnTextBox.Text = isbnLabel.Content.ToString().Substring(6,10);
            orderItemDialog.titleTextBox.Text = titleLabel.Content.ToString();
            orderItemDialog.priceTextBox.Text = priceLabel.Content.ToString();
            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();
            if (orderItemDialog.DialogResult == true)
            {
                MainWindow main = new MainWindow();
                string isbn = orderItemDialog.isbnTextBox.Text;
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
                main.orderListView.ItemsSource = bookOrder.OrderItemList;
            }
        }
    }
}

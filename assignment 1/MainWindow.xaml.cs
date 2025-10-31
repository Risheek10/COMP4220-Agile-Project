/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using ywBookStoreLIB;
using System.Collections.ObjectModel;
using ywBookStoreGUI;
using ywBookStoreLIB;
using System.Diagnostics;

namespace BookStoreGUI
{
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true)
            {
                bool loggedIn = userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password);
                if (loggedIn)
                {
                    // Show user id and role (Admin / Regular)
                    this.statusTextBlock.Text = "User #" + userData.UserID + " (" + userData.Role + ")";

                    // Show admin button only for admins (case-insensitive)
                    adminButton.Visibility = string.Equals(userData.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
                else
                {
                    // Login failed - hide admin button explicitly
                    this.statusTextBlock.Text = "Login Failed. Please Try Again.";
                    adminButton.Visibility = Visibility.Collapsed;
                    Debug.WriteLine("Failed");
                }
            }
        }
        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
        public MainWindow() { InitializeComponent(); }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"];
            bookOrder = new BookOrder();
            userData = new UserData();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;

            // Ensure admin button is hidden until a real admin logs in
            if (adminButton != null)
                adminButton.Visibility = Visibility.Collapsed;
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
            orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
            orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
            orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();
            if (orderItemDialog.DialogResult == true)
            {
                string isbn = orderItemDialog.isbnTextBox.Text;
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
            }
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }

        private void adminButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Admin functionality to be implemented later
            MessageBox.Show("Admin area (placeholder).", "Admin", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void chechoutButton_Click(object sender, RoutedEventArgs e)
        {

            if (userData.UserID <= 0)
            {
                MessageBox.Show("You must log in before you can place an order.");
                return;
            }

            CheckoutWindow checkoutDlg = new CheckoutWindow(bookOrder);
            checkoutDlg.Owner = this;
            checkoutDlg.ShowDialog();
            /*int orderId;
            orderId = bookOrder.PlaceOrder(userData.UserID);
            MessageBox.Show("Your order has been placed. Your order id is " +
            orderId.ToString());*/
        }
        private void btnRecommendBook_Click(object sender, RoutedEventArgs e)
        {
            RecommendBookWindow recommendWin = new RecommendBookWindow();
            recommendWin.ShowDialog();
        }

    }
}
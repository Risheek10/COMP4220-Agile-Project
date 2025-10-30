/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ywBookstoreGUI;
using ywBookStoreGUI;
using ywBookStoreLIB;

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
                if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true)
                    this.statusTextBlock.Text = "You are logged in as User #" +
                    userData.UserID;
                else
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
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
        private void chechoutButton_Click(object sender, RoutedEventArgs e)
        {
            int orderId;
            orderId = bookOrder.PlaceOrder(userData.UserID);
            MessageBox.Show("Your order has been placed. Your order id is " +
            orderId.ToString());
        }

        private void searchButton_Click(Object sender, RoutedEventArgs e)
        {
            string keyword = searchText.Text;
            string category = search_category.Text;
            searchData s = new searchData();
            if (s.search(keyword, category) == 1)
            {
                MessageBox.Show("Please type the keyword. ");
            }
            else if (s.search(keyword, category) == 2)
            {
                MessageBox.Show("Sorry, we cannot find the book. Please try again later.");
            }
            else if (s.search(keyword, category) == 3)
            {
                MessageBox.Show("Please type the correct format keyword try again later.");
            }
            else if (s.search(keyword, category) == 4)
            {
                MessageBox.Show("Please type the year or edition and try again.");
            }
            else
            {
                dsBookCat = s.result;
                ProductsDataGrid.ItemsSource = dsBookCat.Tables["result"].DefaultView;
            }
        }

        private void pricecheckbox_Checked(object sender, RoutedEventArgs e)
        {
            minPrice.IsEnabled = true;
            maxPrice.IsEnabled = true;
            sortButton.IsEnabled = true;
        }
        private void pricecheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            minPrice.IsEnabled = false;
            maxPrice.IsEnabled = false;
            sortButton.IsEnabled = false;
        }

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            PriceFilterData f = new PriceFilterData();
            if (f.filter(minPrice.Text, maxPrice.Text) == 0)
            {
                dsBookCat = f.result;
                ProductsDataGrid.ItemsSource = dsBookCat.Tables["result"].DefaultView;
            }
            else if (f.filter(minPrice.Text, maxPrice.Text) == 1)
            {
                MessageBox.Show("Please input the minimum price or maximum price and try again.");
            }
            else if (f.filter(minPrice.Text, maxPrice.Text) == 2)
            {
                MessageBox.Show("The format of the price with two decimal degits, please input again and try it later. ");

            }
            else if (f.filter(minPrice.Text, maxPrice.Text) == 3) {
                MessageBox.Show("Sorry, we do not have books in this price range. Pleas try again.");

            }
            else
            {
                MessageBox.Show("Sorry, something goes wrong, please try it later.");
            }
        }

    }
}
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Win32; // <--- THIS IS THE MISSING PIECE FOR THE FILE PICKER
using ywBookStoreGUI;
using ywBookStoreLIB;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;

        public MainWindow() { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"];
            bookOrder = new BookOrder();
            userData = new UserData();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;

            if (adminButton != null)
                adminButton.Visibility = Visibility.Collapsed;
        }

        // ***************************************************************
        //  USER PROFILE FEATURES (Menu & Picture)
        // ***************************************************************

        // 1. Opens the dropdown menu when you click the User circle
        private void UserProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
            }
        }

        // 2. Handles changing the Profile Picture
        private void ChangeProfilePic_Click(object sender, RoutedEventArgs e)
        {
            // Check if user is logged in first
            if (userData.UserID <= 0)
            {
                MessageBox.Show("Please login to change your profile picture.");
                return;
            }

            // Open the file chooser
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                // Find the Ellipse (circle) inside the Button structure
                // Note: This relies on you having named the ellipse "UserAvatarShape" in the XAML!
                Ellipse avatarShape = UserProfileBtn.Template.FindName("UserAvatarShape", UserProfileBtn) as Ellipse;

                if (avatarShape != null)
                {
                    ImageBrush imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(op.FileName));
                    imgBrush.Stretch = Stretch.UniformToFill;
                    avatarShape.Fill = imgBrush;
                }
            }
        }
        // ***************************************************************

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                bool loggedIn = userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password);
                if (loggedIn)
                {
                    this.statusTextBlock.Text = "User #" + userData.UserID + " (" + userData.Role + ")";

                    adminButton.Visibility = string.Equals(userData.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
                else
                {
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
                    if (adminButton != null)
                        adminButton.Visibility = Visibility.Collapsed;
                    Debug.WriteLine("Login failed for user: " + dlg.nameTextBox.Text);
                }
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            if (this.ProductsDataGrid.SelectedItems.Count == 0)
                return;
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
        }

        private void btnRecommendBook_Click(object sender, RoutedEventArgs e)
        {
            RecommendBookWindow recommendWin = new RecommendBookWindow();
            recommendWin.Owner = this;
            recommendWin.ShowDialog();
        }

        private void searchButton_Click(Object sender, RoutedEventArgs e)
        {
            string keyword = searchText.Text;
            string category = search_category.Text;
            searchData s = new searchData();
            int rc = s.search(keyword, category);
            if (rc == 1) MessageBox.Show("Please type the keyword.");
            else if (rc == 3) MessageBox.Show("Please type the correct format keyword try again later.");
            else if (rc == 4) MessageBox.Show("Please type the year or edition and try again.");
            else if (rc == 0)
            {
                dsBookCat = s.result;
                if (dsBookCat.Tables.Contains("result") && dsBookCat.Tables["result"].Rows.Count > 0)
                    ProductsDataGrid.ItemsSource = dsBookCat.Tables["result"].DefaultView;
                else
                    MessageBox.Show("Sorry, we do not have books related with this keyword. Please try again.");
            }
            else MessageBox.Show("Sorry, something goes wrong, please try it later.");
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
            int rc = f.filter(minPrice.Text, maxPrice.Text);
            if (rc == 0)
            {
                dsBookCat = f.result;
                if (dsBookCat.Tables.Contains("result") && dsBookCat.Tables["result"].Rows.Count > 0)
                    ProductsDataGrid.ItemsSource = dsBookCat.Tables["result"].DefaultView;
                else
                    MessageBox.Show("Sorry, we do not have books in this price range. Please try again.");
            }
            else if (rc == 1) MessageBox.Show("Please input the minimum price or maximum price and try again.");
            else if (rc == 2) MessageBox.Show("The format of the price must have two decimal digits, please input again and try it later.");
            else if (rc == 3) MessageBox.Show("Sorry, the price range is incorrect. Please try again.");
            else MessageBox.Show("Sorry, something goes wrong, please try it later.");
        }
    }
}
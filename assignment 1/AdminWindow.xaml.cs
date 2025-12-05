using System.Windows;
using ywBookStoreLIB;
using System.Data;
using System.Windows.Controls;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Linq;
using System;;

namespace ywBookStoreGUI
{
    public partial class AdminWindow : Window
    {
        private DALBookCatalog bookCatalog;
        private DALUserInfo userInfo;
        private DALOrder order;

        public AdminWindow()
        {
            InitializeComponent();
            bookCatalog = new DALBookCatalog();
            userInfo = new DALUserInfo();
            order = new DALOrder();
            LoadBooks();
            LoadUsers();
            LoadAnalytics();
            LoadSettings();
        }

        private void LoadBooks(string searchTerm = "", string stockStatus = "All")
        {
            DataSet ds;
            if (string.IsNullOrEmpty(searchTerm) && stockStatus == "All")
            {
                ds = bookCatalog.GetBookInfo();
            }
            else
            {
                ds = bookCatalog.GetBookInfo(searchTerm, stockStatus);
            }
            InventoryDataGrid.ItemsSource = ds.Tables["Books"].DefaultView;
        }

        private void LoadUsers(string searchTerm = "", string filterStatus = "All")
        {
            DataSet ds;
            if (string.IsNullOrEmpty(searchTerm) && filterStatus == "All")
            {
                ds = userInfo.GetUsers();
            }
            else
            {
                ds = userInfo.GetUsers(searchTerm, filterStatus);
            }
            UsersDataGrid.ItemsSource = ds.Tables["Users"].DefaultView;
        }

        private void LoadAnalytics()
        {
            TotalUsersTextBlock.Text = userInfo.GetTotalUsers().ToString();
            TotalSalesTextBlock.Text = order.GetTotalSales().ToString("C"); // Format as currency
            TotalOrdersTextBlock.Text = order.GetTotalOrders().ToString();
            BooksInStockTextBlock.Text = bookCatalog.GetTotalBooksInStock().ToString();

            RenderSalesTrendChart();
            RenderTopSellersChart();
        }

        private void RenderSalesTrendChart()
        {
            DataTable salesData = order.GetSalesDataForLast30Days();
            if (salesData != null && salesData.Rows.Count > 0)
            {
                StringBuilder chartData = new StringBuilder("Sales Trend (Last 30 Days):\n");
                foreach (DataRow row in salesData.Rows)
                {
                    chartData.AppendLine($"Date: {((DateTime)row["OrderDay"]).ToShortDateString()}, Sales: {((decimal)row["DailySales"]).ToString("C")}");
                }
                SalesTrendChartTextBlock.Text = chartData.ToString();
            }
            else
            {
                SalesTrendChartTextBlock.Text = "No sales data available for the last 30 days.";
            }
        }

        private void RenderTopSellersChart()
        {
            DataTable topSellers = bookCatalog.GetTop10BestSellingBooks();
            if (topSellers != null && topSellers.Rows.Count > 0)
            {
                StringBuilder chartData = new StringBuilder("Top 10 Best Sellers:\n");
                foreach (DataRow row in topSellers.Rows)
                {
                    chartData.AppendLine($"- {row["Title"]} (Sold: {row["TotalSold"]})");
                }
                TopSellersChartTextBlock.Text = chartData.ToString();
            }
            else
            {
                TopSellersChartTextBlock.Text = "No top selling books data available.";
            }
        }

        private void LoadSettings()
        {
            EnableUserRegistrationCheckBox.IsChecked = SettingsManager.EnableUserRegistration;
            RequireEmailVerificationCheckBox.IsChecked = SettingsManager.RequireEmailVerification;
            EnableMaintenanceModeCheckBox.IsChecked = SettingsManager.EnableMaintenanceMode;
            SendDailyReportsCheckBox.IsChecked = SettingsManager.SendDailyReports;

            SmtpServerTextBox.Text = SettingsManager.SmtpServer;
            SmtpPortTextBox.Text = SettingsManager.SmtpPort.ToString();
            AdminEmailTextBox.Text = SettingsManager.AdminEmail;
        }

        private void SaveSettings()
        {
            SettingsManager.EnableUserRegistration = EnableUserRegistrationCheckBox.IsChecked ?? false;
            SettingsManager.RequireEmailVerification = RequireEmailVerificationCheckBox.IsChecked ?? false;
            SettingsManager.EnableMaintenanceMode = EnableMaintenanceModeCheckBox.IsChecked ?? false;
            SettingsManager.SendDailyReports = SendDailyReportsCheckBox.IsChecked ?? false;

            SettingsManager.SmtpServer = SmtpServerTextBox.Text;
            SettingsManager.SmtpPort = int.Parse(SmtpPortTextBox.Text);
            SettingsManager.AdminEmail = AdminEmailTextBox.Text;
        }

        private void DisableUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedUser = (DataRowView)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                UserData user = new UserData
                {
                    UserID = (int)selectedUser["UserID"],
                    UserName = selectedUser["UserName"].ToString(),
                    Password = selectedUser["Password"].ToString(),
                    Type = "DI", // Assuming "DI" means Disabled
                    Manager = false
                };
                if (userInfo.UpdateUser(user))
                {
                    LoadUsers();
                    MessageBox.Show("User disabled successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to disable user.");
                }
            }
            else
            {
                MessageBox.Show("Please select a user to disable.", "No User Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchUserTextBox.Text == "Enter username or email" ? "" : SearchUserTextBox.Text;
            string filterStatus = (FilterUserStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "All";
            LoadUsers(searchTerm, filterStatus);
        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser_Click(sender, e);
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedUser = (DataRowView)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                ResetPasswordDialog dialog = new ResetPasswordDialog();
                if (dialog.ShowDialog() == true)
                {
                    UserData user = new UserData
                    {
                        UserID = (int)selectedUser["UserID"],
                        UserName = selectedUser["UserName"].ToString(),
                        Password = dialog.NewPassword,
                        Type = selectedUser["Type"].ToString(),
                        Manager = (bool)selectedUser["Manager"]
                    };
                    if (userInfo.UpdateUser(user))
                    {
                        LoadUsers();
                        MessageBox.Show("User password reset successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to reset user password.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to reset password.", "No User Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportUserList_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csv = new StringBuilder();

                // Add header row
                foreach (DataGridColumn column in UsersDataGrid.Columns)
                {
                    csv.Append(column.Header.ToString() + ",");
                }
                csv.AppendLine();

                // Add data rows
                foreach (DataRowView row in UsersDataGrid.ItemsSource)
                {
                    foreach (DataGridColumn column in UsersDataGrid.Columns)
                    {
                        csv.Append(row[column.Header.ToString()].ToString() + ",");
                    }
                    csv.AppendLine();
                }

                File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                MessageBox.Show("User list exported successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshInventory_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchBookTextBox.Text == "Title, ISBN, or Author" ? "" : SearchBookTextBox.Text;
            string stockStatus = (FilterStockStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "All";
            LoadBooks(searchTerm, stockStatus);
        }

        private void BulkUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string[] lines = File.ReadAllLines(openFileDialog.FileName);
                    foreach (string line in lines.Skip(1)) // Skip header row
                    {
                        string[] values = line.Split(',');
                        if (values.Length >= 8) // Ensure enough columns for Book data
                        {
                            Book book = new Book
                            {
                                ISBN = values[0],
                                CategoryID = int.Parse(values[1]),
                                Title = values[2],
                                Author = values[3],
                                Price = decimal.Parse(values[4]),
                                Year = int.Parse(values[5]),
                                Edition = values[6],
                                Publisher = values[7]
                            };
                            bookCatalog.AddBook(book);
                        }
                    }
                    LoadBooks();
                    MessageBox.Show("Books uploaded successfully.", "Bulk Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during bulk upload: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LowStockAlert_Click(object sender, RoutedEventArgs e)
        {
            DataSet ds = bookCatalog.GetBookInfo("", "Low Stock"); // Use the GetBookInfo overload to filter for low stock
            DataTable lowStockBooks = ds.Tables["Books"];

            if (lowStockBooks != null && lowStockBooks.Rows.Count > 0)
            {
                StringBuilder alertMessage = new StringBuilder("The following books are low in stock:\n");
                foreach (DataRow row in lowStockBooks.Rows)
                {
                    alertMessage.AppendLine($"- {row["Title"]} by {row["Author"]} (ISBN: {row["ISBN"]}, Stock: {row["InStock"]})");
                }
                MessageBox.Show(alertMessage.ToString(), "Low Stock Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("No books are currently low in stock.", "Low Stock Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportInventory_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csv = new StringBuilder();

                // Add header row
                foreach (DataGridColumn column in InventoryDataGrid.Columns)
                {
                    csv.Append(column.Header.ToString() + ",");
                }
                csv.AppendLine();

                // Add data rows
                foreach (DataRowView row in InventoryDataGrid.ItemsSource)
                {
                    foreach (DataGridColumn column in InventoryDataGrid.Columns)
                    {
                        csv.Append(row[column.Header.ToString()].ToString() + ",");
                    }
                    csv.AppendLine();
                }

                File.WriteAllText(saveFileDialog.FileName, csv.ToString());
                MessageBox.Show("Inventory exported successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BackupDatabase_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Database backup functionality would be implemented here. This involves complex database operations and permissions.", "Backup Database", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreDatabase_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Database restore functionality would be implemented here. This involves complex database operations and careful consideration of data integrity.", "Restore Database", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clearing logs functionality would be implemented here. This involves file system operations or database log management.", "Clear Logs", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Settings saved successfully.", "Save Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            BookDialog dialog = new BookDialog();
            if (dialog.ShowDialog() == true)
            {
                if (bookCatalog.AddBook(dialog.Book))
                {
                    LoadBooks();
                }
                else
                {
                    MessageBox.Show("Failed to add book.");
                }
            }
        }

        private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedBook = (DataRowView)InventoryDataGrid.SelectedItem;
            if (selectedBook != null)
            {
                Book book = new Book
                {
                    ISBN = selectedBook["ISBN"].ToString(),
                    CategoryID = (int)selectedBook["CategoryID"],
                    Title = selectedBook["Title"].ToString(),
                    Author = selectedBook["Author"].ToString(),
                    Price = (decimal)selectedBook["Price"],
                    Year = (int)selectedBook["Year"],
                    Edition = selectedBook["Edition"].ToString(),
                    Publisher = selectedBook["Publisher"].ToString()
                };

                BookDialog dialog = new BookDialog(book);
                if (dialog.ShowDialog() == true)
                {
                    if (bookCatalog.UpdateBook(dialog.Book))
                    {
                        LoadBooks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update book.");
                    }
                }
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedBook = (DataRowView)InventoryDataGrid.SelectedItem;
            if (selectedBook != null)
            {
                string isbn = selectedBook["ISBN"].ToString();
                if (bookCatalog.DeleteBook(isbn))
                {
                    LoadBooks();
                }
                else
                {
                    MessageBox.Show("Failed to delete book.");
                }
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            UserDialog dialog = new UserDialog();
            if (dialog.ShowDialog() == true)
            {
                if (userInfo.AddUser(dialog.User))
                {
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Failed to add user.");
                }
            }
        }

        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedUser = (DataRowView)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                UserData user = new UserData
                {
                    UserID = (int)selectedUser["UserID"],
                    UserName = selectedUser["UserName"].ToString(),
                    Password = selectedUser["Password"].ToString(),
                    Type = selectedUser["Type"].ToString(),
                    Manager = (bool)selectedUser["Manager"]
                };

                UserDialog dialog = new UserDialog(user);
                if (dialog.ShowDialog() == true)
                {
                    if (userInfo.UpdateUser(dialog.User))
                    {
                        LoadUsers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update user.");
                    }
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedUser = (DataRowView)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                int userId = (int)selectedUser["UserID"];
                if (userInfo.DeleteUser(userId))
                {
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Failed to delete user.");
                }
            }
        }
    }
}

using System.Windows;
using ywBookStoreLIB;
using System.Data;
using System.Windows.Controls;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace ywBookStoreGUI
{
    public partial class AdminWindow : Window
    {
        private DALBookCatalog bookCatalog;
        private DALUserInfo userInfo;
        public AdminWindow()
        {
            InitializeComponent();
            bookCatalog = new DALBookCatalog();
            userInfo = new DALUserInfo();
            LoadBooks();
            LoadUsers();
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

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
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
                UserDetailDialog dialog = new UserDetailDialog(user);
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a user to view details.", "No User Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            MessageBox.Show("Bulk Upload Clicked");
        }

        private void LowStockAlert_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Low Stock Alert Clicked");
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
            MessageBox.Show("Backup Database Clicked");
        }

        private void RestoreDatabase_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Restore Database Clicked");
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clear Logs Clicked");
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save Settings Clicked");
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

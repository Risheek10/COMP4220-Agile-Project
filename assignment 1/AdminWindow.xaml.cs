using System.Windows;
using ywBookStoreLIB;
using System.Data;
using System.Windows.Controls;

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

        private void LoadBooks()
        {
            DataSet ds = bookCatalog.GetBookInfo();
            BooksGrid.ItemsSource = ds.Tables["Books"].DefaultView;
        }

        private void LoadUsers()
        {
            DataSet ds = userInfo.GetUsers();
            UsersGrid.ItemsSource = ds.Tables["Users"].DefaultView;
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
            DataRowView selectedBook = (DataRowView)BooksGrid.SelectedItem;
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
            DataRowView selectedBook = (DataRowView)BooksGrid.SelectedItem;
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
            DataRowView selectedUser = (DataRowView)UsersGrid.SelectedItem;
            if (selectedUser != null)
            {
                UserData user = new UserData
                {
                    UserID = (int)selectedUser["UserID"],
                    UserName = selectedUser["UserName"].ToString(),
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
            DataRowView selectedUser = (DataRowView)UsersGrid.SelectedItem;
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

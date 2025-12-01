using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ywBookStoreGUI
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
   InitializeComponent();
    LoadData();
        }

     /// <summary>
     /// Load initial data for the admin panel
        /// </summary>
        private void LoadData()
        {
      // Load users data into UsersDataGrid
      LoadSampleUsers();

            // Load inventory data into InventoryDataGrid
LoadSampleInventory();
     }

        /// <summary>
        /// Load sample user data for demonstration
        /// </summary>
 private void LoadSampleUsers()
        {
            var users = new ObservableCollection<UserInfo>
     {
   new UserInfo { Username = "john_doe", Email = "john@example.com", Status = "Active", JoinedDate = DateTime.Now.AddDays(-30) },
      new UserInfo { Username = "jane_smith", Email = "jane@example.com", Status = "Active", JoinedDate = DateTime.Now.AddDays(-15) },
      new UserInfo { Username = "admin_user", Email = "admin@bookstore.com", Status = "Active", JoinedDate = DateTime.Now.AddDays(-60) },
              new UserInfo { Username = "inactive_user", Email = "inactive@example.com", Status = "Inactive", JoinedDate = DateTime.Now.AddDays(-90) }
            };
            UsersDataGrid.ItemsSource = users;
        }

        /// <summary>
 /// Load sample inventory data for demonstration
        /// </summary>
        private void LoadSampleInventory()
        {
            var books = new ObservableCollection<BookInfo>
     {
       new BookInfo { ISBN = "978-0-123456-78-9", Title = "C# Programming Guide", Author = "John Smith", StockQuantity = 45, Price = 49.99m },
new BookInfo { ISBN = "978-0-987654-32-1", Title = "Database Design", Author = "Jane Doe", StockQuantity = 12, Price = 59.99m },
              new BookInfo { ISBN = "978-0-555666-77-8", Title = "Web Development", Author = "Bob Wilson", StockQuantity = 3, Price = 44.99m },
    new BookInfo { ISBN = "978-0-111222-33-4", Title = "Advanced C# Topics", Author = "Mike Brown", StockQuantity = 28, Price = 54.99m }
     };
            InventoryDataGrid.ItemsSource = books;
}

        /// <summary>
        /// Retrieve users from the database
/// </summary>
        private List<UserInfo> GetUsersFromDatabase()
        {
  // TODO: Implement database call to fetch user data
         return new List<UserInfo>();
 }

        /// <summary>
        /// Retrieve inventory from the database
      /// </summary>
        private List<BookInfo> GetInventoryFromDatabase()
        {
       // TODO: Implement database call to fetch inventory data
return new List<BookInfo>();
    }
    }

    /// <summary>
    /// User data model for admin panel display
    /// </summary>
    public class UserInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime JoinedDate { get; set; }
    }

  /// <summary>
    /// Book data model for inventory management
    /// </summary>
    public class BookInfo
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}

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
using System.Windows.Shapes;
using ywBookStoreLIB;

namespace BookStoreGUI{
    /// <summary>
    /// Interaction logic for ReviewWindow.xaml
    /// </summary>
    public partial class ReviewWindow : Window
    {
        private string _isbn;
        private BookCatalog bookCat;

        public ReviewWindow(string isbn)
        {
            InitializeComponent();
            _isbn = isbn;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bookCat = new BookCatalog();

            string query = $@"
            SELECT UserID, Rating, ReviewText, CreatedAt
            FROM Reviews
            WHERE ISBN = '{_isbn}'
            ORDER BY CreatedAt DESC;
        ";

            var table = bookCat.ExecuteQuery(query);
            ReviewsDataGrid.ItemsSource = table.DefaultView;
        }
    }
}
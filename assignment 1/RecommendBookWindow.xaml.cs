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
using System.Data;
using System.Diagnostics;
using ywBookStoreLIB;
using System.Collections.ObjectModel;

namespace ywBookStoreGUI
{
    public partial class RecommendBookWindow : Window
    {
        private DataSet dsBookCat;
        private BookCatalog bookCat;
        private UserData userData;

        public RecommendBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            userData = new UserData();

            int userId = userData.UserID != 0 ? userData.UserID : 1;

            var guessYouLike = GetGuessYouLikeBooks(userId);
            guessYouLikeList.ItemsSource = guessYouLike;

            var popularBooks = GetMostPopularBooks();
            popularBooksList.ItemsSource = popularBooks;
        }

        private List<dynamic> GetGuessYouLikeBooks(int userId)
        {
            var table = dsBookCat.Tables["BookData"];
            if (table == null || table.Rows.Count == 0)
            {
                Debug.WriteLine("[DEBUG] No BookData found.");
                return new List<dynamic>();
            }

            string query = $@"
                SELECT TOP 1 b.CategoryID
                FROM Orders o
                JOIN OrderItem oi ON o.OrderID = oi.OrderID
                JOIN BookData b ON oi.ISBN = b.ISBN
                WHERE o.UserID = {userId}
                GROUP BY b.CategoryID
                ORDER BY COUNT(*) DESC";

            DataTable categoryTable = bookCat.ExecuteQuery(query);

            int? topCategoryId = null;
            if (categoryTable.Rows.Count > 0 && categoryTable.Rows[0]["CategoryID"] != DBNull.Value)
                topCategoryId = Convert.ToInt32(categoryTable.Rows[0]["CategoryID"]);

            IEnumerable<DataRow> candidateBooks;
            if (topCategoryId != null)
            {
                // Books from top category
                candidateBooks = table.AsEnumerable()
                    .Where(r => !r.IsNull("CategoryID") && r.Field<int>("CategoryID") == topCategoryId);
            }
            else
            {
                candidateBooks = table.AsEnumerable();
                Debug.WriteLine("[DEBUG] No purchase history found. Using fallback: random books from all.");
            }

            var books = candidateBooks
                .OrderBy(r => Guid.NewGuid())
                .Take(3)
                .Select(r => new
                {
                    Title = r.Field<string>("Title") ?? "(Untitled)",
                    Author = r.Field<string>("Author") ?? "(Unknown)",
                    Price = r.Field<decimal?>("Price") ?? 0m
                })
                .Cast<dynamic>()
                .ToList();

            return books;
        }

        private List<dynamic> GetMostPopularBooks()
        {
            string query = @"
                SELECT TOP 3 b.Title, b.Author, b.Price, SUM(oi.Quantity) AS TotalSold
                FROM OrderItem oi
                JOIN BookData b ON oi.ISBN = b.ISBN
                GROUP BY b.Title, b.Author, b.Price
                ORDER BY TotalSold DESC";

            DataTable result = bookCat.ExecuteQuery(query);
            List<dynamic> books;

            if (result != null && result.Rows.Count > 0)
            {
                books = result.AsEnumerable()
                    .Select(r => new
                    {
                        Title = r.Field<string>("Title") ?? "(Untitled)",
                        Author = r.Field<string>("Author") ?? "(Unknown)",
                        Price = r.Field<decimal?>("Price") ?? 0m
                    })
                    .Cast<dynamic>()
                    .ToList();
            }
            else
            {
                // Fallback: random 3 books from all
                var table = dsBookCat.Tables["BookData"];
                if (table == null || table.Rows.Count == 0)
                    return new List<dynamic>();

                books = table.AsEnumerable()
                    .OrderBy(r => Guid.NewGuid())
                    .Take(3)
                    .Select(r => new
                    {
                        Title = r.Field<string>("Title") ?? "(Untitled)",
                        Author = r.Field<string>("Author") ?? "(Unknown)",
                        Price = r.Field<decimal?>("Price") ?? 0m
                    })
                    .Cast<dynamic>()
                    .ToList();
            }

            return books;
        }
    }
}

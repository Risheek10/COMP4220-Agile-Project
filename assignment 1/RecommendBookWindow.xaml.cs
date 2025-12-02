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

namespace BookStoreGUI
{
    public partial class RecommendBookWindow : Window
    {
        private UserData userData;
        private BookRecommender recommender;

        public RecommendBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var bookCat = new BookCatalog();
            recommender = new BookRecommender(bookCat);
            userData = new UserData();

            int userId = userData.UserID != 0 ? userData.UserID : 1;

            var guessYouLike = recommender.GetGuessYouLikeBooks(userId);
            guessYouLikeList.ItemsSource = guessYouLike;

            var popularBooks = recommender.GetMostPopularBooks();
            popularBooksList.ItemsSource = popularBooks;
        }
    }
}

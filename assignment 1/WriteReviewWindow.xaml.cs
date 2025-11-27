using System;
using System.Collections.Generic;
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
using ywBookStoreLIB;

namespace BookStoreGUI
{
    public partial class WriteReviewWindow : Window
    {
        private string _isbn;
        private int _userId;
        private BookCatalog bookCat;

        public WriteReviewWindow(string isbn, int userId)
        {
            InitializeComponent();
            _isbn = isbn;
            _userId = userId;
        }

        private void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            int rating = int.Parse(((ComboBoxItem)RatingBox.SelectedItem).Content.ToString());
            string reviewText = ReviewTextBox.Text;
            int userId = 1;

            string sql = $@"
                INSERT INTO Reviews (ISBN, UserID, Rating, ReviewText, CreatedAt)
                VALUES ('{_isbn}', {userId}, {rating}, '{reviewText.Replace("'", "''")}', GETDATE())
            ";

            DatabaseHelper db = new DatabaseHelper();

            try
            {
                db.ExecuteNonQuery(sql);
                MessageBox.Show("Review submitted!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

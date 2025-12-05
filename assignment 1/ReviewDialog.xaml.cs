using System.Windows;

namespace ywBookStoreGUI
{
    public partial class ReviewDialog : Window
    {
        public int Rating { get; private set; }
        public string ReviewText { get; private set; }

        public ReviewDialog(int reviewId, int rating, string reviewText)
        {
            InitializeComponent();
            ReviewIDTextBox.Text = reviewId.ToString();
            RatingTextBox.Text = rating.ToString();
            ReviewTextTextBox.Text = reviewText;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RatingTextBox.Text, out int rating))
            {
                Rating = rating;
                ReviewText = ReviewTextTextBox.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid rating.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

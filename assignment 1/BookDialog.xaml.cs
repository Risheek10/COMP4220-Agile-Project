using System.Windows;
using ywBookStoreLIB;

namespace ywBookStoreGUI
{
    public partial class BookDialog : Window
    {
        public Book Book { get; private set; }

        public BookDialog(Book book = null)
        {
            InitializeComponent();
            if (book != null)
            {
                Book = book;
                ISBNTextBox.Text = book.ISBN;
                CategoryIDTextBox.Text = book.CategoryID.ToString();
                TitleTextBox.Text = book.Title;
                AuthorTextBox.Text = book.Author;
                PriceTextBox.Text = book.Price.ToString();
                YearTextBox.Text = book.Year.ToString();
                EditionTextBox.Text = book.Edition;
                PublisherTextBox.Text = book.Publisher;
            }
            else
            {
                Book = new Book();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Book.ISBN = ISBNTextBox.Text;
            Book.CategoryID = int.Parse(CategoryIDTextBox.Text);
            Book.Title = TitleTextBox.Text;
            Book.Author = AuthorTextBox.Text;
            Book.Price = decimal.Parse(PriceTextBox.Text);
            Book.Year = int.Parse(YearTextBox.Text);
            Book.Edition = EditionTextBox.Text;
            Book.Publisher = PublisherTextBox.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

using System.Windows;
using ywBookStoreLIB;

namespace BookStoreGUI
{
    public partial class ViewBookDetails : Window
    {
        public ViewBookDetails(BookDto book)
        {
            InitializeComponent();
            if (book != null)
                Populate(book);
        }

        private void Populate(BookDto b)
        {
            txtISBN.Text = b.ISBN ?? string.Empty;
            txtTitle.Text = b.Title ?? string.Empty;
            txtAuthor.Text = b.Author ?? string.Empty;
            txtCategory.Text = b.CategoryID?.ToString() ?? string.Empty;
            txtPrice.Text = b.Price.HasValue ? b.Price.Value.ToString("0.00") : string.Empty;
            txtSupplier.Text = b.SupplierId?.ToString() ?? string.Empty;
            txtYear.Text = b.Year ?? string.Empty;
            txtEdition.Text = b.Edition ?? string.Empty;
            txtPublisher.Text = b.Publisher ?? string.Empty;
            txtInStock.Text = b.InStock?.ToString() ?? string.Empty;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
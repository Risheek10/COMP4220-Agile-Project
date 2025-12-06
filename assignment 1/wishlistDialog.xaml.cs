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

namespace ywBookstoreGUI
{
	/// <summary>
	/// Interaction logic for wishlistDialog.xaml
	/// </summary>
	public partial class wishlistDialog : Window
	{
        private readonly BookOrder bookOrder;

		public wishlistDialog(BookOrder order)
		{
			InitializeComponent();
            this.bookOrder = order ?? throw new System.ArgumentNullException(nameof(order));
            // Bind ListView to the wishlist collection so it updates automatically
            this.wishlistListView.ItemsSource = bookOrder.WishlistItemList;
		}
		private void removeWishlistButton_Click(object sender, RoutedEventArgs e)
		{
            var selected = wishlistListView.SelectedItem as OrderItem;
            if (selected == null)
            {
                MessageBox.Show("Please select a wishlist item to remove.", "Remove Wishlist Item", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Remove from wishlist collection
            bookOrder.WishlistItemList.Remove(selected);
        }

		private void shareWishlistButton_Click(object sender, RoutedEventArgs e)
		{
            // Build a single formatted string from all wishlist items, each on its own line.
            if (bookOrder == null || bookOrder.WishlistItemList == null || bookOrder.WishlistItemList.Count == 0)
            {
                MessageBox.Show("Wishlist is empty.", "Share Wishlist", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var sb = new StringBuilder();
            foreach (var item in bookOrder.WishlistItemList)
            {
                // Format: ISBN | Title | Quantity | UnitPrice | SubTotal
                sb.AppendFormat(
                    "ISBN: {0} | Title: {1} | Quantity: {2} | UnitPrice: {3:F2} | SubTotal: {4:F2}",
                    item.BookID ?? string.Empty,
                    item.BookTitle ?? string.Empty,
                    item.Quantity,
                    item.UnitPrice,
                    item.SubTotal);
                sb.AppendLine();
            }

            string wishlistText = sb.ToString().TrimEnd(); // Remove trailing newline

            // Copy to clipboard for easy sharing and show confirmation.
            try
            {
                Clipboard.SetText(wishlistText);
                MessageBox.Show("Wishlist copied to clipboard.", "Share Wishlist", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                // If clipboard fails for any reason, still show the full text so user can copy manually.
                MessageBox.Show(wishlistText, "Wishlist (copy manually)", MessageBoxButton.OK, MessageBoxImage.Information);
            }
		}
	}
}

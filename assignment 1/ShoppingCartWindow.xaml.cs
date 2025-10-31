using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using ywBookStoreLIB;
using System.ComponentModel;

namespace ywBookStoreGUI
{
    public partial class ShoppingCartWindow : Window
    {
        private BookOrder order;
        
        public ShoppingCartWindow()
        {
            InitializeComponent();
            order = ShoppingCartManager.CurrentOrder;
            cartDataGrid.ItemsSource = order.OrderItemList;

            order.OrderItemList.CollectionChanged += OrderItemList_CollectionChanged;
            foreach (var it in order.OrderItemList)
                it.PropertyChanged += Item_PropertyChanged;

            UpdateTotal();
        }

        private void OrderItemList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (OrderItem it in e.NewItems)
                    it.PropertyChanged += Item_PropertyChanged;
            }
            if (e.OldItems != null)
            {
                foreach (OrderItem it in e.OldItems)
                    it.PropertyChanged -= Item_PropertyChanged;
            }
            UpdateTotal();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity" || e.PropertyName == "UnitPrice" || e.PropertyName == "SubTotal")
            {
                var itm = sender as OrderItem;
                if (itm != null && itm.Quantity == 0)
                {
                    order.RemoveItem(itm.BookID);
                    return;
                }
                UpdateTotal();
            }
                
        }

        private void UpdateTotal()
        {
            double total = 0;
            foreach (var item in order.OrderItemList)
                total += item.SubTotal;
            totalTextBlock.Text = "Total: " + total.ToString("C");
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string bookId)
            {
                order.RemoveItem(bookId);
            }
        }

        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Proceed to Checkout (this will be added later).", "Checkout", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
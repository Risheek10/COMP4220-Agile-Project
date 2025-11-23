using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ywBookStoreLIB;

namespace BookStoreGUI
{
    public partial class OrdersWindow : Window
    {
        public ObservableCollection<OrderModel> Orders { get; } = new ObservableCollection<OrderModel>();

        public OrdersWindow(int userId)
        {
            InitializeComponent();
            DataContext = this;
            LoadOrdersFromLibrary(userId);
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private void LoadOrdersFromLibrary(int userId)
        {
            try
            {
                var loader = new LoadOrders();
                var dtos = loader.GetOrders(userId);

                if (dtos == null || dtos.Count == 0)
                {
                    MessageBox.Show("No orders found for your account.");
                    Close();
                    return;
                }

                foreach (var dto in dtos.OrderByDescending(o => o.OrderDate))
                {
                    var vm = new OrderModel
                    {
                        OrderID = dto.OrderID,
                        OrderDate = dto.OrderDate,
                        Status = dto.Status,
                        DiscountPercent = dto.DiscountPercent
                    };

                    foreach (var item in dto.Items)
                    {
                        vm.Items.Add(new OrderItemModel
                        {
                            ISBN = item.ISBN,
                            Quantity = item.Quantity
                        });
                    }

                    Orders.Add(vm);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading orders: " + ex);
                MessageBox.Show("An error occurred while loading your orders.");
                Close();
            }
        }
    }

    // Simple view models for binding (kept in UI assembly)
    public class OrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int DiscountPercent { get; set; }
        public ObservableCollection<OrderItemModel> Items { get; } = new ObservableCollection<OrderItemModel>();
    }

    public class OrderItemModel
    {
        public string ISBN { get; set; }
        public int Quantity { get; set; }
    }
}
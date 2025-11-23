using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ywBookStoreLIB
{
    // DTOs returned by the library
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int DiscountPercent { get; set; }
        public List<OrderItemDto> Items { get; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public string ISBN { get; set; }
        public int Quantity { get; set; }
    }

    // Encapsulates loading orders from the DB
    public class LoadOrders
    {
        // Returns empty list on error or no rows
        public List<OrderDto> GetOrders(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            var connStr = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);

            const string sql = @"
SELECT o.OrderID, o.OrderDate, o.Status, o.DiscountPercent,
       oi.ISBN, oi.Quantity
FROM Orders o
LEFT JOIN OrderItem oi ON o.OrderID = oi.OrderID
WHERE o.UserID = @UserId
ORDER BY o.OrderDate DESC, o.OrderID, oi.ISBN;";

            var dt = new DataTable();
            using (var conn = connStr)
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                da.Fill(dt);
            }

            if (dt.Rows.Count == 0)
                return new List<OrderDto>();

            var groups = dt.AsEnumerable()
                .GroupBy(r => r.Field<int>("OrderID"))
                .OrderByDescending(g => g.First().Field<DateTime>("OrderDate"));

            var result = new List<OrderDto>(groups.Count());

            foreach (var g in groups)
            {
                var first = g.First();
                var order = new OrderDto
                {
                    OrderID = first.Field<int>("OrderID"),
                    OrderDate = first.Field<DateTime>("OrderDate"),
                    Status = first.Field<string>("Status") ?? string.Empty,
                    DiscountPercent = first.Table.Columns.Contains("DiscountPercent") && !first.IsNull("DiscountPercent")
                        ? Convert.ToInt32(first["DiscountPercent"])
                        : 0
                };

                foreach (var row in g)
                {
                    if (row.Table.Columns.Contains("ISBN") && !row.IsNull("ISBN"))
                    {
                        order.Items.Add(new OrderItemDto
                        {
                            ISBN = row.Field<string>("ISBN").Trim(),
                            Quantity = row.Field<int>("Quantity")
                        });
                    }
                }

                result.Add(order);
            }

            return result;
        }
    }
}
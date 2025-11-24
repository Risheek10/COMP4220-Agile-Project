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

    // DTO for BookData
    public class BookDto
    {
        public string ISBN { get; set; }
        public int? CategoryID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal? Price { get; set; }
        public int? SupplierId { get; set; }
        public string Year { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public int? InStock { get; set; }
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

        public BookDto GetBookByIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentNullException(nameof(isbn));

            // normalize
            isbn = isbn.Trim();

            var connStr = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);

            const string sql = @"
            SELECT ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher, InStock
            FROM BookData
            WHERE RTRIM(LTRIM(ISBN)) = @ISBN OR ISBN = @ISBN";

            var dt = new DataTable();
            using (var conn = connStr)
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@ISBN", isbn);
                da.Fill(dt);
            }

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            var b = new BookDto
            {
                ISBN = row["ISBN"] != DBNull.Value ? row.Field<string>("ISBN").Trim() : null,
                CategoryID = row.Table.Columns.Contains("CategoryID") && row["CategoryID"] != DBNull.Value ? (int?)Convert.ToInt32(row["CategoryID"]) : null,
                Title = row.Table.Columns.Contains("Title") && row["Title"] != DBNull.Value ? row.Field<string>("Title") : null,
                Author = row.Table.Columns.Contains("Author") && row["Author"] != DBNull.Value ? row.Field<string>("Author") : null,
                Price = row.Table.Columns.Contains("Price") && row["Price"] != DBNull.Value ? (decimal?)Convert.ToDecimal(row["Price"]) : null,
                SupplierId = row.Table.Columns.Contains("SupplierId") && row["SupplierId"] != DBNull.Value ? (int?)Convert.ToInt32(row["SupplierId"]) : null,
                Year = row.Table.Columns.Contains("Year") && row["Year"] != DBNull.Value ? row.Field<string>("Year").Trim() : null,
                Edition = row.Table.Columns.Contains("Edition") && row["Edition"] != DBNull.Value ? row.Field<string>("Edition").Trim() : null,
                Publisher = row.Table.Columns.Contains("Publisher") && row["Publisher"] != DBNull.Value ? row.Field<string>("Publisher") : null,
                InStock = row.Table.Columns.Contains("InStock") && row["InStock"] != DBNull.Value ? (int?)Convert.ToInt32(row["InStock"]) : null
            };

            return b;
        }

    }
}
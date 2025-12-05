using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace ywBookStoreLIB
{
    public class BookStockData
    {
        public Boolean IsBookStock { get; set; }
        public int quantity { get; set; }
        public Boolean stockCheck(DataRowView selectedRow)
        {

            if (selectedRow == null)// if the row is empty
            {
                return false;
            }
            else
            {
                string ISBN = selectedRow.Row.ItemArray[0].ToString();
                var conn = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);
                DataSet ds = new DataSet("Books");

                String cmdtext = "SELECT InStock FROM BookData WHERE ISBN = @isbn";
                    SqlCommand cmd = new SqlCommand(cmdtext, conn);
                    cmd.Parameters.AddWithValue("@isbn", ISBN);
                    conn.Open();
                    var stock = cmd.ExecuteScalar();
                    if (stock != null)//find the book
                    {
                        quantity = (int)stock;
                        if (quantity > 0)//is in stock
                        {
                            IsBookStock = true;
                        }
                        else//out of order
                        {
                            IsBookStock = false;
                        }
                    }
                    else // cannot find the book
                    {
                        IsBookStock = false;
                    }
            }
            return IsBookStock;
        }

    }
}

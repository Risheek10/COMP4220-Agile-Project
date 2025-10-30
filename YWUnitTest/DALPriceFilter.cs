using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    public class DALPriceFilter
    {
        public DataSet PriceFilter(string min, string max)
        {
            var conn = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);
            DataSet ds = new DataSet("Books");
            String cmdtext;

            if (String.IsNullOrEmpty(min))
            {
                cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Price <= @max";
            }
            else if(String.IsNullOrEmpty(max))
            {
                cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Price >= @min";
            }
            else
            {
                cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Price BETWEEN @min AND @max";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@min", min);
                cmd.Parameters.AddWithValue("@max", max);
                adapter.Fill(ds, "result");
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return ds;

        }
    }
}

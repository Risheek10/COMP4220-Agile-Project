using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    public class search
    {
        public DataSet search_keyword(string category, string keyword)
        {
            var conn = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);
            DataSet ds = new DataSet("Books");
            String cmdtext;
            try
            {
                switch (category)
                {
                    case "ISBN":
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE ISBN LIKE @keyword";
                        break;
                    case "Title":
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Title LIKE @keyword";
                        break;
                    case "Author":
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Author LIKE @keyword";
                        break;
                    case "Year":
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Year LIKE @keyword";
                        break;
                    case "Publisher":
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Publisher LIKE @keyword";
                        break;
                    case "Edition":
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE Edition LIKE @keyword";
                        break;
                    default:
                        cmdtext = "SELECT ISBN, Title, Author, Year, Price, Publisher, Edition FROM BookData WHERE 1=0";
                        break;
                }
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                adapter.Fill(ds, "result");
                return ds;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return ds;

        }

    }
}

using System;
using System.Data;
using System.Data.SqlClient;

using System.Diagnostics;
namespace ywBookStoreLIB
{
    public class DALOrder
    {
        SqlConnection conn;

        public DALOrder()
        {
            conn = new SqlConnection(Properties.Settings.Default.ywConnectionString);
        }

        public int Proceed2Order(string xmlOrder)
        {
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "down_PlaceOrder";
                SqlParameter inParameter = new SqlParameter();
                inParameter.ParameterName = "@xmlOrder";
                inParameter.Value = xmlOrder;
                inParameter.DbType = DbType.String;
                inParameter.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(inParameter);
                SqlParameter ReturnParameter = new SqlParameter();
                ReturnParameter.ParameterName = "@OrderID";
                ReturnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(ReturnParameter);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return (int)cmd.Parameters["@OrderID"].Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataSet GetOrders()
        {
            try
            {
                String strSQL = "Select OrderID, UserID, OrderDate from OrderData";
                SqlCommand cmdSelOrders = new SqlCommand(strSQL, conn);
                SqlDataAdapter daOrders = new SqlDataAdapter(cmdSelOrders);
                DataSet dsOrders = new DataSet("Orders");
                daOrders.Fill(dsOrders, "Orders");
                return dsOrders;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null;
        }

        public bool UpdateOrder(int orderId, DateTime orderDate)
        {
            try
            {
                String strSQL = "UPDATE OrderData SET OrderDate = @OrderDate WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool DeleteOrder(int orderId)
        {
            try
            {
                String strSQL = "DELETE FROM OrderData WHERE OrderID = @OrderID";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public decimal GetTotalSales()
        {
            try
            {
                String strSQL = "SELECT SUM(OI.Quantity * BD.Price) FROM OrderItem OI INNER JOIN BookData BD ON OI.ISBN = BD.ISBN";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return (decimal)result;
                }
                return 0m;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0m;
            }
            finally
            {
                conn.Close();
            }
        }

        public int GetTotalOrders()
        {
            try
            {
                String strSQL = "SELECT COUNT(*) FROM Orders";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                conn.Open();
                int totalOrders = (int)cmd.ExecuteScalar();
                return totalOrders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1; // Indicate error
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable GetSalesDataForLast30Days()
        {
            try
            {
                String strSQL = @"
                    SELECT 
                        CAST(O.OrderDate AS DATE) AS OrderDay,
                        SUM(OI.Quantity * BD.Price) AS DailySales
                    FROM Orders O
                    JOIN OrderItem OI ON O.OrderID = OI.OrderID
                    JOIN BookData BD ON OI.ISBN = BD.ISBN
                    WHERE O.OrderDate >= DATEADD(day, -30, GETDATE())
                    GROUP BY CAST(O.OrderDate AS DATE)
                    ORDER BY OrderDay";

                SqlCommand cmd = new SqlCommand(strSQL, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

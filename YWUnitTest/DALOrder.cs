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
    }
}

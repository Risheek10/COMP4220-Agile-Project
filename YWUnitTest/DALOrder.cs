/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ywBookStoreLIB
{
    class DALOrder
    {
        public int Proceed2Order(string xmlOrder)
        {
            if (string.IsNullOrWhiteSpace(xmlOrder))
                throw new ArgumentException("xmlOrder must not be null or empty", nameof(xmlOrder));

            var connString = Properties.Settings.Default.ywConnectionString;

            try
            {
                using (var cn = new SqlConnection(connString))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "insertOrder";

                    // IMPORTANT: parameter name must match the stored procedure parameter (@xml)
                    var xmlParam = new SqlParameter("@xml", SqlDbType.Xml)
                    {
                        Direction = ParameterDirection.Input,
                        Value = xmlOrder
                    };
                    cmd.Parameters.Add(xmlParam);

                    // Capture RETURN value from the proc
                    var returnParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParam);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int orderId = 0;
                    if (returnParam.Value != DBNull.Value && returnParam.Value != null)
                    {
                        orderId = Convert.ToInt32(returnParam.Value);
                    }

                    Debug.WriteLine("Order placed successfully, OrderID: " + orderId);
                    return orderId;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}
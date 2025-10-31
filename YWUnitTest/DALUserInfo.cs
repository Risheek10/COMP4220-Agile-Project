/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ywBookStoreLIB
{
    class DALUserInfo
    {
        public int LogIn(string userName, string password)
        {
            var conn = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select UserID from UserData where "
                    + " UserName = @UserName and Password = @Password ";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                var userId = cmd.ExecuteScalar();
                if (userId != null)//valid username and password
                {
                    int id = (int)userId;
                    return id;
                }
                else { return -1; }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // New: fetch Type and Manager flag for a given user id
        public (string UserType, bool Manager) GetUserTypeAndManager(int userId)
        {
            var conn = new SqlConnection(ywBookStoreLIB.Properties.Settings.Default.ywConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT [Type], [Manager] FROM [UserData] WHERE [UserID] = @UserID";
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        string type = null;
                        if (!rdr.IsDBNull(0))
                            type = rdr.GetString(0).Trim();
                        bool manager = false;
                        if (!rdr.IsDBNull(1))
                            manager = Convert.ToBoolean(rdr.GetValue(1));
                        return (type, manager);
                    }
                }
                return (null, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return (null, false);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}

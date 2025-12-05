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
    public class DALUserInfo
    {
        SqlConnection conn;

        public DALUserInfo()
        {
            conn = new SqlConnection(Properties.Settings.Default.ywConnectionString);
        }

        public int LogIn(string loginName, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select UserID from UserData where "
                    + "UserName = @UserName and Password = @Password ";
                cmd.Parameters.AddWithValue("@UserName", loginName);
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

        public DataSet GetUsers()
        {
            try
            {
                String strSQL = "Select UserID, UserName, Type, Manager from UserData";
                SqlCommand cmdSelUsers = new SqlCommand(strSQL, conn);
                SqlDataAdapter daUsers = new SqlDataAdapter(cmdSelUsers);
                DataSet dsUsers = new DataSet("Users");
                daUsers.Fill(dsUsers, "Users");
                return dsUsers;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null;
        }

        public DataSet GetUsers(string searchTerm)
        {
            try
            {
                String strSQL = "Select UserID, UserName, Type, Manager from UserData WHERE UserName LIKE @SearchTerm";
                SqlCommand cmdSelUsers = new SqlCommand(strSQL, conn);
                cmdSelUsers.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                SqlDataAdapter daUsers = new SqlDataAdapter(cmdSelUsers);
                DataSet dsUsers = new DataSet("Users");
                daUsers.Fill(dsUsers, "Users");
                return dsUsers;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null;
        }

        public UserData GetUser(int userId)
        {
            try
            {
                String strSQL = "SELECT * FROM UserData WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new UserData
                    {
                        UserID = (int)reader["UserID"],
                        UserName = reader["UserName"].ToString(),
                        Password = reader["Password"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public bool AddUser(UserData user)
        {
            try
            {
                String strSQL = "INSERT INTO UserData (UserID, UserName, Password, Type, Manager) VALUES (@UserID, @UserName, @Password, @Type, @Manager)";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@UserID", user.UserID);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Type", user.Type);
                cmd.Parameters.AddWithValue("@Manager", user.Manager);
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

        public bool UpdateUser(UserData user)
        {
            try
            {
                String strSQL = "UPDATE UserData SET UserName = @UserName, Password = @Password, Type = @Type, Manager = @Manager WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Type", user.Type);
                cmd.Parameters.AddWithValue("@Manager", user.Manager);
                cmd.Parameters.AddWithValue("@UserID", user.UserID);
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

        public bool DeleteUser(int userId)
        {
            try
            {
                String strSQL = "DELETE FROM UserData WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
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

        public DataSet GetUsers(string searchTerm, string userType)
        {
            try
            {
                string strSQL = "Select UserID, UserName, Type, Manager from UserData WHERE UserName LIKE @SearchTerm";
                if (userType != "All")
                {
                    strSQL += " AND Type = @UserType";
                }
                SqlCommand cmdSelUsers = new SqlCommand(strSQL, conn);
                cmdSelUsers.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                if (userType != "All")
                {
                    cmdSelUsers.Parameters.AddWithValue("@UserType", userType);
                }
                SqlDataAdapter daUsers = new SqlDataAdapter(cmdSelUsers);
                DataSet dsUsers = new DataSet("Users");
                daUsers.Fill(dsUsers, "Users");
                return dsUsers;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null;
        }
    }
}

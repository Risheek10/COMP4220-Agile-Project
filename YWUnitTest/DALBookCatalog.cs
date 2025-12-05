using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ywBookStoreLIB
{
    public class DALBookCatalog
    {
        SqlConnection conn;
        DataSet dsBooks;
        public DALBookCatalog()
        {
            conn = new SqlConnection(Properties.Settings.Default.ywConnectionString);
        }
        public DataSet GetBookInfo()
        {
            try
            {
                String strSQL = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCatagory = new SqlDataAdapter(cmdSelCategory);
                dsBooks = new DataSet("Books");
                daCatagory.Fill(dsBooks, "Category");            //Get category info
                String strSQL2 = "Select ISBN, CategoryID, Title," +
                    "Author, Price, Year, Edition, Publisher from BookData";
                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "Books");                  //Get Books info
                DataRelation drCat_Book = new DataRelation("drCat_Book",
                dsBooks.Tables["Category"].Columns["CategoryID"],
                dsBooks.Tables["Books"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book);       //Set up the table relation
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return dsBooks;
        }
        public Book GetBook(string isbn)
        {
            try
            {
                String strSQL = "SELECT * FROM BookData WHERE ISBN = @ISBN";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@ISBN", isbn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Book
                    {
                        ISBN = reader["ISBN"].ToString(),
                        CategoryID = (int)reader["CategoryID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Price = (decimal)reader["Price"],
                        Year = (int)reader["Year"],
                        Edition = reader["Edition"].ToString(),
                        Publisher = reader["Publisher"].ToString()
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

        public bool AddBook(Book book)
        {
            try
            {
                String strSQL = "INSERT INTO BookData (ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher) VALUES (@ISBN, @CategoryID, @Title, @Author, @Price, @Year, @Edition, @Publisher)";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Year", book.Year);
                cmd.Parameters.AddWithValue("@Edition", book.Edition);
                cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
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

        public bool UpdateBook(Book book)
        {
            try
            {
                String strSQL = "UPDATE BookData SET CategoryID = @CategoryID, Title = @Title, Author = @Author, Price = @Price, Year = @Year, Edition = @Edition, Publisher = @Publisher WHERE ISBN = @ISBN";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@CategoryID", book.CategoryID);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Year", book.Year);
                cmd.Parameters.AddWithValue("@Edition", book.Edition);
                cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
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

        public bool DeleteBook(string isbn)
        {
            try
            {
                String strSQL = "DELETE FROM BookData WHERE ISBN = @ISBN";
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.Parameters.AddWithValue("@ISBN", isbn);
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

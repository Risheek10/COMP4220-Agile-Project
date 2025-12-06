using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace ywBookStoreLIB
{
    [TestClass]
    public class UnitTest5
    {
        BookStockData check = new BookStockData();
        DataTable dataTable = new DataTable();
        Boolean actualResult;
        Boolean expectedResult;


        [TestMethod]
        public void TestMethod1()//OUT OF ORDER
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("0135974445", "Agile Software Development, Principles, Patterns, and Practices", "Robert C. Martin", "Pearson", "1", "70.40", "2002");
            DataView view = dataTable.DefaultView;
            DataRowView row = view[0];
            expectedResult = false;
            actualResult = check.stockCheck(row);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod2()//miss the ISBN
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("", "Agile Software Development, Principles, Patterns, and Practices", "Robert C. Martin", "Pearson", "1", "70.40", "2002");
            DataView view = dataTable.DefaultView;
            DataRowView row = view[0];
            expectedResult = false;
            actualResult = check.stockCheck(row);
            Assert.AreEqual(expectedResult, actualResult);
        }
       
        [TestMethod]
        public void TestMethod3() // empty row
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("", "", "", "", "", "", "");
            DataView view = dataTable.DefaultView;
            DataRowView row = view[0];
            expectedResult = false;
            actualResult = check.stockCheck(row);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestMethod4() // cannot find the book
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("1", "Agile Software Development, Principles, Patterns, and Practices", "Robert C. Martin", "Pearson", "1", "70.40", "2002");
            DataView view = dataTable.DefaultView;
            DataRowView row = view[0];
            expectedResult = false;
            actualResult = check.stockCheck(row);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]

        public void TestMethod5() 
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("0321278658", "Extreme Programming Explained: Embrace Change", "Kent Beck and Cynthia Andres", "Addison-Wesley Professional", "2", "44.63", "2004");
            DataView view = dataTable.DefaultView;
            DataRowView row = view[0];
            expectedResult = true;
            actualResult = check.stockCheck(row);
            Assert.AreEqual(expectedResult, actualResult);

        }

    }
}

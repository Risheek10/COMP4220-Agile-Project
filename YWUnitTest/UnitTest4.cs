using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace ywBookStoreLIB
{
    [TestClass]
    public class UnitTest4
    {
        detailData check = new detailData();
        DataTable dataTable= new DataTable();
        Boolean actualResult;
        Boolean expectedResult;


        [TestMethod]
        public void TestMethod1()//success
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("0135974445", "Agile Software Development, Principles, Patterns, and Practices", "Robert C. Martin", "Pearson", "1", "70.40", "2002");
            expectedResult = true;
            actualResult = true;
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
            expectedResult = false;
            actualResult = false;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod3()// miss title
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("0135974445", "", "Robert C. Martin", "Pearson", "1", "70.40", "2002");
            expectedResult = false;
            actualResult = false;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod4()//mis price
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("0135974445", "Agile Software Development, Principles, Patterns, and Practices", "Robert C. Martin", "Pearson", "1", "", "2002");
            expectedResult = false;
            actualResult = false;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod5() // empty row
        {
            dataTable.Columns.Add("ISBN", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Publisher", typeof(string));
            dataTable.Columns.Add("Edition", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));

            dataTable.Rows.Add("", "", "", "", "", "", "");
            expectedResult = false;
            actualResult = false;
            Assert.AreEqual(expectedResult, actualResult);
        }

    }
}

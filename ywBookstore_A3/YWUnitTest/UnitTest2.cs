using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    [TestClass]
    public class UnitTest2
    {
        searchData searchdata = new searchData();
        string inputKeyword;
        string inputCategory;
        int actualResult;
        [TestMethod]
        public void TestMethod1()//search successful
        {
            inputKeyword = "Agile";
            inputCategory = "Title";
            int expectedResult = 0;
            actualResult = searchdata.search(inputKeyword, inputCategory);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod2() // search with no keyword
        {
            inputKeyword = "";
            inputCategory = "Year";
            int expectedResult = 1;
            actualResult = searchdata.search(inputKeyword, inputCategory);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod3()//search with wrong keyword
        {
            inputKeyword = "2003";
            inputCategory = "Year";
            int expectedResult = 2;
            actualResult = searchdata.search(inputKeyword, inputCategory);
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethod4()//search with whitespace
        {
            inputKeyword = "@weewref";
            inputCategory = "Title";
            int expectedResult = 3;
            actualResult = searchdata.search(inputKeyword, inputCategory);
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethod5()//search with year but the keyword contains letters
        {
            inputKeyword = "eewref";
            inputCategory = "Year";
            int expectedResult = 4;
            actualResult = searchdata.search(inputKeyword, inputCategory);
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethod6()//search with year but the keyword contains letters
        {
            inputKeyword = "eewref";
            inputCategory = "Edition";
            int expectedResult = 4;
            actualResult = searchdata.search(inputKeyword, inputCategory);
            Assert.AreEqual(expectedResult, actualResult);

        }


    }
}

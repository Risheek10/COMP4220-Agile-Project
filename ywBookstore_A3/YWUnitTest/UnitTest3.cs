using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ywBookStoreLIB
{
    [TestClass]
    public class UnitTest3
    {
        PriceFilterData price_filter = new PriceFilterData();
        string inputMin;
        string inputMax;
        int actualResult;
        int expectedResult;
        [TestMethod]
        public void TestMethod1()//both inputs are valid, and successful
        {
            inputMin = "20";
            inputMax = "50.66";
            expectedResult = 0;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod2()//both blank
        {
            inputMin = "";
            inputMax = "";
            expectedResult = 1;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod3()//wrong format (max)
        {
            inputMin = "55";
            inputMax = "30.345";
            expectedResult = 2;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod4()//wrong format (min)
        {
            inputMin = ".5";
            inputMax = "30";
            expectedResult = 2;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod5()//min > max, valid values but invalid range
        {
            inputMin = "50.34";
            inputMax = "21.56";
            expectedResult = 3;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod6()//numbers are valid but no result
        {
            inputMin = "0.1";
            inputMax = "0.2";
            expectedResult = 4;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod7()//valid formats  - 2
        {
            inputMin = "";
            inputMax = "50.66";
            expectedResult = 0;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethod8()// valid formats - 3
        {
            inputMin = "20.1";
            inputMax = "";
            expectedResult = 0;
            actualResult = price_filter.filter(inputMin, inputMax);
            Assert.AreEqual(expectedResult, actualResult);
        }


    }
}

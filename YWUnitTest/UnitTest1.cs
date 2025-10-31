
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ywBookStoreLIB
{
    [TestClass]
    public class UnitTest1
    {
        UserData userdata = new UserData();
        string inputName, inputPassword;
        int actualUserId;
        [TestMethod]
        public void TestMethod1()//log in successfully, username and password are in the database and match. 
        {
            //specify the value of test inputs
            inputName = "dclark";
            inputPassword = "dc1234";
            //specify the value of expected outputs
            Boolean expectedReturn = true;
            int expectedUserId = 1;
            //obtain the actual outputs by calling the method under testing
            Boolean actualReturn = userdata.LogIn(inputName, inputPassword);
            actualUserId = userdata.UserID;
            //verify the result;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);

        }
        [TestMethod]
        public void TestMethod2() { //black string
            //specify the value of test inputs
            inputName = "";
            inputPassword = "dc1234";
            //specify the value of expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;
            Boolean actualReturn = userdata.LogIn(inputName, inputPassword);
            actualUserId = userdata.UserID;
            //verify the result;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        public void TestMethod3()//cant find username in the database or username and password cant match
        {
            //specify the value of test inputs
            inputName = "dclark";
            inputPassword = "";
            //specify the value of expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;
            Boolean actualReturn = userdata.LogIn(inputName, inputPassword);
            actualUserId = userdata.UserID;
            //verify the result;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        public void TestMethod4()//format of password is wrong
        { //black string
            //specify the value of test inputs
            inputName = "dclark";
<<<<<<< HEAD
            inputPassword = "d1234c";
=======
            inputPassword = "1dc1234";
>>>>>>> 78ce4ab3d5188519080d469adaaa48baf2de81d0
            //specify the value of expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;
            Boolean actualReturn = userdata.LogIn(inputName, inputPassword);
            actualUserId = userdata.UserID;
            //verify the result;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }

    }
}

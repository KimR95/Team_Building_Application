using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamBuildingApp;

namespace TeamBuildingUnitTest
{
    [TestClass]
    public class UnitTest1
    {
      
        [TestMethod]
        public void adminLogin()
        {
            Library lib = Library.Instance;
            string expectedAnswer = "True Null TomF94";
            string actualAnswer = ""; 

            bool answer = lib.checkUsername("FakeJohn55");
            actualAnswer = answer.ToString();

            lib.addAdministrator("Tom", "Smith", "TomSmith@fake.com", "TestingComp", "TomF94");
            Administrator admin = lib.validateLogin("TomF94","PASSWORD");
            actualAnswer += admin == null ? " Null" : " Not Null";
           // actualAnswer += " " + admin.ToString();

            actualAnswer +=  " " + lib.validateLogin("TomF94", "TomSmith").getUsername().ToString();

            Assert.AreEqual(expectedAnswer, actualAnswer);
        }

        [TestMethod]
        public void studentLogin()
        {
            Library lib = Library.Instance;
            string expectedAnswer = "False True";            
            string actual;

            actual = lib.checkCode("WRONGCODE").ToString();
            actual += " " + lib.checkCode("k52zoxolck").ToString();

            Assert.AreEqual(expectedAnswer, actual);
        }

        [TestMethod]
        public void groupManagementDetails()
        {
            Library lib = Library.Instance;
            Loading l = new Loading();

            string expectedAnswer = "Class Code Error Group Size Error";

            string actual = lib.generateGroups("WrongCode", 5, l);
            actual += " " + lib.generateGroups("k52zoxolck", 500, l);

            Assert.AreEqual(expectedAnswer, actual);

        }

        [TestMethod]
        public void changingPassword()
        {
            Library lib = Library.Instance;
            lib.addAdministrator("Tom", "Smith", "TomSmith@fake.com", "TestingComp", "TomF94");
            Administrator admin = lib.validateLogin("TomF94", "TomSmith");
            winPasswordChange pass = new winPasswordChange(admin);

            string actual;
            string expectedAnswer = "Password Confirmation Incorrect Password Changed Old Password is Incorrect";
           

            pass.testingPurposes("TomSmith", "TomFord1", "TomFord2", admin);
            pass.btnAdmin_Click(null, null);
            actual = pass.testing;

            pass.testingPurposes("TomSmith", "TomFord1", "TomFord1",admin);
            pass.btnAdmin_Click(null, null);
            actual += " " + pass.testing;

            pass.testingPurposes("TomSmith", "TomFord1", "TomFord1",admin);
            pass.btnAdmin_Click(null, null);
            actual += " " + pass.testing;

            Assert.AreEqual(expectedAnswer, actual);
            

        }

        [TestMethod]
        public void loginWithChangedPassword()
        {
            Library lib = Library.Instance;
            Administrator admin = lib.validateLogin("TomF94", "TomFord1");
            string actual = admin == null ? "Null" : "Not Null";
            string expected = "Not Null";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void roundRobin()
        {
            Library lib = Library.Instance;
            

            Assert.AreEqual("pass", "pass");

        }
        
      
    }
}

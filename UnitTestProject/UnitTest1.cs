using System;
using System.Net.Security;
using BITClientServer.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BITClientServer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void StaffValidationTest()
        {
            var cvm = new Model.Client
            {
                FirstName = "Doug", LastName = "Patty", Phone = "0456757463", Email = "Doug.Patty@gmail.com"
            };
            Assert.IsFalse(cvm.Validation(), "Should return true as Validation is correct.");
        }

        [TestMethod]
        public void PostcodeValidation()
        {
            string wrongPostcode = "43";
            string rightPostcode = "2077";
            bool postcodeValidation = Model.ValidationClass.CheckPostcode(wrongPostcode);
            // True to stop as Validation has failed
            Assert.IsTrue(postcodeValidation, "Number should be invalid: Australian Postcodes must be 3-4 numbers");
            postcodeValidation = Model.ValidationClass.CheckPostcode(rightPostcode);
            Assert.IsFalse(postcodeValidation, "Should return false as postcode is valid for the system");
        }

        [TestMethod]
        public void PhoneValidation()
        {
            string rightPhone = "0430564737";
            string wrongPhone = "073056473";
            bool rightValidation = Model.ValidationClass.CheckPhone(rightPhone);
            bool wrongValidation = Model.ValidationClass.CheckPhone(wrongPhone);

            Assert.IsFalse(rightValidation, "Should return false");
            Assert.IsTrue(wrongValidation, "Should return true");

        }
    }
}

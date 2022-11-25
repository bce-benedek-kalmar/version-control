using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        [
            Test,
            TestCase("pwd123", false),
            TestCase("info@tejivo", false),
            TestCase("infoűtejivo.hu", false),
            TestCase("info@tejivo.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            //Arrange

            var accountController = new AccountController();

            //Act

            bool result = accountController.ValidateEmail(email);

            //Assert

            Assert.AreEqual(expectedResult, result);
        }

        [
            Test,
            TestCase("Asdfasdfg",false),            
            TestCase("ASDF12345678",false),            
            TestCase("asdf12345678",false),    
            TestCase("asdf123",false),    
            TestCase("Asdf12345",true),
        ]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            //Arrange

            var accountController = new AccountController();

            //Act

            bool result = accountController.ValidatePassword(password);

            //Assert

            Assert.AreEqual(expectedResult, result);
        }

        [
            Test,
            TestCase("info@tejivo.hu", "Abcd12345"),
            TestCase("boci@tejivo.hu", "Password12345")
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            //Arrange

            var accountController = new AccountController();

            //Act

            var result = accountController.Register(email, password);

            //Assert

            Assert.AreEqual(email, result.Email);
            Assert.AreEqual(password, result.Password);
            Assert.AreNotEqual(Guid.Empty, result.ID);
        }


        [
            Test,
            TestCase("info@tejivo", "Abcd1234"),
            TestCase("info.tejivo.hu", "Abcd1234"),
            TestCase("info@tejivo.hu", "abcd1234"),
            TestCase("info@tejivo.hu", "ABCD1234"),
            TestCase("info@tejivo.hu", "abcdABCD"),
            TestCase("info@tejivo.hu", "Ab1234"),
        ]
        public void TestRegisterValidateException(string email, string password)
        {
            //Arrange

            var accountController = new AccountController();

            //Act

            try
            {
            var result = accountController.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ValidationException>(ex);
            }
        }
    }
}

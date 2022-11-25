using NUnit.Framework;
using System;
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
    }
}

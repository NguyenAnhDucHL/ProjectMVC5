using Microsoft.VisualStudio.TestTools.UnitTesting;
using CourseOnline.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CourseOnline.Controllers.Tests
{
    [TestClass()]
    public class VerifyAccControllerTests
    {
        [TestMethod()]
        public void MenuTest()
        {
            VerifyAccController verifyAccController = new VerifyAccController();
            string actual = "Accept";
            Assert.AreEqual(actual, actual: verifyAccController.Menu("ducnase04962@fpt.edu.vn", "CMS/CourseManagement/CoursesList"));
        }
    }
}
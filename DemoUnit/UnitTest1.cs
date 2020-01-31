using System;
using CourseOnline.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoUnit
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            VerifyAccController verifyAccController = new VerifyAccController();
            Assert.AreEqual("Accept", verifyAccController.Menu("ducnase04962@fpt.edu.vn", "CMS/CourseManagement/CoursesList"));
        }
    }
}

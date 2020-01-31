using CourseOnline.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseOnlineTests3.Controllers
{
    [TestClass()]
    public class CalculateGradeControllerTests
    {
        [TestMethod()]
        public void CaculateGradeBad()
        {
            using (TestController testController = new TestController())
            {
                string expect = "Bad";
                string actual = testController.Grade(3);
                Assert.AreEqual(expect, actual);
            }
        }
        [TestMethod()]
        public void CaculateGradeAverage()
        {
            using (TestController testController = new TestController())
            {
                string expect = "Average";
                string actual = testController.Grade(6);
                Assert.AreEqual(expect, actual);
            }
        }
        [TestMethod()]
        public void CaculateGradeGood()
        {
            using (TestController testController = new TestController())
            {
                string expect = "Good";
                string actual = testController.Grade(9);
                Assert.AreEqual(expect, actual);
            }
        }
        [TestMethod()]
        public void CaculateGradeExcellent()
        {
            using (TestController testController = new TestController())
            {
                string expect = "Excellent";
                string actual = testController.Grade(10);
                Assert.AreEqual(expect, actual);
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CourseOnline.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace CourseOnline.Controllers.Tests
{
    [TestClass()]
    public class VerifyAccControllerTests
    {
        [TestMethod()] 
        public void TestAdmin()
        {
            using (VerifyAccController verifyAccController = new VerifyAccController())
            {
                string expect = "Admin";
                string actual = verifyAccController.Menu("ducnase04962@fpt.edu.vn", "CMS/CourseManagement/Registrations");
                Assert.AreEqual(expect, actual);
            }
        }

        [TestMethod()]
        public void TestTeacher()
        {
            using (VerifyAccController verifyAccController = new VerifyAccController())
            {
                string expect = "Accept";
                string actual = verifyAccController.Menu("soitrangtn123@gmail.com", "CMS/PublicContent/Posts");
                Assert.AreEqual(expect, actual);
            }
        }
        [TestMethod()]
        public void TestNotTeacherAccess()
        {
            using (VerifyAccController verifyAccController = new VerifyAccController())
            {
                string expect = "Reject";
                var actual = verifyAccController.Menu("soitrangtn123@gmail.com", "CMS/LTContent/SubjectsList");
                Assert.AreEqual(expect, actual);
            }
        }
        [TestMethod()]
        public void TestNotAdmin()
        {
            using (VerifyAccController verifyAccController = new VerifyAccController())
            {
                 string expect = "Admin";
                var actual = verifyAccController.Menu("lamnsse06118@fpt.edu.vn", "CMS/CourseManagement/Registrations");
                Assert.AreNotEqual(expect, actual);
            }
        }
    }
}
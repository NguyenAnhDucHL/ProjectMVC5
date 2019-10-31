using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class CourseController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        // GET: Course
        public ActionResult ListCourse()
        {

           //var lstSubject = 
            return View("/Views/User/Course.cshtml");
        }
    }
}
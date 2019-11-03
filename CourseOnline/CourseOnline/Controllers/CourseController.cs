using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace CourseOnline.Controllers
{
    public class CourseController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        // GET: Course
        public ActionResult ListCourse(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var lstCourse = db.Subjects.OrderBy(n => n.subject_id).Where(n => n.subject_status == "Submitted").ToPagedList(pageNumber,pageSize);
            ViewBag.lstCourse = lstCourse;
            return View("/Views/User/Course.cshtml");
        }

        public ActionResult YourCourse(int? page)
        {
            //int pageSize = 6;
            //int pageNumber = (page ?? 1);
            //var lstMyCourse = from 
            //ViewBag.lstCourse = lstCourse;
            //return View("/Views/User/Course.cshtml");
            return View();
        }
    }
}
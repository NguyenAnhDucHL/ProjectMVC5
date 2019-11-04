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
            var lstCourse = db.Subjects.OrderBy(n => n.subject_id).Where(n => n.subject_status == "Submitted").ToPagedList(pageNumber, pageSize);
            ViewBag.lstCourse = lstCourse;
            return View("/Views/User/Course.cshtml");
        }

        public ActionResult YourCourse(int? page)
        {
            string myemail = Session["Email"].ToString();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var lstMyCourse = (from s in db.Subjects
                               join c in db.Courses on s.subject_id equals c.subject_id
                               join re in db.Registrations on c.course_id equals re.course_id
                               join u in db.Users on re.user_id equals u.user_id
                               select new MySubjectModel
                               {
                                   email = u.user_email,
                                   subject_name = s.subject_name,
                                   subject_brief_info = s.subject_brief_info,
                                   picture = s.picture,
                                   subject_category = s.subject_category,
                               }).OrderBy(n => n.subject_name).Where(n => n.email == myemail).ToPagedList(pageNumber, pageSize);
                      
             ViewBag.lstMyCourse = lstMyCourse;
            return View("/Views/User/MyCourse.cshtml");
        }
    }
}
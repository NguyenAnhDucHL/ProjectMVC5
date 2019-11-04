using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {

        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        public ActionResult Home_CMS()
        {
            return View("/Views/CMS/Home.cshtml");
        }
        public ActionResult Home_User()
        {
            var lstSubject = db.Subjects.Take(5).Where(n => n.subject_status == "Submitted").ToList();      
            ViewBag.lstSubject = lstSubject;

            var lstPost = db.Posts.Take(4).Where(n => n.post_status == "Submitted").ToList();
            ViewBag.lstPost = lstPost;

            var lstTeacher = (from u in db.Users
                             join ur in db.UserRoles.Where(ur => ur.role_id == 2) on u.user_id equals ur.user_id
                             select new TeacherModel
                             {
                               user_fullname = u.user_fullname,
                               user_group = u.user_group,
                               user_image = u.user_image,
                               user_description = u.user_description,
                             }).Take(6).ToList();

            ViewBag.lstTeacher = lstTeacher;
            return View("/Views/User/Home.cshtml");
        }

    }
    // GET: Home

}
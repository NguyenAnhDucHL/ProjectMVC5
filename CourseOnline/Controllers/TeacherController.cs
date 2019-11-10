using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Models;
using PagedList;

namespace CourseOnline.Controllers
{
    public class TeacherController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        // GET: Teacher
        public ActionResult listAllTeacher(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var lstTeacher = (from u in db.Users
                              join ur in db.UserRoles.Where(ur => ur.role_id == 2) on u.user_id equals ur.user_id
                              select new UserListModel
                              {
                                  user_fullname = u.user_fullname,
                                  user_position = u.user_position,
                                  user_image = u.user_image,
                                  user_description = u.user_description,
                              }).OrderBy(n => n.user_fullname).ToPagedList(pageNumber, pageSize);
            ViewBag.lstTeacher = lstTeacher;
            return View("/Views/User/TeacherList.cshtml");
        }
    }
}
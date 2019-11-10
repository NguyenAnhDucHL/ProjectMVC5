using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        [Route("CourseList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/CourseList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllCourse()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select c.course_id, c.course_name, u.user_fullname, c.course_start_date, c.course_end_date, c.course_status " +
                            "from Course c join [User] u " +
                            "on c.teacher_id = u.user_id";

                List<CourseListModel> Courses = db.Database.SqlQuery<CourseListModel>(sql).ToList();

                int totalrows = Courses.Count;
                int totalrowsafterfiltering = Courses.Count;
                Courses = Courses.Skip(start).Take(length).ToList();
                Courses = Courses.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Courses, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
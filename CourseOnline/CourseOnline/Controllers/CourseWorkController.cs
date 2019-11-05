using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class CourseWorkController : Controller
    {
        // GET: CourseWork
        [Route("CourseWorkList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/CourseWorkList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllCourseWork()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select cw.coursework_id, c.course_name, cw.coursework_type, et.test_name, u.user_email, cw.due_date, cw.coursework_status " +
                                "from Course c join Coursework cw " +
                                "on c.course_id = cw.course_id " +
                                "join [User] u " +
                                "on c.teacher_id = u.[user_id] " +
                                "join ExamTest et " +
                                "on c.course_id = et.course_id";

                List<CourseWorkListModel> Courseworks = db.Database.SqlQuery<CourseWorkListModel>(sql).ToList();

                int totalrows = Courseworks.Count;
                int totalrowsafterfiltering = Courseworks.Count;
                Courseworks = Courseworks.Skip(start).Take(length).ToList();
                Courseworks = Courseworks.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Courseworks, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Models;

namespace CourseOnline.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        [Route("ExamList")]
        public ActionResult Index()
        {
            return View("~\\Views\\CMS\\ExamList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllExam()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select e.exam_id, e.exam_name,s.subject_name, e.exam_level, e.exam_duration, tr.pass_rate, tr.test_type  " +
                            "from Exam e join TestResult tr " +
                            "on e.exam_id = tr.exam_id " +
                            "join [Subject] s " +
                            "on e.subject_id = s.subject_id";

                List<ExamListModel> examListModels = db.Database.SqlQuery<ExamListModel>(sql).ToList();

                int totalrows = examListModels.Count;
                int totalrowsafterfiltering = examListModels.Count;
                examListModels = examListModels.Skip(start).Take(length).ToList();
                examListModels = examListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = examListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
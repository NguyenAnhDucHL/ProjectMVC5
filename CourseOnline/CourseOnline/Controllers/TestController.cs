using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        [Route("TestList")]
        public ActionResult Index()
        {
            return View("~\\Views\\CMS\\TestList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllTest()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select e.exam_id, e.exam_name, et.test_name, et.exam_note, tr.tested, tr.average  " +
                            "from Exam e join TestResult tr " +
                            "on e.exam_id = tr.exam_id " +
                            "join ExamTest et " +
                            "on e.exam_id = et.exam_id";

                List<TestListModel> testListModels = db.Database.SqlQuery<TestListModel>(sql).ToList();

                int totalrows = testListModels.Count;
                int totalrowsafterfiltering = testListModels.Count;
                testListModels = testListModels.Skip(start).Take(length).ToList();
                testListModels = testListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = testListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
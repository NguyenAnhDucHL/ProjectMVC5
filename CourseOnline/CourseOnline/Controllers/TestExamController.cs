using CourseOnline.Models;
using MvcPWy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class TestExamController : Controller
    {
        [Route("CMS/TestList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/TestList.cshtml");
        }
        // GET: TestExam

        [HttpPost]
        public ActionResult GetAllTestExam()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var testExamList = (from et in db.ExamTests
                                    join e in db.Exams on et.exam_id equals e.exam_id
                                    join tr in db.TestResults on et.exam_id equals tr.exam_id
                                    select new
                                    {
                                        test_id = et.test_id,
                                        test_name = et.test_name,
                                        test_code = et.test_code,
                                        exam_id = e.exam_id,
                                        exam_name = e.exam_name,
                                        average = tr.average,
                                        tested = tr.tested,
                                    }).ToList();

                int totalrows = testExamList.Count;
                int totalrowsafterfiltering = testExamList.Count;
                testExamList = testExamList.Skip(start).Take(length).ToList();
                testExamList = testExamList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = testExamList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class PraticeResultController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listSubject = db.Subjects.Select(s => s.subject_name).Distinct().ToList();
                ViewBag.listSubject = listSubject;

                var listDomains = db.Domains.Select(s => s.domain_name).Distinct().ToList();
                ViewBag.listDomains = listDomains;

                var listLessons = db.Lessons.Select(s => s.lesson_name).Distinct().ToList();
                ViewBag.listLessons = listLessons;

            }
            return View("~/Views/CMS/PracticeResult/PracticeResultList.cshtml");
        }

        [HttpPost]
        public ActionResult GetAllPracticeResults()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select u.[user_fullname], u.[user_email], s.[subject_name], e.[exam_name], tr.[tested_at], gr.[grade]" +
                            "from [User] u join [TestResult] tr " +
                            "on u.[user_id] = tr.[user_id] " +
                            "join [Grade] gr " +
                            "on u.user_id = gr.user_id " +
                            "join [Exam] e " +
                            "on tr.exam_id = e.exam_id " +
                            "join [Subject] s " +
                            "on e.subject_id = s.subject_id"
                            ;

                List<PracticeResultModel> practiceListModels = db.Database.SqlQuery<PracticeResultModel>(sql).ToList();

                int totalrows = practiceListModels.Count;
                int totalrowsafterfiltering = practiceListModels.Count;
                practiceListModels = practiceListModels.Skip(start).Take(length).ToList();
                practiceListModels = practiceListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = practiceListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
            
        }



    }
}
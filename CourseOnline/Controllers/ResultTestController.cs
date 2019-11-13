using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class ResultTestController : Controller
    {
        // GET: ResultTest
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listBatch = db.TestBatches.Select(s => s.batch_name).Distinct().ToList();
                ViewBag.listBatch = listBatch;
                List<TestBatch> list = db.TestBatches.Where(s => s.batch_name != null).Distinct().ToList();
                ViewBag.batch = list;
            }
            return View("/Views/CMS/Test/ResultTest.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllResult()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select tr.test_user_id, tb.batch_name, u.user_fullname, u.user_email,tr.[tested_at] , g.grade_user " +
                            "from TestResult tr " +
                            "join TestBatch tb " +
                            "on tr.batch_id = tb.batch_id " +
                            "join [User] u " +
                            "on tr.[user_id] = u.[user_id] " +
                            "join [Grade] g " +
                            "on g.[user_id] = u.[user_id]";

                List<ResultModel> gradeListModels = db.Database.SqlQuery<ResultModel>(sql).ToList();

                int totalrows = gradeListModels.Count;
                int totalrowsafterfiltering = gradeListModels.Count;
                gradeListModels = gradeListModels.Skip(start).Take(length).ToList();
                gradeListModels = gradeListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = gradeListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult FilterByBatch(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (type.Equals(All.ALL_BATCH))
                {
                    string sql = "select tr.test_user_id, tb.batch_name, u.user_fullname, u.user_email,tr.[tested_at] , g.grade_user " +
                            "from TestResult tr " +
                            "join TestBatch tb " +
                            "on tr.batch_id = tb.batch_id " +
                            "join [User] u " +
                            "on tr.[user_id] = u.[user_id] " +
                            "join [Grade] g " +
                            "on g.[user_id] = u.[user_id]";


                    List<ResultModel> testListModels = db.Database.SqlQuery<ResultModel>(sql).ToList();
                    int totalrows = testListModels.Count;
                    int totalrowsafterfiltering = testListModels.Count;
                    testListModels = testListModels.Skip(start).Take(length).ToList();
                    testListModels = testListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = testListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    string sql = "select tr.test_user_id, tb.batch_name, u.user_fullname, u.user_email,tr.[tested_at] , g.grade_user " +
                            "from TestResult tr " +
                            "join TestBatch tb " +
                            "on tr.batch_id = tb.batch_id " +
                            "join [User] u " +
                            "on tr.[user_id] = u.[user_id] " +
                            "join [Grade] g " +
                            "on g.[user_id] = u.[user_id] " +
                            "where tb.batch_name = @bname";

                    List<ResultModel> testListModels = db.Database.SqlQuery<ResultModel>(sql, new SqlParameter("bname", type)).ToList();

                    int totalrows = testListModels.Count;
                    int totalrowsafterfiltering = testListModels.Count;
                    testListModels = testListModels.Skip(start).Take(length).ToList();
                    testListModels = testListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = testListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
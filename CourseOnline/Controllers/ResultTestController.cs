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
                //var listBatch = db.TestBatches.Select(s => s.batch_name).Distinct().ToList();
                //ViewBag.listBatch = listBatch;
                //List<TestBatch> list = db.TestBatches.Where(s => s.batch_name != null).Distinct().ToList();
                //ViewBag.batch = list;
            }
            return View("/Views/CMS/Test/ResultTest.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllResult(int id)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select tr.test_user_id, u.user_fullname, u.user_email,tr.[tested_at] , g.grade_user " +
                                "from[User] u " +
                                "join TestResult tr " +
                                "on u.[user_id] = tr.[user_id] " +
                                "join Grade g " +
                                "on u.[user_id] = g.[user_id]" +
                                " where tr.test_id = @id";

                List<ResultModel> gradeListModels = db.Database.SqlQuery<ResultModel>(sql, new SqlParameter("id", id)).ToList();

                int totalrows = gradeListModels.Count;
                int totalrowsafterfiltering = gradeListModels.Count;
                gradeListModels = gradeListModels.Skip(start).Take(length).ToList();
                gradeListModels = gradeListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = gradeListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        //search
        [HttpPost]
        public ActionResult SearchByName(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var resultTestList = (from u in db.Users
                                join tr in db.TestResults on u.user_id equals tr.user_id
                                join g in db.Grades on u.user_id equals g.user_id
                                where u.user_fullname.Contains(type) ||
                                u.user_email.Contains(type)
                                select new ResultModel
                                {
                                    test_user_id = tr.test_user_id,
                                    user_fullname = u.user_fullname,
                                    user_email = u.user_email,
                                    tested_at = tr.tested_at,
                                    grade_user = g.grade_user,
                                }).ToList();
                int totalrows = resultTestList.Count;
                int totalrowsafterfiltering = resultTestList.Count;
                resultTestList = resultTestList.Skip(start).Take(length).ToList();
                resultTestList = resultTestList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = resultTestList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
        //delete
        [HttpPost]
        public ActionResult delResultTest(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var resultTest = db.TestResults.Where(tr => tr.test_user_id == id).FirstOrDefault();
                if (resultTest != null)
                {
                    db.TestResults.Remove(resultTest);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }

        }
    }
}
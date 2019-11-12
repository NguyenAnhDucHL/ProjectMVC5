﻿using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace CourseOnline.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listBatch = db.TestBatches.Select(s => s.batch_name).Distinct().ToList();
                ViewBag.listBatch = listBatch;
                var listExam = db.Exams.Select(s => s.exam_name).Distinct().ToList();
                ViewBag.listExam = listExam;
            }

                return View("/Views/CMS/Test/TestList.cshtml");
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
                string sql = "select et.test_id, e.exam_name, et.test_name, test_code, tr.tested, tr.average " +
                            "from ExamTest et left join Exam e " +
                            "on et.exam_id = e.exam_id " +
                            "left join TestResult tr " +
                            "on et.test_id = tr.test_id";

                List<TestListModel> testListModels = db.Database.SqlQuery<TestListModel>(sql).ToList();

                int totalrows = testListModels.Count;
                int totalrowsafterfiltering = testListModels.Count;
                testListModels = testListModels.Skip(start).Take(length).ToList();
                testListModels = testListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = testListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FilterByExam(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (type.Equals(All.ALL_EXAM) && type.Equals(All.ALL_TYPE))
                {
                    string sql = "select et.test_id, e.exam_name, et.test_name, test_code, tr.tested, tr.average  " +
                            "from ExamTest et left join Exam e " +
                            "on et.exam_id = e.exam_id " +
                            "left join TestResult tr " +
                            "on et.test_id = tr.test_id";


                    List<TestListModel> testListModels = db.Database.SqlQuery<TestListModel>(sql).ToList();
                    int totalrows = testListModels.Count;
                    int totalrowsafterfiltering = testListModels.Count;
                    testListModels = testListModels.Skip(start).Take(length).ToList();
                    testListModels = testListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = testListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    
                }
                else
                {
                    string sql = "select et.test_id, e.exam_name, et.test_name, et.test_code, tr.tested, tr.average  " +
                            "from ExamTest et left join Exam e " +
                            "on et.exam_id = e.exam_id " +
                            "left join TestResult tr " +
                            "on et.test_id = tr.test_id" +
                            " where e.exam_name = @ename";

                    List<TestListModel> testListModels = db.Database.SqlQuery<TestListModel>(sql, new SqlParameter("ename",type)).ToList();

                    int totalrows = testListModels.Count;
                    int totalrowsafterfiltering = testListModels.Count;
                    testListModels = testListModels.Skip(start).Take(length).ToList();
                    testListModels = testListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = testListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult AddTest()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Exam> listExam = db.Exams.Where(s => s.exam_name != null).Distinct().ToList();
                ViewBag.listExam = listExam;

            }
            return View("/Views/CMS/Test/AddTest.cshtml");
        }
        [HttpGet]
        public ActionResult Result()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listBatch = db.TestBatches.Select(s => s.batch_name).Distinct().ToList();
                ViewBag.listBatch = listBatch;
            }
                return View("/Views/CMS/Test/ResultTest.cshtml");
        }

        [HttpGet]
        public ActionResult EditTest(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Exam> listExam = db.Exams.Where(s => s.exam_name != null).Distinct().ToList();
                ViewBag.listExam = listExam;
                ExamTest ex = db.ExamTests.Where(s => s.exam_id == id).FirstOrDefault();
                ViewBag.ExamTest = ex;

                ViewBag.TestId = id;
            }
            return View("/Views/CMS/Test/EditTest.cshtml");
        }

        [HttpPost]
        public ActionResult SubmitTest(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addTest = JValue.Parse(postJson);
                    ExamTest t = new ExamTest();
                    t.exam_id = addTest.examName;
                    t.test_name = addTest.testName;
                    db.ExamTests.Add(t);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SubmitTestEdit(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtExam = JValue.Parse(postJson);
                    int id = edtExam.id;
                    ExamTest ex = db.ExamTests.Where(e => e.test_id == id).FirstOrDefault();
                    if (ex != null)
                    {
                        ex.exam_id = edtExam.examName;
                        ex.test_name = edtExam.testName;
                        ex.test_code = edtExam.testCode;
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
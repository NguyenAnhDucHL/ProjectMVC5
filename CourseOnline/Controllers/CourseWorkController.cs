using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using CourseOnline.Global.Setting;

namespace CourseOnline.Controllers
{
    public class CourseWorkController : Controller
    {
        // GET: CourseWork
        [Route("CourseWorkList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/Course/CourseWorkList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllCourseWork(int id = -1)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select cw.coursework_id, c.course_name, cw.coursework_name, u.user_email, cw.due_date, cw.coursework_status, et.test_code " +
                            "from Coursework cw " +
                            "join Course c on cw.course_id = c.course_id " +
                            "join ExamTest et on cw.test_id = et.test_id " +
                            "join[User] u on cw.usercreate_id = u.[user_id] " +
                            "where c.course_id = @id;";

                List<CourseWorkListModel> Courseworks = db.Database.SqlQuery<CourseWorkListModel>(sql, new SqlParameter("id", id)).ToList();

                int totalrows = Courseworks.Count;
                int totalrowsafterfiltering = Courseworks.Count;
                Courseworks = Courseworks.Skip(start).Take(length).ToList();
                Courseworks = Courseworks.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Courseworks, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
      
        [HttpGet]
        public ActionResult AddCourseWork(int courseid)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<ExamTest> listExamTest = db.ExamTests.Where(ex => ex.course_id == courseid).ToList();
                ViewBag.ExamTestName = listExamTest;
                Course course = db.Courses.Where(c => c.course_id == courseid).FirstOrDefault();
                ViewBag.course = course;
                return View("/Views/CMS/Course/CourseWorkAdd.cshtml");
            }
        }
        [HttpGet]
        public ActionResult CourseWorkEdit(int courseworkid, int courseid)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select et.test_id " +
                                "from Coursework cw join ExamTest et " +
                                "on cw.test_id = et.test_id where cw.coursework_id = @id";
                string sql2 = "select et.test_name " +
                                "from Coursework cw join ExamTest et " +
                                "on cw.test_id = et.test_id where cw.coursework_id = @id";
                string sql3 = "select et.test_code " +
                                "from Coursework cw join ExamTest et " +
                                "on cw.test_id = et.test_id where cw.coursework_id = @id";
                int testid = db.Database.SqlQuery<int>(sql, new SqlParameter("id", courseworkid)).FirstOrDefault();
                ViewBag.testid = testid;
                String testName = db.Database.SqlQuery<String>(sql2, new SqlParameter("id", courseworkid)).FirstOrDefault();
                ViewBag.testName = testName;
                String testCode = db.Database.SqlQuery<String>(sql3, new SqlParameter("id", courseworkid)).FirstOrDefault();
                ViewBag.testCode = testCode;
                List<ExamTest> listExamTest = db.ExamTests.Where(ex => ex.course_id == courseid).ToList();
                ViewBag.ExamTestName = listExamTest;
                Coursework coursework = db.Courseworks.Where(cw => cw.coursework_id == courseworkid).FirstOrDefault();
                Course course = db.Courses.Where(c => c.course_id == courseid).FirstOrDefault();
                ExamTest examTest = db.ExamTests.Where(et => et.test_id == coursework.test_id).FirstOrDefault();
                ViewBag.examtest = examTest;
                ViewBag.coursework = coursework;
                ViewBag.course = course;
                return View("/Views/CMS/Course/CourseWorkEdit.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitAddCourseWork(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editcoursework = JValue.Parse(postJson);

                    Coursework cw = new Coursework();
                    cw.coursework_name = editcoursework.courseworkName;
                    temp = editcoursework.courseworkStatus;
                    if (temp.Equals("Active"))
                    {
                        cw.coursework_status = true;
                    }
                    else
                    {
                        cw.coursework_status = false;
                    }
                    cw.course_id = editcoursework.course_id;
                    cw.due_date = editcoursework.courseworkDeadline;
                    cw.usercreate_id = editcoursework.usercreate_id;
                    cw.coursework_weight = editcoursework.courseworkWeight;
                    cw.coursework_link = editcoursework.courseworkLink;
                    cw.test_id = editcoursework.test_id;
                    cw.coursework_note = editcoursework.courseworkNote;
                    db.Courseworks.Add(cw);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult SubmitEditCourseWork(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editcoursework = JValue.Parse(postJson);
                    int courseworkid = editcoursework.coursework_id;
                    Coursework cw = db.Courseworks.Where(coursework => coursework.coursework_id == courseworkid).FirstOrDefault();
                    if (cw != null)
                    {
                        cw.coursework_name = editcoursework.courseworkName;
                        temp = editcoursework.courseworkStatus;
                        if (temp.Equals("Active"))
                        {
                            cw.coursework_status = true;
                        }
                        else
                        {
                            cw.coursework_status = false;
                        }
                        cw.course_id = editcoursework.course_id;
                        cw.due_date = editcoursework.courseworkDeadline;
                        cw.usercreate_id = editcoursework.usercreate_id;
                        cw.coursework_weight = editcoursework.courseworkWeight;
                        cw.coursework_link = editcoursework.courseworkLink;
                        cw.test_id = editcoursework.test_id;
                        cw.coursework_note = editcoursework.courseworkNote;
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult BackCourseWorkList(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Course course = db.Courses.Where(Course => Course.course_id == id).FirstOrDefault();
                ViewBag.course = course;
                return View("/Views/CMS/Course/CourseWorkList.cshtml");
            }
        }
    }
}
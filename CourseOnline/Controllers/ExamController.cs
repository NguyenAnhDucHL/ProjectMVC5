using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Models;
using CourseOnline.Global.Setting;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listSubject = db.Subjects.Select(s => s.subject_name).Distinct().ToList();
                ViewBag.listSubject = listSubject;
                List<Setting> listType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.EXAM_TYPES)).ToList();
                ViewBag.examType = listType;
            }
            return View("/Views/CMS/Exam/ExamList.cshtml");
        }

        //filter 
        [HttpPost]
        public ActionResult DoFilter(string filterBy = "")
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            dynamic filterByJson = JValue.Parse(filterBy);

            string subject = filterByJson.subjectName;
            string type = filterByJson.examType;

            if (subject.Equals(All.ALL_SUBJECT))
            {
                subject = "";
            }
            if (type.Equals(All.ALL_TYPE))
            {
                type = "";
            }

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var examList = (from e in db.Exams
                                    join s in db.Subjects on e.subject_id equals s.subject_id
                                    where s.subject_name.Contains(subject)
                                    && e.test_type.Contains(type)
                                    select new ExamListModel
                                    {
                                        exam_id = e.exam_id,
                                        exam_name = e.exam_name,
                                        subject_name = s.subject_name,
                                        exam_level = e.exam_level,
                                        exam_duration = e.exam_duration,
                                        pass_rate = e.pass_rate,
                                        test_type = e.test_type,
                                        exam_description = e.exam_description
                                    }).ToList();

                int totalrows = examList.Count;
                int totalrowsafterfiltering = examList.Count;
                examList = examList.Skip(start).Take(length).ToList();
                examList = examList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = examList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
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
                string sql = "select e.exam_id, e.exam_name,s.subject_name, e.exam_level, e.exam_duration, e.pass_rate, e.test_type, e.exam_description  " +
                            "from Exam e join [Subject] s " +
                            "on e.subject_id = s.subject_id";

                List<ExamListModel> examListModels = db.Database.SqlQuery<ExamListModel>(sql).ToList();

                int totalrows = examListModels.Count;
                int totalrowsafterfiltering = examListModels.Count;
                examListModels = examListModels.Skip(start).Take(length).ToList();
                examListModels = examListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = examListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddExam()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {

                List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).ToList();
                ViewBag.subjectName = listSubject;

                List<Setting> listType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.EXAM_TYPES)).ToList();
                ViewBag.examType = listType;
                return View("/Views/CMS/Exam/ExamDetail.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitExam(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addExam = JValue.Parse(postJson);
                    Exam e = new Exam();
                    e.subject_id = addExam.subjectId;
                    e.exam_name = addExam.examName;
                    e.exam_level = addExam.examLevel;
                    e.exam_duration = addExam.examDuration;
                    e.pass_rate = addExam.examPassRate;
                    e.exam_description = addExam.examDescription;
                    e.test_type = addExam.examType;
                    db.Exams.Add(e);
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
        public ActionResult delExam(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var exam = db.Exams.Where(s => s.exam_id == id).FirstOrDefault();
                if (exam != null)
                {
                    db.Exams.Remove(exam);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        [HttpGet]
        public ActionResult EditExam(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Setting> listType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.EXAM_TYPES)).ToList();
                ViewBag.examType = listType;
                Exam exam = db.Exams.Where(s => s.exam_id == id).FirstOrDefault();
                ViewBag.Exam = exam;
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;
                Setting setting = db.Settings.Where(s => s.setting_id == id).FirstOrDefault();
                ViewBag.Setting = setting;
                List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).ToList();
                ViewBag.subjectName = listSubject;
                ViewBag.id = id;
                return View("/Views/CMS/Exam/ExamEdit.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitExamEdit(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtExam = JValue.Parse(postJson);
                    int id = edtExam.id;
                    Exam ex = db.Exams.Where(e => e.exam_id == id).FirstOrDefault();
                    if(ex != null)
                    {
                        ex.subject_id = edtExam.subjectId;
                        ex.exam_name = edtExam.examName;
                        ex.exam_level = edtExam.examLevel;
                        ex.exam_duration = edtExam.examDuration;
                        ex.pass_rate = edtExam.examPassRate;
                        ex.exam_description = edtExam.examDescription;
                        ex.test_type = edtExam.examType;
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
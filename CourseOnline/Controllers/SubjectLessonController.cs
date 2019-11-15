using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Global.Setting;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class SubjectLessonController : Controller
    {
        // GET: SubjectLesson
        [Route("SubjectLessonList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/Subject/SubjectLessonList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllSubjectLesson(int id = -1)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Lesson where Lesson.subject_id = @id";


                List<Lesson> Lessons = db.Database.SqlQuery<Lesson>(sql,new SqlParameter("id", id)).ToList();

                int totalrows = Lessons.Count;
                int totalrowsafterfiltering = Lessons.Count;
                Lessons = Lessons.Skip(start).Take(length).ToList();
                Lessons = Lessons.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Lessons, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        //filter by status
        [HttpPost]
        public ActionResult FilterByLessonStatus(string type, int subject_id)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_STATUS)) // filter theo status
                {
                    if (type.Equals(Status.ACTIVE))
                    {
                        var Lessons = (from l in db.Lessons
                                     where l.lesson_status == true & l.subject_id == subject_id
                                       select new
                                     {
                                         l.lesson_id,
                                         l.subject_id,
                                         l.lesson_name,
                                         l.lesson_order,
                                         l.lesson_type,
                                         l.lesson_status,
                                         l.lesson_link,
                                         l.lesson_content,
                                     }).ToList();

                        int totalrows = Lessons.Count;
                        int totalrowsafterfiltering = Lessons.Count;
                        Lessons = Lessons.Skip(start).Take(length).ToList();
                        Lessons = Lessons.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Lessons, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var Lessons = (from l in db.Lessons
                                     where l.lesson_status == false & l.subject_id == subject_id
                                       select new
                                     {
                                         l.lesson_id,
                                         l.subject_id,
                                         l.lesson_name,
                                         l.lesson_order,
                                         l.lesson_type,
                                         l.lesson_status,
                                         l.lesson_link,
                                         l.lesson_content,
                                     }).ToList();

                        int totalrows = Lessons.Count;
                        int totalrowsafterfiltering = Lessons.Count;
                        Lessons = Lessons.Skip(start).Take(length).ToList();
                        Lessons = Lessons.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Lessons, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
                else // lay ra tat ca
                {
                    var Lessons = (from l in db.Lessons
                                   where l.subject_id == subject_id
                                   select new
                                   {
                                       l.lesson_id,
                                       l.subject_id,
                                       l.lesson_name,
                                       l.lesson_order,
                                       l.lesson_type,
                                       l.lesson_status,
                                       l.lesson_link,
                                       l.lesson_content,
                                   }).ToList();

                    int totalrows = Lessons.Count;
                    int totalrowsafterfiltering = Lessons.Count;
                    Lessons = Lessons.Skip(start).Take(length).ToList();
                    Lessons = Lessons.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = Lessons, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpGet]
        public ActionResult AddLesson(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.SubjectId = id;
                List<Setting> listLessonType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.LESSON_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.LessonType = listLessonType;

                return View("/Views/CMS/Subject/LessonAdding.cshtml");
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SubmitAddLesson(string postJson)
        {
         
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addlesson = JValue.Parse(postJson);
                    string temp = null;

                    Lesson l = new Lesson();
                    l.subject_id = addlesson.subjectId;
                    l.lesson_name = addlesson.lessonName;
                    l.lesson_type = addlesson.lessonType;
                    temp = addlesson.lessonStatus;
                    if (temp.Equals("Active"))
                    {
                        l.lesson_status = true;
                    }
                    else
                    {
                        l.lesson_status = false;
                    }
                    l.lesson_order = addlesson.lessonOrder;
                    l.lesson_link = addlesson.lessonLink;
                    l.lesson_content = addlesson.lessonContent;
                    db.Lessons.Add(l);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult BackSubjectLessonList(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.id = id;
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;
                return View("/Views/CMS/Subject/SubjectLessonList.cshtml");
            }
        }

        [HttpGet]
        public ActionResult EditLesson(int lessonId, int subjectId)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Lesson lesson = db.Lessons.Where(l => l.lesson_id == lessonId).FirstOrDefault();
                ViewBag.Lesson = lesson;
                List<Setting> listLessonType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.LESSON_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.LessonType = listLessonType;
                ViewBag.Lesson_id = lessonId;
                ViewBag.Subject_id = subjectId;
                return View("/Views/CMS/Subject/LessonEditting.cshtml");
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SubmitEditLesson(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editlesson = JValue.Parse(postJson);
                    int lesson_id = editlesson.lessonid;

                    Lesson l = db.Lessons.Where(lesson => lesson.lesson_id == lesson_id).FirstOrDefault();
                    if (l != null)
                    {
                        l.subject_id = editlesson.subjectid;
                        l.lesson_name = editlesson.lessonName;
                        l.lesson_type = editlesson.lessonType;
                        temp = editlesson.lessonStatus;
                        if (temp.Equals("Active"))
                        {
                            l.lesson_status = true;
                        }
                        else
                        {
                            l.lesson_status = false;
                        }
                        l.lesson_order = editlesson.lessonOrder;
                        l.lesson_link = editlesson.lessonLink;
                        l.lesson_content = editlesson.lessonContent;
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

        [HttpPost]
        public ActionResult deleteLesson(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var lesson = db.Lessons.Where(l => l.lesson_id == id).FirstOrDefault();
                if (lesson != null)
                {
                    db.Lessons.Remove(lesson);
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
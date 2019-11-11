using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;
using Newtonsoft.Json.Linq;

namespace CourseOnline.Controllers
{
    public class SubjectController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        // GET: Course
        [Route("SubjectList")]
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listType = db.Subjects.Select(s => s.subject_type).Distinct().ToList();
                ViewBag.subjectType = listType;

                var listCategory = db.Subjects.Select(s => s.subject_category).Distinct().ToList();
                ViewBag.subjectCategory = listCategory;

                var listStatus = db.Subjects.Select(s => s.subject_status).Distinct().ToList();
                ViewBag.subjectStatus = listStatus;
            }
            return View("/Views/CMS/Subject/SubjectList.cshtml");
        }
        public ActionResult ListSubjects(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var lstSubject = db.Subjects.OrderBy(n => n.subject_id).Where(n => n.subject_status == "Submitted").ToPagedList(pageNumber, pageSize);
            ViewBag.lstSubject = lstSubject;
            return View("/Views/User/SubjectList.cshtml");
        }

        public ActionResult SubjectDetail(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Subject subject = db.Subjects.SingleOrDefault(n => n.subject_id == id && n.subject_status == "Submitted");

            User teacher = (from u in db.Users
                           join c in db.Courses.Where(c => c.subject_id == id) on u.user_id equals c.teacher_id
                           select new UserListModel
                           {
                              user_fullname = u.user_fullname,
                              use_mobile = u.use_mobile,
                              user_email = u.user_email
                           }
                           ).FirstOrDefault();

            
            if(subject == null)
            {
                return HttpNotFound();
            }
            if(teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.teacher = teacher;
            ViewBag.subject = subject;
            return View("/Views/User/SubjectDetail.cshtml");
        }


        public ActionResult YourSubject(int? page)
        {
            string myemail = Session["Email"].ToString();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var lstMySubject = (from s in db.Subjects
                               join c in db.Courses on s.subject_id equals c.subject_id
                               join re in db.Registrations.Where(re => re.registration_status == "Approved") on c.course_id equals re.course_id
                               join u in db.Users on re.user_id equals u.user_id
                               select new MySubjectModel
                               {
                                   email = u.user_email,
                                   subject_name = s.subject_name,
                                   subject_brief_info = s.subject_brief_info,
                                   picture = s.picture,
                                   subject_category = s.subject_category,
                               }).OrderBy(n => n.subject_name).Where(n => n.email == myemail).ToPagedList(pageNumber, pageSize);
                      
             ViewBag.lstMySubject = lstMySubject;
            return View("/Views/User/MySubject.cshtml");
        }
        //filter by type subject
        [HttpPost]
        public ActionResult FilterBySubjectType(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_TYPE)) // filter theo type
                {
                    var typeSubjectList = (from s in db.Subjects
                                           where s.subject_type.Equals(type)
                                           select new
                                           {
                                               s.subject_id,
                                               s.subject_category,
                                               s.subject_name,
                                               s.subject_brief_info,
                                               s.subject_type,
                                               s.subject_status,
                                               s.subject_tag_line,
                                           }).ToList();
                    List<SubjectListModel> arrSubjectList = typeSubjectList.Select(s => new SubjectListModel
                    {
                        subject_id = s.subject_id,
                        subject_category = s.subject_category,
                        subject_name = s.subject_name,
                        subject_brief_info = s.subject_brief_info,
                        subject_type = s.subject_type,
                        subject_status = s.subject_status,
                        subject_tag_line = s.subject_tag_line
                    }).ToList();
                    foreach (var sj in arrSubjectList)
                    {
                        var lessons = (from l in db.Lessons
                                       where l.subject_id == sj.subject_id
                                       select new { l.lesson_id }).ToList();
                        sj.lesson_count = lessons.Count;
                    }



                    int totalrows = arrSubjectList.Count;
                    int totalrowsafterfiltering = arrSubjectList.Count;
                    arrSubjectList = arrSubjectList.Skip(start).Take(length).ToList();
                    arrSubjectList = arrSubjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = arrSubjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
                    var typeSubjectList = (from s in db.Subjects
                                           select new
                                           {
                                               s.subject_id,
                                               s.subject_category,
                                               s.subject_name,
                                               s.subject_brief_info,
                                               s.subject_type,
                                               s.subject_status,
                                               s.subject_tag_line,
                                           }).ToList();
                    List<SubjectListModel> arrSubjectList = typeSubjectList.Select(s => new SubjectListModel
                    {
                        subject_id = s.subject_id,
                        subject_category = s.subject_category,
                        subject_name = s.subject_name,
                        subject_brief_info = s.subject_brief_info,
                        subject_type = s.subject_type,
                        subject_status = s.subject_status,
                        subject_tag_line = s.subject_tag_line
                    }).ToList();
                    foreach (var sj in arrSubjectList)
                    {
                        var lessons = (from l in db.Lessons
                                       where l.subject_id == sj.subject_id
                                       select new { l.lesson_id }).ToList();
                        sj.lesson_count = lessons.Count;
                    }


                    int totalrows = arrSubjectList.Count;
                    int totalrowsafterfiltering = arrSubjectList.Count;
                    arrSubjectList = arrSubjectList.Skip(start).Take(length).ToList();
                    arrSubjectList = arrSubjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = arrSubjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        //filter by category subject
        [HttpPost]
        public ActionResult FilterBySubjectCategory(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_CATEGORY)) // filter theo type
                {
                    var typeSubjectList = (from s in db.Subjects
                                           where s.subject_category.Equals(type)
                                           select new
                                           {
                                               s.subject_id,
                                               s.subject_category,
                                               s.subject_name,
                                               s.subject_brief_info,
                                               s.subject_type,
                                               s.subject_status,
                                               s.subject_tag_line,
                                           }).ToList();
                    List<SubjectListModel> arrSubjectList = typeSubjectList.Select(s => new SubjectListModel
                    {
                        subject_id = s.subject_id,
                        subject_category = s.subject_category,
                        subject_name = s.subject_name,
                        subject_brief_info = s.subject_brief_info,
                        subject_type = s.subject_type,
                        subject_status = s.subject_status,
                        subject_tag_line = s.subject_tag_line
                    }).ToList();
                    foreach (var sj in arrSubjectList)
                    {
                        var lessons = (from l in db.Lessons
                                       where l.subject_id == sj.subject_id
                                       select new { l.lesson_id }).ToList();
                        sj.lesson_count = lessons.Count;
                    }



                    int totalrows = arrSubjectList.Count;
                    int totalrowsafterfiltering = arrSubjectList.Count;
                    arrSubjectList = arrSubjectList.Skip(start).Take(length).ToList();
                    arrSubjectList = arrSubjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = arrSubjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
                    var typeSubjectList = (from s in db.Subjects
                                           select new
                                           {
                                               s.subject_id,
                                               s.subject_category,
                                               s.subject_name,
                                               s.subject_brief_info,
                                               s.subject_type,
                                               s.subject_status,
                                               s.subject_tag_line,
                                           }).ToList();
                    List<SubjectListModel> arrSubjectList = typeSubjectList.Select(s => new SubjectListModel
                    {
                        subject_id = s.subject_id,
                        subject_category = s.subject_category,
                        subject_name = s.subject_name,
                        subject_brief_info = s.subject_brief_info,
                        subject_type = s.subject_type,
                        subject_status = s.subject_status,
                        subject_tag_line = s.subject_tag_line
                    }).ToList();
                    foreach (var sj in arrSubjectList)
                    {
                        var lessons = (from l in db.Lessons
                                       where l.subject_id == sj.subject_id
                                       select new { l.lesson_id }).ToList();
                        sj.lesson_count = lessons.Count;
                    }


                    int totalrows = arrSubjectList.Count;
                    int totalrowsafterfiltering = arrSubjectList.Count;
                    arrSubjectList = arrSubjectList.Skip(start).Take(length).ToList();
                    arrSubjectList = arrSubjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = arrSubjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        //filter by status subject
        [HttpPost]
        public ActionResult FilterBySubjectStatus(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_STATUS)) // filter theo type
                {
                    var typeSubjectList = (from s in db.Subjects
                                           where s.subject_status.Equals(type)
                                           select new
                                           {
                                               s.subject_id,
                                               s.subject_category,
                                               s.subject_name,
                                               s.subject_brief_info,
                                               s.subject_type,
                                               s.subject_status,
                                               s.subject_tag_line,
                                           }).ToList();
                    List<SubjectListModel> arrSubjectList = typeSubjectList.Select(s => new SubjectListModel
                    {
                        subject_id = s.subject_id,
                        subject_category = s.subject_category,
                        subject_name = s.subject_name,
                        subject_brief_info = s.subject_brief_info,
                        subject_type = s.subject_type,
                        subject_status = s.subject_status,
                        subject_tag_line = s.subject_tag_line
                    }).ToList();
                    foreach (var sj in arrSubjectList)
                    {
                        var lessons = (from l in db.Lessons
                                       where l.subject_id == sj.subject_id
                                       select new { l.lesson_id }).ToList();
                        sj.lesson_count = lessons.Count;
                    }



                    int totalrows = arrSubjectList.Count;
                    int totalrowsafterfiltering = arrSubjectList.Count;
                    arrSubjectList = arrSubjectList.Skip(start).Take(length).ToList();
                    arrSubjectList = arrSubjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = arrSubjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
                    var typeSubjectList = (from s in db.Subjects
                                           select new
                                           {
                                               s.subject_id,
                                               s.subject_category,
                                               s.subject_name,
                                               s.subject_brief_info,
                                               s.subject_type,
                                               s.subject_status,
                                               s.subject_tag_line,
                                           }).ToList();
                    List<SubjectListModel> arrSubjectList = typeSubjectList.Select(s => new SubjectListModel
                    {
                        subject_id = s.subject_id,
                        subject_category = s.subject_category,
                        subject_name = s.subject_name,
                        subject_brief_info = s.subject_brief_info,
                        subject_type = s.subject_type,
                        subject_status = s.subject_status,
                        subject_tag_line = s.subject_tag_line
                    }).ToList();
                    foreach (var sj in arrSubjectList)
                    {
                        var lessons = (from l in db.Lessons
                                       where l.subject_id == sj.subject_id
                                       select new { l.lesson_id }).ToList();
                        sj.lesson_count = lessons.Count;
                    }


                    int totalrows = arrSubjectList.Count;
                    int totalrowsafterfiltering = arrSubjectList.Count;
                    arrSubjectList = arrSubjectList.Skip(start).Take(length).ToList();
                    arrSubjectList = arrSubjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = arrSubjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult GetAllSubject()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {

                string sql = "select * from Subject";

                List<SubjectListModel> Subjects = db.Database.SqlQuery<SubjectListModel>(sql).ToList();

                foreach (var sj in Subjects)
                {
                    var lessons = (from l in db.Lessons
                                   where l.subject_id == sj.subject_id
                                   select new { l.lesson_id }).ToList();
                    sj.lesson_count = lessons.Count;
                }

                int totalrows = Subjects.Count;
                int totalrowsafterfiltering = Subjects.Count;
                Subjects = Subjects.Skip(start).Take(length).ToList();
                Subjects = Subjects.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Subjects, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddSubject()
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Setting> listSettingType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.SUBJECT_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingType = listSettingType;

                List<Setting> listSettingCategory = db.Settings.Where(s => (s.setting_group_value.Equals(SettingGroup.SUBJECT_CATEGORY) || s.setting_group_value.Equals(SettingGroup.GUIDE_CATEGORY)) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingCategory = listSettingCategory;

                return View("/Views/CMS/Subject/SubjectAdding.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitAddSubject(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addsubject = JValue.Parse(postJson);

                    Subject s = new Subject();
                    s.subject_name = addsubject.subjectName;
                    s.subject_category = addsubject.subjectCategory;
                    s.subject_type = addsubject.subjectType;
                    s.subject_brief_info = addsubject.shortDes;
                    s.subject_status = addsubject.subjectStatus;
                    db.Subjects.Add(s);
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
        public ActionResult EditSubject(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;

                List<Setting> listSettingType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.SUBJECT_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingType = listSettingType;

                List<Setting> listSettingCategory = db.Settings.Where(s => (s.setting_group_value.Equals(SettingGroup.SUBJECT_CATEGORY) || s.setting_group_value.Equals(SettingGroup.GUIDE_CATEGORY)) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingCategory = listSettingCategory;

                ViewBag.id = id;
                return View("/Views/CMS/Subject/SubjectEditting.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitEditSubject(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic editsubject = JValue.Parse(postJson);
                    int id = editsubject.subjectId;

                    Subject s = db.Subjects.Where(subject => subject.subject_id == id).FirstOrDefault();
                    if (s != null)
                    {
                        s.subject_name = editsubject.subjectName;
                        s.subject_category = editsubject.subjectCategory;
                        s.subject_type = editsubject.subjectType;
                        s.subject_brief_info = editsubject.shortDes;
                        s.subject_status = editsubject.subjectStatus;
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
        public ActionResult SubjectLessonList(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.id = id;
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;
                return View("/Views/CMS/Subject/SubjectLessonList.cshtml");
            }
        }


    }
}
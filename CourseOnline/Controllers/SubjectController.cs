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
        // GET: Subject
        [Route("SubjectList")]
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/LTContent/SubjectsList");
                if (result.Equals("Student"))
                {
                    return View("/Views/Error_404.cshtml");
                }
                if (result.Equals("Reject"))
                {
                    return View("~/Views/CMS/Home.cshtml");
                }
                else
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
                var subjectList = (from s in db.Subjects
                                   where s.subject_name.Contains(type)
                                   select new SubjectListModel
                                   {
                                       subject_id = s.subject_id,
                                       subject_category = s.subject_category,
                                       subject_name = s.subject_name,
                                       subject_brief_info = s.subject_brief_info,
                                       subject_type = s.subject_type,
                                       subject_status = s.subject_status,
                                       subject_tag_line = s.subject_tag_line
                                   }).ToList();
                int totalrows = subjectList.Count;
                int totalrowsafterfiltering = subjectList.Count;
                subjectList = subjectList.Skip(start).Take(length).ToList();
                subjectList = subjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = subjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
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

            string type = filterByJson.subjectType;
            string category = filterByJson.subjectCategory;
            string status = filterByJson.subjectStatus;

            if (type.Equals(All.ALL_TYPE))
            {
                type = "";
            }
            if (category.Equals(All.ALL_CATEGORY))
            {
                category = "";
            }
            if (status.Equals(All.ALL_STATUS))
            {
                status = "";
            }

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var subjectList = (from s in db.Subjects
                                   where s.subject_type.Contains(type)
                                   && s.subject_category.Contains(category)
                                   && s.subject_status.Contains(status)
                                   select new SubjectListModel
                                   {
                                       subject_id = s.subject_id,
                                       subject_category = s.subject_category,
                                       subject_name = s.subject_name,
                                       subject_brief_info = s.subject_brief_info,
                                       subject_type = s.subject_type,
                                       subject_status = s.subject_status,
                                       subject_tag_line = s.subject_tag_line
                                   }).ToList();

                foreach (var sj in subjectList)
                {
                    var lessons = (from l in db.Lessons
                                   where l.subject_id == sj.subject_id
                                   select new { l.lesson_id }).ToList();
                    sj.lesson_count = lessons.Count;
                }

                int totalrows = subjectList.Count;
                int totalrowsafterfiltering = subjectList.Count;
                subjectList = subjectList.Skip(start).Take(length).ToList();
                subjectList = subjectList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = subjectList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAllSubject()
        {
            if (Session["Email"] == null)
            {
                return null;
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/LTContent/SubjectsList");
                if (result.Equals("Student"))
                {
                    return null;
                }
                if (result.Equals("Reject"))
                {
                    return null;
                }
                else
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
                    int temp = db.Subjects.DefaultIfEmpty().Max(sub => sub == null ? 0 : sub.subject_id);
                    int id_new = temp + 1;
                    string imageValue = addsubject.subjectImage;
                    var ava = imageValue.Substring(imageValue.IndexOf(",") + 1);
                    var hinhanh = Convert.FromBase64String(ava);
                    string relative_path = "/Assets/dist/img/" + "subject" + id_new + ".png";
                    string path = Server.MapPath(relative_path);
                    System.IO.File.WriteAllBytes(path, hinhanh);
                    Subject s = new Subject();
                    s.subject_name = addsubject.subjectName;
                    s.subject_category = addsubject.subjectCategory;
                    s.subject_type = addsubject.subjectType;
                    s.subject_brief_info = addsubject.shortDes;
                    s.picture = relative_path;
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
                    string imageValue = editsubject.subjectImage;
                    var ava = imageValue.Substring(imageValue.IndexOf(",") + 1);
                    if (ava == "/Assets/dist/img/" + "subject" + editsubject.id + ".png")
                    {
                        if (s != null)
                        {
                            s.subject_name = editsubject.subjectName;
                            s.subject_category = editsubject.subjectCategory;
                            s.subject_type = editsubject.subjectType;
                            s.subject_brief_info = editsubject.shortDes;
                            s.picture = editsubject.subjectImage;
                            s.subject_status = editsubject.subjectStatus;
                            db.SaveChanges();
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var hinhanh = Convert.FromBase64String(ava);
                        string relative_path = "/Assets/dist/img/" + "subject" + editsubject.id + ".png";
                        string path = Server.MapPath(relative_path);
                        System.IO.File.WriteAllBytes(path, hinhanh);
                        if (s != null)
                        {
                            s.subject_name = editsubject.subjectName;
                            s.subject_category = editsubject.subjectCategory;
                            s.subject_type = editsubject.subjectType;
                            s.subject_brief_info = editsubject.shortDes;
                            s.picture = relative_path;
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

        [HttpGet]
        public ActionResult DomainList(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.id = id;
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;
                return View("/Views/CMS/Subject/DomainList.cshtml");
            }
        }

 
    }
}

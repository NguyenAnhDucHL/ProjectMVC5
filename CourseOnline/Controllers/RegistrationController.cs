using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;
using Newtonsoft.Json.Linq;

namespace CourseOnline.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/CourseManagement/Registrations");
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
                        var lstSubject = db.Subjects.Select(s => s.subject_name).Distinct().ToList();
                        ViewBag.lstSubject = lstSubject;

                        var lstCourse = db.Courses.Select(c => c.course_name).Distinct().ToList();
                        ViewBag.lstCourse = lstCourse;

                        var lststatus = db.Registrations.Select(r => r.registration_status).Distinct().ToList();
                        ViewBag.lststatus = lststatus;
                    }
                    return View("/Views/CMS/Registration/RegistrationList.cshtml");
                }
            }
        }
        [HttpPost]
        public ActionResult GetAllRegistration()
        {
            if (Session["Email"] == null)
            {
                return null;
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/CourseManagement/Registrations");
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
                        string sql = "select r.registration_id, s.subject_name, c.course_name, u.user_fullname, u.user_email, r.registration_time, r.registration_status " +
                                    "from Registration r join [User] u " +
                                    "on r.[user_id] = u.[user_id] " +
                                    "join Course c " +
                                    "on c.course_id = r.course_id " +
                                    "join Subject s " +
                                    "on s.subject_id = c.subject_id";

                        List<RegistrationListModel> Registrations = db.Database.SqlQuery<RegistrationListModel>(sql).ToList();

                        int totalrows = Registrations.Count;
                        int totalrowsafterfiltering = Registrations.Count;
                        Registrations = Registrations.Skip(start).Take(length).ToList();
                        Registrations = Registrations.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Registrations, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
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

            string subject = filterByJson.subject;
            string course = filterByJson.course;
            string status = filterByJson.regisStatus;

            if (subject.Equals(All.ALL_SUBJECT))
            {
                subject = "";
            }
            if (course.Equals(All.All_Course))
            {
                course = "";
            }
            if (status.Equals(All.ALL_STATUS))
            {
                status = "";
            }

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var registrationList = (from r in db.Registrations
                                        join u in db.Users on r.user_id equals u.user_id
                                        join c in db.Courses on r.course_id equals c.course_id
                                        join s in db.Subjects on c.subject_id equals s.subject_id
                                        where s.subject_name.Contains(subject)
                                        && c.course_name.Contains(course)
                                        && r.registration_status.Contains(status)
                                        select new RegistrationListModel
                                        {
                                            registration_id = r.registration_id,
                                            subject_name = s.subject_name,
                                            course_name = c.course_name,
                                            user_fullname = u.user_fullname,
                                            user_email = u.user_email,
                                            registration_time = r.registration_time,
                                            registration_status = r.registration_status
                                        }).ToList();


                int totalrows = registrationList.Count;
                int totalrowsafterfiltering = registrationList.Count;
                registrationList = registrationList.Skip(start).Take(length).ToList();
                registrationList = registrationList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = registrationList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SetRegistrationStatus(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic changeStatus = JValue.Parse(postJson);
                    int id = changeStatus.id;

                    Registration r = db.Registrations.Where(re => re.registration_id == id).FirstOrDefault();
                    if (r != null)
                    {
                        r.registration_status = changeStatus.resStatus;
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
    }
}
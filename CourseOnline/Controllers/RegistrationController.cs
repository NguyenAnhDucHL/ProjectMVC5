﻿using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;

namespace CourseOnline.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        [HttpGet]
        public ActionResult Index()
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
        [HttpPost]
        public ActionResult GetAllRegistration()
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
                            "on r.user_id = u.user_id " +
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
        [HttpPost]
        public ActionResult FilterBySubjectName(string subjectName)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!subjectName.Equals(All.ALL_SUBJECT))
                {
                    var lstRegistration = (from r in db.Registrations
                                           join u in db.Users on r.user_id equals u.user_id
                                           join c in db.Courses on r.course_id equals c.course_id
                                           join s in db.Subjects.Where(s => s.subject_name == subjectName) on c.subject_id equals s.subject_id
                                           select new RegistrationListModel()
                                           {
                                               subject_name = s.subject_name,
                                               course_name = c.course_name,
                                               user_fullname = u.user_fullname,
                                               user_email = u.user_email,
                                               registration_time = r.registration_time,
                                               registration_status = r.registration_status,
                                           }).ToList();
                    int totalrows = lstRegistration.Count;
                    int totalrowsafterfiltering = lstRegistration.Count;
                    lstRegistration = lstRegistration.Skip(start).Take(length).ToList();
                    lstRegistration = lstRegistration.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = lstRegistration, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lstRegistration = (from r in db.Registrations
                                           join u in db.Users on r.user_id equals u.user_id
                                           join c in db.Courses on r.course_id equals c.course_id
                                           join s in db.Subjects on c.subject_id equals s.subject_id
                                           select new RegistrationListModel()
                                           {
                                               subject_name = s.subject_name,
                                               course_name = c.course_name,
                                               user_fullname = u.user_fullname,
                                               user_email = u.user_email,
                                               registration_time = r.registration_time,
                                               registration_status = r.registration_status,
                                           }).ToList();
                    


                    int totalrows = lstRegistration.Count;
                    int totalrowsafterfiltering = lstRegistration.Count;
                    lstRegistration = lstRegistration.Skip(start).Take(length).ToList();
                    lstRegistration = lstRegistration.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = lstRegistration, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult FilterByCourses(string courseName)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!courseName.Equals(All.All_Course))
                {
                    var lstRegistration = (from r in db.Registrations
                                           join u in db.Users on r.user_id equals u.user_id
                                           join c in db.Courses.Where(c => c.course_name == courseName) on r.course_id equals c.course_id
                                           join s in db.Subjects on c.subject_id equals s.subject_id
                                           select new RegistrationListModel()
                                           {
                                               subject_name = s.subject_name,
                                               course_name = c.course_name,
                                               user_fullname = u.user_fullname,
                                               user_email = u.user_email,
                                               registration_time = r.registration_time,
                                               registration_status = r.registration_status,
                                           }).ToList();
                    int totalrows = lstRegistration.Count;
                    int totalrowsafterfiltering = lstRegistration.Count;
                    lstRegistration = lstRegistration.Skip(start).Take(length).ToList();
                    lstRegistration = lstRegistration.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = lstRegistration, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lstRegistration = (from r in db.Registrations
                                           join u in db.Users on r.user_id equals u.user_id
                                           join c in db.Courses on r.course_id equals c.course_id
                                           join s in db.Subjects on c.subject_id equals s.subject_id
                                           select new RegistrationListModel()
                                           {
                                               subject_name = s.subject_name,
                                               course_name = c.course_name,
                                               user_fullname = u.user_fullname,
                                               user_email = u.user_email,
                                               registration_time = r.registration_time,
                                               registration_status = r.registration_status,
                                           }).ToList();



                    int totalrows = lstRegistration.Count;
                    int totalrowsafterfiltering = lstRegistration.Count;
                    lstRegistration = lstRegistration.Skip(start).Take(length).ToList();
                    lstRegistration = lstRegistration.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = lstRegistration, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult FilterBySubjectStatus(string statusName)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!statusName.Equals(All.ALL_STATUS))
                {
                    var lstRegistration = (from r in db.Registrations.Where(r => r.registration_status == statusName)
                                           join u in db.Users on r.user_id equals u.user_id
                                           join c in db.Courses on r.course_id equals c.course_id
                                           join s in db.Subjects on c.subject_id equals s.subject_id
                                           select new RegistrationListModel()
                                           {
                                               subject_name = s.subject_name,
                                               course_name = c.course_name,
                                               user_fullname = u.user_fullname,
                                               user_email = u.user_email,
                                               registration_time = r.registration_time,
                                               registration_status = r.registration_status,
                                           }).ToList();
                    int totalrows = lstRegistration.Count;
                    int totalrowsafterfiltering = lstRegistration.Count;
                    lstRegistration = lstRegistration.Skip(start).Take(length).ToList();
                    lstRegistration = lstRegistration.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = lstRegistration, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lstRegistration = (from r in db.Registrations
                                           join u in db.Users on r.user_id equals u.user_id
                                           join c in db.Courses on r.course_id equals c.course_id
                                           join s in db.Subjects on c.subject_id equals s.subject_id
                                           select new RegistrationListModel()
                                           {
                                               subject_name = s.subject_name,
                                               course_name = c.course_name,
                                               user_fullname = u.user_fullname,
                                               user_email = u.user_email,
                                               registration_time = r.registration_time,
                                               registration_status = r.registration_status,
                                           }).ToList();



                    int totalrows = lstRegistration.Count;
                    int totalrowsafterfiltering = lstRegistration.Count;
                    lstRegistration = lstRegistration.Skip(start).Take(length).ToList();
                    lstRegistration = lstRegistration.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = lstRegistration, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
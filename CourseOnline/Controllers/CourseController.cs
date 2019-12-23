using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using CourseOnline.Global.Setting;
using System.Collections;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        [Route("CourseList")]
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/CourseManagement/CoursesList");
                if (result.Equals("Student"))
                {
                    return View("/Views/Error_404.cshtml");
                }
                if (result.Equals("Reject"))
                {
                    return RedirectToAction("Home_CMS", "Home");
                }
                else
                {
                    using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                    {
                        List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).ToList();
                        ViewBag.Subjects = listSubject;
                    }
                    return View("/Views/CMS/Course/CourseList.cshtml");
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
                var courseList = (from c in db.Courses
                                  join s in db.Subjects on c.subject_id equals s.subject_id
                                  join u in db.Users on c.teacher_id equals u.user_id
                                  join ur in db.UserRoles on u.user_id equals ur.user_id
                                  join r in db.Roles on ur.role_id equals r.role_id
                                  where c.course_name.Contains(type) && r.role_id == 2
                                   select new CourseListModel
                                   {
                                       course_id = c.course_id,
                                       course_name = c.course_name,
                                       user_fullname = u.user_fullname,
                                       course_start_date = c.course_start_date,
                                       course_end_date = c.course_end_date,
                                       course_status = c.course_status
                                   }).ToList();
                int totalrows = courseList.Count;
                int totalrowsafterfiltering = courseList.Count;
                courseList = courseList.Skip(start).Take(length).ToList();
                courseList = courseList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = courseList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        //filter by subject
        [HttpPost]
        public ActionResult FilterBySubjectName(int type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (type != All.ALL_ID_SUBJECT) // filter theo subject
                {
                    var courseList = (from c in db.Courses
                                      join s in db.Subjects on c.subject_id equals s.subject_id
                                      join u in db.Users on c.teacher_id equals u.user_id
                                      join ur in db.UserRoles on u.user_id equals ur.user_id
                                      join r in db.Roles on ur.role_id equals r.role_id
                                      where s.subject_id.Equals(type) && r.role_id == 2
                                      select new CourseListModel
                                      {
                                          course_id = c.course_id,
                                          course_name = c.course_name,
                                          user_fullname = u.user_fullname,
                                          course_start_date = c.course_start_date,
                                          course_end_date = c.course_end_date,
                                          course_status = c.course_status
                                      }).ToList();


                    int totalrows = courseList.Count;
                    int totalrowsafterfiltering = courseList.Count;
                    courseList = courseList.Skip(start).Take(length).ToList();
                    courseList = courseList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = courseList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
                    var courseList = (from c in db.Courses
                                      join s in db.Subjects on c.subject_id equals s.subject_id
                                      join u in db.Users on c.teacher_id equals u.user_id
                                      join ur in db.UserRoles on u.user_id equals ur.user_id
                                      join r in db.Roles on ur.role_id equals r.role_id
                                      where r.role_id == 2
                                      select new CourseListModel
                                      {
                                          course_id = c.course_id,
                                          course_name = c.course_name,
                                          user_fullname = u.user_fullname,
                                          course_start_date = c.course_start_date,
                                          course_end_date = c.course_end_date,
                                          course_status = c.course_status
                                      }).ToList();


                    int totalrows = courseList.Count;
                    int totalrowsafterfiltering = courseList.Count;
                    courseList = courseList.Skip(start).Take(length).ToList();
                    courseList = courseList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = courseList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }
        [HttpPost]
        public ActionResult GetAllCourse()
        {
            if (Session["Email"] == null)
            {
                return null;
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/CourseManagement/CoursesList");
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
                        string sql = "select c.course_id, c.course_name, u.user_fullname, c.course_start_date, c.course_end_date, c.course_status " +
                                    "from Course c join [User] u " +
                                    "on c.teacher_id = u.user_id";

                        List<CourseListModel> Courses = db.Database.SqlQuery<CourseListModel>(sql).ToList();

                        int totalrows = Courses.Count;
                        int totalrowsafterfiltering = Courses.Count;
                        Courses = Courses.Skip(start).Take(length).ToList();
                        Courses = Courses.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Courses, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        [HttpGet]
        public ActionResult AddCourse()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {

                List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).Distinct().ToList();
                ViewBag.Subjects = listSubject;
                String sql = "select * from [User] as u " +
                                "join UserRole ur on ur.user_id = u.user_id " +
                                "join Roles r on r.role_id = ur.role_id " +
                                "where r.role_id = 2";
                List<User> listUser = db.Database.SqlQuery<User>(sql).ToList();
                ViewBag.Users = listUser;
                return View("/Views/CMS/Course/CourseAdd.cshtml");

            }
        }
        [HttpPost]
        public ActionResult SubmitAddCourse(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addCourse = JValue.Parse(postJson);
                    Course c = new Course();
                    c.course_name = addCourse.courseName;
                    String temp = addCourse.courseStatus;
                    if (temp.Equals("Active"))
                    {
                        c.course_status = true;
                    }
                    else
                    {
                        c.course_status = false;
                    }
                    c.subject_id = addCourse.courseSubject;
                    c.teacher_id = addCourse.courseTeacher;
                    c.course_start_date = addCourse.courseStartDate;
                    c.course_end_date = addCourse.courseEndDate;
                    c.course_note = addCourse.courseNote;
                    db.Courses.Add(c);
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
        public ActionResult SubmitEditCourse(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editcourse = JValue.Parse(postJson);
                    int id = editcourse.courseid;

                    Course c = db.Courses.Where(course => course.course_id == id).FirstOrDefault();
                    if (c != null)
                    {
                        c.course_name = editcourse.courseName;
                        temp = editcourse.courseStatus;
                        if (temp.Equals("Active"))
                        {
                            c.course_status = true;
                        }
                        else
                        {
                            c.course_status = false;
                        }
                        c.subject_id = editcourse.courseSubject;
                        c.teacher_id = editcourse.courseTeacher;
                        c.course_start_date = editcourse.courseStartDate;
                        c.course_end_date = editcourse.courseEndDate;
                        c.course_note = editcourse.courseNote;
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
        public ActionResult CourseEdit(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).Distinct().ToList();
                ViewBag.Subjects = listSubject;

                string sql = "select c.course_id, c.course_name, u.user_fullname, s.subject_name, c.course_start_date, c.course_end_date, c.course_status, c.subject_id, c.teacher_id " +
                            "from Course c join [User] u " +
                            "on c.teacher_id = u.[user_id] " +
                            "join [Subject] s " +
                            "on c.subject_id = s.subject_id " +
                            "where c.course_id = @id";
                CourseListModel Courses = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("id", id)).FirstOrDefault();
                ViewBag.Course = Courses;
                ViewBag.SubjectName = Courses.subject_name;
                ViewBag.UserName = Courses.user_fullname;

                return View("/Views/CMS/Course/CourseEdit.cshtml");
            }
        }
        [HttpGet]
        public ActionResult CourseWorkList(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.id = id;
                Course course = db.Courses.Where(c => c.course_id == id).FirstOrDefault();
                ViewBag.Course = course;
                return View("/Views/CMS/Course/CourseWorkList.cshtml");
            }
        }
        [HttpGet]
        public ActionResult getGrade(int id = -1)
        {
            ViewBag.courseid = id;
            return View("/Views/CMS/Course/Grade.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllGrade(int id = -1)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Coursework> courseworks = db.Courseworks.Where(cw => cw.course_id == id && cw.coursework_status==true).ToList();
                List<String> testsorted = new List<string>();
                foreach (Coursework coursework in courseworks)
                {

                    var date = coursework.due_date;
                    var datearray = date.Split('/');
                    date = datearray[2] + '/' + datearray[0] + '/' + datearray[1] + '/' + coursework.coursework_id.ToString();
                    testsorted.Add(date);
                }
                testsorted.Sort();
                List<GradeModels> gradeModels = new List<GradeModels>();

                String sql = "select u.[user_id], user_fullname, u.user_email, g.grade_user, cw.coursework_id, cw.coursework_weight " +
                                "from Grade g " +
                                "join Coursework cw on g.coursework_id = cw.coursework_id " +
                                "join[User] u on g.[user_id] = u.[user_id] " +
                                "where cw.course_id = @id ";

                List<GradeModels> gradeTempModels = db.Database.SqlQuery<GradeModels>(sql, new SqlParameter("id", id)).ToList();
                String[] courseworkid;
                foreach (GradeModels gms in gradeTempModels)
                {
                    if (!gradeModels.Exists(gm => gm.registration_id == gms.registration_id))
                    {
                        for (int i = 0; i < testsorted.Count;)
                        {
                            courseworkid = testsorted[i].Split('/');
                            if (courseworkid[3].Equals(gms.coursework_id.ToString()))
                            {
                                if (i == 0)
                                {
                                    gradeModels.Add(new GradeModels()
                                    {
                                        user_fullname = gms.user_fullname,
                                        user_email = gms.user_email,
                                        Test1 = gms.grade_user,
                                        Total = ((float)gms.grade_user/10)*gms.coursework_weight
                                    });
                                }
                                if (i == 1)
                                {
                                    gradeModels.Add(new GradeModels()
                                    {
                                        user_fullname = gms.user_fullname,
                                        user_email = gms.user_email,
                                        Test2 = gms.grade_user,
                                        Total = ((float)gms.grade_user / 10) * gms.coursework_weight
                                    });
                                }
                                if (i == 2)
                                {
                                    gradeModels.Add(new GradeModels()
                                    {
                                        user_fullname = gms.user_fullname,
                                        user_email = gms.user_email,
                                        Test3 = gms.grade_user,
                                        Total = ((float)gms.grade_user / 10) * gms.coursework_weight
                                    });
                                }
                                if (i == 3)
                                {
                                    gradeModels.Add(new GradeModels()
                                    {
                                        user_fullname = gms.user_fullname,
                                        user_email = gms.user_email,
                                        Test4 = gms.grade_user,
                                        Total = ((float)gms.grade_user / 10) * gms.coursework_weight
                                    });
                                }
                                if (i == 4)
                                {
                                    gradeModels.Add(new GradeModels()
                                    {
                                        user_fullname = gms.user_fullname,
                                        user_email = gms.user_email,
                                        Test5 = gms.grade_user,
                                        Total = ((float)gms.grade_user / 10) * gms.coursework_weight
                                    });
                                }
                                if (i == 5)
                                {
                                    gradeModels.Add(new GradeModels()
                                    {
                                        user_fullname = gms.user_fullname,
                                        user_email = gms.user_email,
                                        Test6 = gms.grade_user,
                                        Total = ((float)gms.grade_user / 10) * gms.coursework_weight
                                    });
                                }
                                break;
                            }
                            i++;
                        }
                    }
                    else
                    {
                        int j = gradeModels.FindIndex(gm => gm.registration_id == gms.registration_id);
                        gradeModels.ElementAt(j).Total = gradeModels.ElementAt(j).Total + ((float)gms.grade_user / 10) * gms.coursework_weight;
                        //gradeModels.ElementAt(j).Total = gradeModels.ElementAt(j).Total + gms.grade_user / 10;
                        for (int i = 0; i < testsorted.Count;)
                        {
                            courseworkid = testsorted[i].Split('/');
                            if (courseworkid[3].Equals(gms.coursework_id.ToString()))
                            {
                                if (i == 0)
                                {
                                    gradeModels.ElementAt(j).Test1 = gms.grade_user;
                                }
                                if (i == 1)
                                {
                                    gradeModels.ElementAt(j).Test2 = gms.grade_user;
                                }
                                if (i == 2)
                                {
                                    gradeModels.ElementAt(j).Test3 = gms.grade_user;
                                }
                                if (i == 3)
                                {
                                    gradeModels.ElementAt(j).Test4 = gms.grade_user;
                                }
                                if (i == 4)
                                {
                                    gradeModels.ElementAt(j).Test5 = gms.grade_user;
                                }
                                if (i == 5)
                                {
                                    gradeModels.ElementAt(j).Test6 = gms.grade_user;
                                }
                                break;
                            }
                            i++;
                        }
                    }
                }
                int totalrows = gradeModels.Count;
                int totalrowsafterfiltering = gradeModels.Count;
                gradeModels = gradeModels.Skip(start).Take(length).ToList();
                gradeModels = gradeModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = gradeModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

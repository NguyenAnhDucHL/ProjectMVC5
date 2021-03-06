﻿using CourseOnline.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using CourseOnline.Global.Setting;
using System.Net;
using System.Globalization;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        public ActionResult Home_CMS()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "No Permission");
                if (result.Equals("Student"))
                {
                    return View("/Views/Error_404.cshtml");
                }
                else
                {
                    int countAdmin = db.UserRoles.Where(ur => ur.role_id == 1).Count();
                    int countTeacher = db.UserRoles.Where(ur => ur.role_id == 2).Count();
                    int countStudent = db.UserRoles.Where(ur => ur.role_id == 3).Count();
                    ViewBag.countAdmin = countAdmin;
                    ViewBag.countTeacher = countTeacher;
                    ViewBag.countStudent = countStudent;
                    return View("/Views/CMS/Home.cshtml");
                }
            }
        }
        public ActionResult Home_User()
        {

            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, s.picture, s.subject_brief_info from Course c  " +
                "join Subject s on c.subject_id = s.subject_id where convert(datetime,c.course_start_date) >= @datetimenow and c.course_status = 'True' and s.subject_status = 'Online'";
            List<CourseListModel> lstCourse = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).Take(3).ToList();
            ViewBag.lstCourse = lstCourse;

            List<Post> lstPost = db.Posts.Take(5).OrderByDescending(n => n.post_id).Where(n => n.post_status == "Published" && n.post_type == "Guide").ToList();
            ViewBag.lstPost = lstPost;

            var lstSlider = db.Sliders.ToList();
            ViewBag.lstSlider = lstSlider;

            var lstTeacher = (from u in db.Users
                              join ur in db.UserRoles.Where(ur => ur.role_id == 2) on u.user_id equals ur.user_id
                              select new UserListModel
                              {
                                  user_fullname = u.user_fullname,
                                  user_position = u.user_position,
                                  user_image = u.user_image,
                                  user_description = u.user_description,
                              }).Take(6).ToList();

            ViewBag.lstTeacher = lstTeacher;
            return View("/Views/User/Home.cshtml");
        }
        [HttpPost]
        public JsonResult GoogleLogin(string email, string name, string gender, string lastname, string location, string picture)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int duplicate = (from Users in db.Users where Users.user_email == email select Users).Count();
                        if (duplicate == 0)
                        {
                            String sql = "insert into [User](user_position,user_fullname,user_email,use_mobile,user_status,user_image) values (@user_position,@user_fullname,@user_email,@use_mobile,@user_status,@user_image)";
                            db.Database.ExecuteSqlCommand(sql,
                                new SqlParameter("user_position", ""),
                                new SqlParameter("user_fullname", name),
                                new SqlParameter("user_email", email),
                                new SqlParameter("use_mobile", ""),
                                new SqlParameter("user_status", true),
                                 new SqlParameter("user_image", picture)
                                );
                            db.SaveChanges();
                            int id_new = db.Users.DefaultIfEmpty().Max(u => u == null ? 0 : u.user_id);

                            String sql2 = "insert into [UserRole](user_id,role_id) values (@user_id,@role_id)";
                            db.Database.ExecuteSqlCommand(sql2,
                                new SqlParameter("user_id", id_new),
                                new SqlParameter("role_id", 3)
                                );
                            Session["Picture"] = picture;
                            Session["Name"] = name;
                            Session["Email"] = email;
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        else
                        {
                            bool userstatus = db.Users.Where(u => u.user_email == email).Select(u => u.user_status).FirstOrDefault();
                            if (userstatus == true)
                            {
                                string userPicture = db.Users.Where(u => u.user_email == email).Select(u => u.user_image).FirstOrDefault();
                                Session["Picture"] = userPicture;
                                Session["Name"] = name;
                                Session["Email"] = email;
                            }
                            else
                            {
                                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    catch (Exception e)
                    {

                        transaction.Rollback();
                    }
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckAccount()
        {
            GetPermission(Session["Email"].ToString());
            if (Session["rolepermission"].Equals("Admin") || Session["rolepermission"].Equals("Teacher"))
            {
                return RedirectToAction("Home_CMS", "Home");
            }
            else
            {
                return RedirectToAction("YourCourse", "Home");
            }
        }

        public void GetPermission(string email)
        {
            var checkPermission = (from u in db.Users.Where(x => x.user_email == email)
                                   join ur in db.UserRoles on u.user_id equals ur.user_id
                                   join r in db.Roles on ur.role_id equals r.role_id
                                   select r.role_name);

            Session["rolepermission"] = "";
            foreach (string permissionName in checkPermission)
            {
                if (permissionName.Equals("Admin"))
                {
                    Session["rolepermission"] = "Admin";
                }
                else if (permissionName.Equals("Teacher"))
                {
                    Session["rolepermission"] = "Teacher";
                }
                else
                {
                    Session["rolepermission"] = "Student";
                }
            }
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Home_User", "Home");
        }
        [HttpGet]
        public ActionResult YourAcountInformation()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Home_User", "Home");
            }
            string email = Session["Email"].ToString();
            User userInformation = db.Users.Where(u => u.user_email == email).FirstOrDefault();
            ViewBag.userInformation = userInformation;
            return View("/Views/User/AccountInformation.cshtml");
        }
        [HttpPost]
        public ActionResult UpdateUser(string userJson)
        {

            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic editUser = JValue.Parse(userJson);
                    int id = editUser.userID;
                    User user = db.Users.Where(u => u.user_id == id).FirstOrDefault();
                    string imageValue = editUser.userImage;
                    var ava = imageValue.Substring(imageValue.IndexOf(",") + 1);

                    if (ava == "/Path/" + "user" + editUser.id + ".png")
                    {
                        if (user != null)
                        {
                            user.user_fullname = editUser.userName;
                            user.use_mobile = editUser.userMobile;
                            user.user_image = editUser.userImage;
                            user.user_description = editUser.userDescription;
                            user.user_gender = editUser.userGender;
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
                        Byte[] hinhanh = null;
                        try
                        {
                            hinhanh = Convert.FromBase64String(ava);
                        }
                        catch (Exception)
                        {
                            if (user != null)
                            {
                                user.user_fullname = editUser.userName;
                                user.use_mobile = editUser.userMobile;
                                user.user_description = editUser.userDescription;
                                user.user_gender = editUser.userGender;
                                db.SaveChanges();
                                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        string relative_path = "/Path/" + "user" + editUser.id + ".png";
                        string path = Server.MapPath(relative_path);
                        System.IO.File.WriteAllBytes(path, hinhanh);
                        if (Session["Email"].Equals(user.user_email))
                        {
                            Session["Picture"] = relative_path;
                        }
                        if (user != null)
                        {
                            user.user_fullname = editUser.userName;
                            user.use_mobile = editUser.userMobile;
                            user.user_image = relative_path;
                            user.user_description = editUser.userDescription;
                            user.user_gender = editUser.userGender;
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
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult CourseFound(string keyword, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            if (keyword == null)
            {
                keyword = "";
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, " +
                "s.picture, s.subject_brief_info from Course c  join Subject s on c.subject_id = s.subject_id " +
                "where convert(datetime,c.course_start_date) >= @datetimenow and c.course_status = 'True' and s.subject_status = 'Online'";
            List<CourseListModel> lstCourse = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).Where(c => c.course_name.Contains(keyword)).ToList();

            ViewBag.KeyWord = keyword;
            ViewBag.FoundCourse = lstCourse.OrderBy(n => n.course_name).ToPagedList(pageNumber, pageSize);
            return View("/Views/User/CourseFound.cshtml");
        }

        [HttpPost]
        public ActionResult GetKeyWord(string keyword)
        {
            return RedirectToAction("CourseFound", new { @keyword = keyword });
        }

        public ActionResult CourseFoundPartialView(string keyword)
        {
            if (keyword == null)
            {
                keyword = "";
            }
            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, " +
               "s.picture,  s.subject_brief_info from Course c  join Subject s on c.subject_id = s.subject_id " +
               "where convert(datetime,c.course_start_date) >= @datetimenow and c.course_status = 'True' and s.subject_status = 'Online'";
            List<CourseListModel> lstCourse = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).Where(c => c.course_name.Contains(keyword)).ToList();
            ViewBag.keyword = keyword;
            ViewBag.lstCourse = lstCourse;
            return PartialView("/Views/User/CourseFoundPartialView.cshtml");
        }

        [HttpGet]
        public ActionResult SelectCourseToQuiz()
        {
            if (Session["Email"] == null)
            {

                return RedirectToAction("Home_User");
            }
            string myemail = Session["Email"].ToString();
            List<CourseListModel> lstMyCourse = (from s in db.Subjects.Where(s => s.subject_status == "Online")
                                                 join c in db.Courses.Where(c => c.course_status == true) on s.subject_id equals c.subject_id
                                                 join re in db.Registrations.Where(re => re.registration_status == "Approved") on c.course_id equals re.course_id
                                                 join u in db.Users on re.user_id equals u.user_id
                                                 select new CourseListModel
                                                 {
                                                     course_id = c.course_id,
                                                     user_email = u.user_email,
                                                     subject_name = s.subject_name,
                                                     subject_brief_info = s.subject_brief_info,
                                                     picture = s.picture,
                                                     subject_category = s.subject_category,
                                                     course_start_date = c.course_start_date,
                                                     subject_id = s.subject_id,
                                                 }).OrderBy(n => n.subject_name).Where(n => n.user_email == myemail).ToList();

            foreach (CourseListModel courseListModel in lstMyCourse.ToList())
            {
                if (Convert.ToDateTime(courseListModel.course_start_date) > DateTime.Now)
                {
                    lstMyCourse.Remove(courseListModel);
                }
            }

            ViewBag.lstMyCourse = lstMyCourse;
            return View("/Views/User/SelectCourseToQuiz.cshtml");
        }

        [HttpPost]
        public ActionResult LoadDomain(string subjectID)
        {
            int id = Convert.ToInt32(subjectID);
            List<DomainListModel> lstDomain = (from d in db.Domains.Where(d => d.domain_status == true)
                                               join s in db.Subjects.Where(s => s.subject_status == "Online" && s.subject_id == id)
                                               on d.subject_id equals s.subject_id
                                               join q in db.Questions.Where(q => q.question_status == "Published")
                                               on d.domain_id equals q.domain_id
                                               select new DomainListModel
                                               {
                                                   domain_id = d.domain_id,
                                                   domain_name = d.domain_name,
                                                   subject_id = s.subject_id,
                                               }).Distinct().ToList();
            return Json(new { success = true, data = lstDomain }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadLesson(string subjectID)
        {
            int id = Convert.ToInt32(subjectID);

            List<LessonModel> lstLesson = (from l in db.Lessons.Where(l => l.lesson_status == true && l.lesson_type != "Quiz")
                                           join s in db.Subjects.Where(s => s.subject_id == id)
                                           on l.subject_id equals s.subject_id
                                           join q in db.Questions.Where(q => q.question_status == "Published")
                                           on l.lesson_id equals q.lesson_id
                                           select new LessonModel
                                           {
                                               lesson_id = l.lesson_id,
                                               lesson_name = l.lesson_name,
                                           }).Distinct().ToList();
            return Json(new { success = true, data = lstLesson }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SelectQuizz(string postJson)
        {
            if (Session["testquizz"] != null)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                dynamic testInfo = JValue.Parse(postJson);
                int subjectID = Convert.ToInt32(testInfo.subjectID);
                int numberQuestion = Convert.ToInt32(testInfo.numberQuestion);
                string testType = testInfo.testType;
                if (testType.Equals("test_domain"))
                {
                    int domainID = Convert.ToInt32(testInfo.domainValue);
                    List<QuestionModel> questionsbydomain = (from q in db.Questions
                                                             where q.domain_id == domainID && q.question_status == "Published" && q.subject_id == subjectID
                                                             join s in db.Subjects.Where(s => s.subject_status == "Online")
                                                             on q.subject_id equals s.subject_id
                                                             select new QuestionModel
                                                             {
                                                                 questionID = q.question_id,
                                                                 questiontext = q.question_name,
                                                                 subjectname = s.subject_name,
                                                                 subjectid = s.subject_id,
                                                                 answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                                 {
                                                                     answerID = tq.answer_option_id,
                                                                     answertext = tq.answer_text,
                                                                     isCorrect = tq.answer_corect,
                                                                 }).ToList()
                                                             }).ToList();
                    if (numberQuestion > questionsbydomain.Count())
                    {
                        return Json(new { success = false, data = "Do not have enough number of question" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (questionsbydomain != null)
                        {
                            List<QuestionModel> testdomain = new List<QuestionModel>();
                            int indexq = 0;
                            Random r = new Random();
                            for (int i = 0; i < numberQuestion; i++)
                            {
                                indexq = r.Next(0, questionsbydomain.Count());
                                testdomain.Add(questionsbydomain[indexq]);
                                questionsbydomain.Remove(questionsbydomain[indexq]);
                            }
                            Session["testquizz"] = testdomain;
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else if (testType.Equals("test_lesson"))
                {
                    int lessonID = Convert.ToInt32(testInfo.lessonValue);
                    List<QuestionModel> questionsByLesson = (from q in db.Questions
                                                             where q.lesson_id == lessonID && q.question_status == "Published" && q.subject_id == subjectID
                                                             join s in db.Subjects.Where(s => s.subject_status == "Online")
                                                             on q.subject_id equals s.subject_id
                                                             select new QuestionModel
                                                             {
                                                                 questionID = q.question_id,
                                                                 questiontext = q.question_name,
                                                                 subjectname = s.subject_name,
                                                                 subjectid = s.subject_id,
                                                                 answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                                 {
                                                                     answerID = tq.answer_option_id,
                                                                     answertext = tq.answer_text,
                                                                     isCorrect = tq.answer_corect,
                                                                 }).ToList()
                                                             }).ToList();
                    if (numberQuestion > questionsByLesson.Count())
                    {
                        return Json(new { success = false, data = "Do not have enough number of question" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (questionsByLesson != null)
                        {
                            List<QuestionModel> testlesson = new List<QuestionModel>();
                            int indexq = 0;
                            Random r = new Random();
                            for (int i = 0; i < numberQuestion; i++)
                            {
                                indexq = r.Next(0, questionsByLesson.Count());
                                testlesson.Add(questionsByLesson[indexq]);
                                questionsByLesson.Remove(questionsByLesson[indexq]);
                            }
                            Session["testquizz"] = testlesson;
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CourseRegistration(string course_id)
        {
            string user_email = Session["Email"].ToString();
            int user_id = db.Users.Where(u => u.user_email == user_email).Select(u => u.user_id).FirstOrDefault();
            Registration registration = new Registration();
            registration.user_id = user_id;
            registration.registration_time = DateTime.Now.ToString();
            registration.registration_status = "Submitted";
            registration.course_id = Convert.ToInt32(course_id);
            db.Registrations.Add(registration);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

                throw;
            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TestOnline()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            if (Session["testquizz"] != null)
            {
                var dateQuery = db.Database.SqlQuery<DateTime>("SELECT GETDATE()");
                if (Session["time_start_test_practice"] == null)
                {
                    DateTime serverStartDate = dateQuery.AsEnumerable().First();
                    Session["time_start_test_practice"] = serverStartDate;
                }
                else if (Session["time_start_test_practice"] != null)
                {
                    DateTime serverEndDate = dateQuery.AsEnumerable().First();
                    TimeSpan ts = TimeSpan.Parse((serverEndDate - (DateTime)Session["time_start_test_practice"]).ToString());
                    Session["time_during_pratice"] = Math.Round(ts.TotalSeconds);
                }
                return View("/Views/User/PraticeOnlineTest.cshtml");
            }
            else
            {
                return RedirectToAction("SelectCourseToQuiz", "Home");
            }
        }

        public ActionResult ListCourses(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, s.picture, s.subject_brief_info from Course c  " +
                "join Subject s on c.subject_id = s.subject_id where convert(datetime,c.course_start_date) >= @datetimenow and c.course_status = 'True' and s.subject_status = 'Online'";
            List<CourseListModel> lstCourses = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).ToList();

            ViewBag.lstCourses = lstCourses.OrderBy(n => n.course_name).ToPagedList(pageNumber, pageSize);
            return View("/Views/User/CourseList.cshtml");
        }

        public ActionResult CourseDetail(int? id)
        {
            if (Session["Email"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                string useremail = Session["Email"].ToString();
                int userid = db.Users.Where(u => u.user_email == useremail).Select(u => u.user_id).FirstOrDefault();
                RegistrationListModel registration = (from c in db.Courses
                                                      where c.course_id == id
                                                      join re in db.Registrations.Where(re => re.user_id == userid)
                                                      on c.course_id equals re.course_id
                                                      select new RegistrationListModel
                                                      {
                                                          registration_status = re.registration_status
                                                      }).FirstOrDefault();


                CourseListModel courseDetail = (from c in db.Courses
                                                where c.course_id == id
                                                join s in db.Subjects
                                                on c.subject_id equals s.subject_id
                                                select new CourseListModel
                                                {
                                                    course_id = c.course_id,
                                                    subject_id = c.subject_id,
                                                    course_name = c.course_name,
                                                    subject_name = s.subject_name,
                                                    ObjectiveCourse = s.ObjectiveCourse,
                                                    course_start_date = c.course_start_date,
                                                    picture = s.picture,
                                                }).FirstOrDefault();

                User teacher = (from u in db.Users
                                join c in db.Courses.Where(c => c.course_id == id && c.course_status == true) on u.user_id equals c.teacher_id
                                select new UserListModel
                                {
                                    user_id = u.user_id,
                                    user_fullname = u.user_fullname,
                                    use_mobile = u.use_mobile,
                                    user_email = u.user_email
                                }
                               ).FirstOrDefault();

                List<LessonModel> lesson = (from l in db.Lessons.Where(l => l.lesson_status == true).OrderBy(l => l.parent_id)
                                            join s in db.Subjects.Where(s => s.subject_id == courseDetail.subject_id) on l.subject_id equals s.subject_id
                                            select new LessonModel
                                            {
                                                lesson_id = l.lesson_id,
                                                lesson_name = l.lesson_name,
                                                lesson_link = l.lesson_link,
                                                lesson_content = l.lesson_content,
                                                parent_id = l.parent_id
                                            }).ToList();

                if (courseDetail == null)
                {
                    return HttpNotFound();
                }

                ViewBag.registration = registration;
                ViewBag.lesson = lesson;
                ViewBag.teacher = teacher;
                ViewBag.courseDetail = courseDetail;
                return View("/Views/User/CourseDetail.cshtml");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CourseListModel courseDetail = (from c in db.Courses
                                                where c.course_id == id
                                                join s in db.Subjects
                                                on c.subject_id equals s.subject_id
                                                select new CourseListModel
                                                {
                                                    course_id = c.course_id,
                                                    subject_id = c.subject_id,
                                                    course_name = c.course_name,
                                                    subject_name = s.subject_name,
                                                    ObjectiveCourse = s.ObjectiveCourse,
                                                    course_start_date = c.course_start_date,
                                                    picture = s.picture,
                                                }).FirstOrDefault();

                User teacher = (from u in db.Users
                                join c in db.Courses.Where(c => c.course_id == id && c.course_status == true) on u.user_id equals c.teacher_id
                                select new UserListModel
                                {
                                    user_id = u.user_id,
                                    user_fullname = u.user_fullname,
                                    use_mobile = u.use_mobile,
                                    user_email = u.user_email
                                }
                               ).FirstOrDefault();

                List<LessonModel> lesson = (from l in db.Lessons.Where(l => l.lesson_status == true).OrderBy(l => l.parent_id)
                                            join s in db.Subjects.Where(s => s.subject_id == courseDetail.subject_id) on l.subject_id equals s.subject_id
                                            select new LessonModel
                                            {
                                                lesson_id = l.lesson_id,
                                                lesson_name = l.lesson_name,
                                                lesson_link = l.lesson_link,
                                                lesson_content = l.lesson_content,
                                                parent_id = l.parent_id
                                            }).ToList();

                if (courseDetail == null)
                {
                    return HttpNotFound();
                }

                ViewBag.registration = null;
                ViewBag.lesson = lesson;
                ViewBag.teacher = teacher;
                ViewBag.courseDetail = courseDetail;
                return View("/Views/User/CourseDetail.cshtml");
            }
        }

        public ActionResult CheckYourCourse(int? id)
        {
            if (Session["Email"] != null)
            {
                string email = Session["Email"].ToString();
                CourseListModel checkYourCourse = (from s in db.Subjects.Where(s => s.subject_status == "Online")
                                                   join c in db.Courses.Where(s => s.course_id == id && s.course_status == true)
                                                   on s.subject_id equals c.subject_id
                                                   join re in db.Registrations.Where(re => re.registration_status == "Approved") on c.course_id equals re.course_id
                                                   join u in db.Users.Where(u => u.user_email == email) on re.user_id equals u.user_id
                                                   select new CourseListModel
                                                   {
                                                       registration_id = re.registration_id,
                                                       subject_id = s.subject_id,
                                                       course_id = c.course_id,
                                                       course_name = c.course_name,
                                                       user_email = u.user_email,
                                                       subject_name = s.subject_name,
                                                       subject_brief_info = s.subject_brief_info,
                                                       picture = s.picture,
                                                       subject_category = s.subject_category,
                                                       course_start_date = c.course_start_date,
                                                       course_end_date = c.course_end_date
                                                   }).FirstOrDefault();

                if (checkYourCourse != null)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    if (Convert.ToDateTime(checkYourCourse.course_start_date) >= DateTime.Now)
                    {
                        TempData["ErrorMessage"] = "Your course is not opened yet";
                        return RedirectToAction("YourCourse", "Home");
                    }
                    else
                    {
                        int idLesson = db.Lessons.Where(l => l.lesson_status == true).Select(s => s.lesson_id).Min();
                        Session["course"] = checkYourCourse;
                        List<LessonModel> lstlesson = (from l in db.Lessons.OrderBy(l => l.parent_id).Where(l => l.lesson_status == true)
                                                       join s in db.Subjects.Where(s => s.subject_id == checkYourCourse.subject_id && s.subject_status == "Online") on l.subject_id equals s.subject_id
                                                       select new LessonModel
                                                       {
                                                           lesson_id = l.lesson_id,
                                                           lesson_name = l.lesson_name,
                                                           lesson_link = l.lesson_link,
                                                           lesson_content = l.lesson_content,
                                                           parent_id = l.parent_id
                                                       }).ToList();
                        Session["lstlesson"] = lstlesson;
                        return RedirectToAction("LessonDetail", "Home", new { @id = idLesson });
                    }
                }
                else
                {
                    return RedirectToAction("CourseDetail", "Home", new { @id = id });
                }
            }
            else
            {
                return RedirectToAction("CourseDetail", "Home", new
                {
                    @id = id
                });
            }

        }


        public ActionResult YourCourse(int? page)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Home_User", "Home");
            }
            string myemail = Session["Email"].ToString();
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, u.user_email, s.subject_category," +
                "s.picture, s.subject_brief_info, CASE WHEN convert(datetime,c.course_start_date) >= @datetimenow Then 'Waiting of Course Open' Else 'Go To Course' END as status_course " +
                "from Course c join Subject s on c.subject_id = s.subject_id " +
                 "join  Registration re on c.course_id = re.course_id join [User] u on  re.user_id = u.user_id where c.course_status = 'True' and  re.registration_status = 'Approved' and s.subject_status = 'Online'";
            List<CourseListModel> lstMyCourse = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).Where(c => c.user_email == myemail).ToList();
            ViewBag.lstMyCourse = lstMyCourse.ToPagedList(pageNumber, pageSize);
            return View("/Views/User/MyCourse.cshtml");
        }

        [HttpPost]
        public ActionResult SubmitQuiz(List<QuizResultModel> resultQuiz)
        {
            Session["configModel"] = null;
            var dateQuery = db.Database.SqlQuery<DateTime>("SELECT GETDATE()");
            DateTime serverEndDate = dateQuery.AsEnumerable().First();
            TimeSpan ts = TimeSpan.Parse((serverEndDate - (DateTime)Session["time_start_test_exam"]).ToString());
            Session["time_during_exam_test"] = Math.Round(ts.TotalSeconds);
            int timefishish = Convert.ToInt32(Session["time_during_exam_test"].ToString());
            string email = Session["Email"].ToString();
            int userid = db.Users.Where(u => u.user_email == email).Select(u => u.user_id).FirstOrDefault();
            Session["time_during_exam_test"] = null;
            Session["ExamTest"] = null;
            Session["time_test_exam"] = null;
            Session["lesson_quiz_id"] = null;
            List<QuizResultModel> finalResultQuiz = new List<QuizResultModel>();
            double numbercorrect = 0;
            foreach (QuizResultModel answser in resultQuiz)
            {
                string corrrectresult = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID && ao.answer_corect == true).Select(ao => ao.answer_text).FirstOrDefault();

                if (answser.answertext == corrrectresult)
                {
                    numbercorrect++;
                    QuizResultModel result = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID).Select(
                    ao => new QuizResultModel
                    {
                        questionID = ao.question_id,
                        answertext = answser.answertext,
                        isCorrect = true,
                        answercorrect = corrrectresult,
                        timeduration = timefishish,
                    }).FirstOrDefault();
                    finalResultQuiz.Add(result);

                }
                else
                {
                    QuizResultModel result = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID).Select(
                    ao => new QuizResultModel
                    {
                        questionID = ao.question_id,
                        answertext = answser.answertext,
                        isCorrect = false,
                        answercorrect = corrrectresult,
                        timeduration = timefishish,
                    }).FirstOrDefault();
                    finalResultQuiz.Add(result);
                }
            }

            double yourgrade = Math.Round((numbercorrect / (resultQuiz.Count())) * 10, 2);

            Grade grade = new Grade();
            CourseListModel courseListModel = Session["course"] as CourseListModel;
            grade.registration_id = courseListModel.registration_id;
            grade.course_id = courseListModel.course_id;
            grade.user_id = userid;
            ConfigModel courseWorkList = Session["test_exam"] as ConfigModel;
            Session["test_exam"] = null;
            grade.coursework_id = courseWorkList.coursework_id;
            grade.grade_user = yourgrade;
            if (yourgrade == 10)
            {
                grade.grade_comment = "Excellent";
            }
            else if (yourgrade < 10 && yourgrade >= 9)
            {
                grade.grade_comment = "Well";
            }
            else if (yourgrade < 9 && yourgrade >= 8)
            {
                grade.grade_comment = "Good";
            }
            else if (yourgrade < 8 && yourgrade >= 7)
            {
                grade.grade_comment = "Average";
            }
            else if (yourgrade < 7 && yourgrade >= 5)
            {
                grade.grade_comment = "Below Average";
            }
            else if (yourgrade < 5 && yourgrade > 0)
            {
                grade.grade_comment = "Poor";
            }
            else
            {
                grade.grade_comment = "Terrible";
            }
            db.Grades.Add(grade);
            db.SaveChanges();
            TestResult testResult = db.TestResults.Where(tr => tr.user_id == userid && tr.test_id == courseWorkList.test_id && tr.exam_id == courseWorkList.exam_id && tr.test_type == "Exam Test").FirstOrDefault();
            if (testResult == null)
            {
                testResult = new TestResult();
                testResult.user_id = userid;
                testResult.test_id = courseWorkList.test_id;
                testResult.exam_id = courseWorkList.exam_id;
                testResult.test_type = "Exam Test";
                testResult.tested = 1;
                testResult.average = Math.Round((numbercorrect / (resultQuiz.Count())) * 100, 2).ToString() + "%";
                if (yourgrade > (courseWorkList.pass_rate / 10))
                {
                    testResult.pass_rate = "100%";
                }
                else
                {
                    testResult.pass_rate = "0%";
                }
                testResult.tested_at = Session["time_start_test_exam"].ToString();

                try
                {
                    db.TestResults.Add(testResult);
                }
                catch (Exception e)
                {

                    throw;
                }
                db.SaveChanges();
            }
            else
            {
                double old_average = (Convert.ToDouble(testResult.average.Substring(0, testResult.average.Length - 1))) / 10;
                double new_average = (old_average * Convert.ToDouble(testResult.tested) + yourgrade) / (Convert.ToDouble(testResult.tested) + 1);
                testResult.average = (Math.Round(new_average * 10, 2)).ToString() + "%";
                double old_pass_rate = (Convert.ToDouble(testResult.pass_rate.Substring(0, testResult.pass_rate.Length - 1))) / 100;
                double new_pass_rate = 0;
                if (yourgrade < (courseWorkList.pass_rate / 10))
                {
                    new_pass_rate = old_pass_rate * Convert.ToDouble(testResult.tested) / (Convert.ToDouble(testResult.tested) + 1);
                }
                else
                {
                    new_pass_rate = (old_pass_rate * Convert.ToDouble(testResult.tested) + 1) / (Convert.ToDouble(testResult.tested) + 1);
                }
                testResult.pass_rate = Math.Round(new_pass_rate * 100, 2).ToString() + "%";
                testResult.tested += 1;
                testResult.tested_at = Session["time_start_test_exam"].ToString();
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            Session["time_start_test_exam"] = null;
            int testResultID = db.TestResults.Select(r => r.test_user_id).Max();

            foreach (QuizResultModel quiz in finalResultQuiz)
            {
                TestAnswer testAnswer = new TestAnswer();
                testAnswer.test_user_id = testResultID;
                testAnswer.user_id = userid;
                testAnswer.question_id = quiz.questionID;
                if (quiz.answertext == null)
                {
                    testAnswer.user_answer = "";
                }
                else
                {
                    testAnswer.user_answer = quiz.answertext;
                }
                testAnswer.test_id = courseWorkList.test_id;
                db.TestAnswers.Add(testAnswer);
                db.SaveChanges();
            }


            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitPracticeQuiz(List<QuizResultModel> resultQuiz)
        {
            var dateQuery = db.Database.SqlQuery<DateTime>("SELECT GETDATE()");
            DateTime serverEndDate = dateQuery.AsEnumerable().First();
            TimeSpan ts = TimeSpan.Parse((serverEndDate - (DateTime)Session["time_start_test_practice"]).ToString());
            Session["time_during_pratice"] = Math.Round(ts.TotalSeconds);
            int timefishish = Convert.ToInt32(Session["time_during_pratice"].ToString());
            List<QuestionModel> lstQuestionModel = Session["testquizz"] as List<QuestionModel>;
            int subjectID = lstQuestionModel[0].subjectid;
            double numbercorrect = 0;
            double time_test = Math.Round(Convert.ToDouble(Session["time_during_pratice"]) / 60, 2);
            Exam exam = new Exam();
            exam.exam_level = "Practice";
            exam.exam_name = "Practice";
            exam.exam_is_practice = true;
            exam.subject_id = subjectID;
            exam.exam_duration = time_test;
            exam.exam_description = "practice quiz of student";
            exam.test_type = "Practice Test";
            exam.pass_rate = 0;
            db.Exams.Add(exam);
            db.SaveChanges();
            List<QuizResultModel> finalResultQuiz = new List<QuizResultModel>();

            foreach (QuizResultModel answser in resultQuiz)
            {
                string corrrectresult = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID && ao.answer_corect == true).Select(ao => ao.answer_text).FirstOrDefault();

                if (answser.answertext == corrrectresult)
                {
                    numbercorrect++;
                    QuizResultModel result = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID).Select(
                    ao => new QuizResultModel
                    {
                        questionID = ao.question_id,
                        answertext = answser.answertext,
                        isCorrect = true,
                        answercorrect = corrrectresult,
                        timeduration = timefishish,
                    }).FirstOrDefault();
                    finalResultQuiz.Add(result);
                }
                else
                {
                    QuizResultModel result = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID).Select(
                    ao => new QuizResultModel
                    {
                        questionID = ao.question_id,
                        answertext = answser.answertext,
                        isCorrect = false,
                        answercorrect = corrrectresult,
                        timeduration = timefishish,
                    }).FirstOrDefault();
                    finalResultQuiz.Add(result);
                }
            }
            string email = Session["Email"].ToString();
            int userid = db.Users.Where(u => u.user_email == email).Select(u => u.user_id).FirstOrDefault();

            TestResult testResult = new TestResult();
            testResult.user_id = userid;
            testResult.exam_id = db.Exams.Select(ex => ex.exam_id).Max();
            testResult.test_type = "Pratice Test";
            testResult.tested = 1;
            double average = numbercorrect / lstQuestionModel.Count();
            testResult.average = Math.Round(average * 100, 2).ToString() + "%";
            testResult.pass_rate = "100%";
            string time_start_test = Session["time_start_test_practice"].ToString();
            testResult.tested_at = time_start_test;
            db.TestResults.Add(testResult);
            db.SaveChanges();
            int testResultID = db.TestResults.Select(r => r.test_user_id).Max();
            foreach (QuizResultModel quiz in finalResultQuiz)
            {
                TestAnswer testAnswer = new TestAnswer();
                testAnswer.test_user_id = testResultID;
                testAnswer.user_id = userid;
                testAnswer.question_id = quiz.questionID;
                if (quiz.answertext == null)
                {
                    testAnswer.user_answer = "";
                }
                else
                {
                    testAnswer.user_answer = quiz.answertext;
                }
                db.TestAnswers.Add(testAnswer);
                db.SaveChanges();
            }
            Session["testquizz"] = null;
            Session["time_during_pratice"] = null;
            Session["time_start_test_practice"] = null;
            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LessonDetail(int? id)
        {
            if(Session["course"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (Session["ExamTest"] != null)
            {
                var dateQuery = db.Database.SqlQuery<DateTime>("SELECT GETDATE()");
                if (Session["time_start_test_exam"] == null)
                {
                    DateTime serverStartDate = dateQuery.AsEnumerable().First();
                    Session["time_start_test_exam"] = serverStartDate;
                }
                else if (Session["time_start_test_exam"] != null)
                {
                    DateTime serverEndDate = dateQuery.AsEnumerable().First();
                    TimeSpan ts = TimeSpan.Parse((serverEndDate - (DateTime)Session["time_start_test_exam"]).ToString());
                    Session["time_during_exam_test"] = Math.Round(ts.TotalSeconds);
                }
                id = Convert.ToInt32(Session["lesson_quiz_id"]);
            }
            ViewBag.Current2 = null;
            ViewBag.Current1 = null;
            ViewBag.lesson = null;
            Lesson lesson = db.Lessons.Where(n => n.lesson_id == id).FirstOrDefault();
            Lesson lesson2 = db.Lessons.Where(n => n.lesson_id == lesson.parent_id).FirstOrDefault();

            if (lesson.lesson_type == "Quiz")
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string sql = "select ls.subject_id, e.exam_level, ISNULL(ec.lesson_id, 0)AS lesson_id, " +
                                "ISNULL(ec.lesson_size, 0) AS lesson_size, ISNULL(ec.domain_id, 0) AS domain_id, ISNULL(ec.domain_size, 0) AS domain_size, " +
                                "e.exam_duration, cw.due_date, ISNULL(et.test_code, 0) AS test_code, et.test_id, " +
                                "e.pass_rate, cw.coursework_id, e.exam_id " +
                                "From Lesson ls " +
                                "join Coursework cw on ls.coursework_id = cw.coursework_id " +
                                "join ExamTest et on cw.test_id = et.test_id " +
                                "join Exam e on et.exam_id = e.exam_id " +
                                "join ExamConfig ec on e.exam_id = ec.exam_id " +
                                "where ls.lesson_id = 23";

                    ConfigModel configModel = db.Database.SqlQuery<ConfigModel>(sql, new SqlParameter("id", id)).FirstOrDefault();
                    Session["lesson_quiz_id"] = id;
                    if (Convert.ToDateTime(configModel.due_date) > DateTime.Now)
                    {
                        ViewBag.lessonQuiz = null;
                    }
                    ViewBag.lessonQuiz = configModel;
                    Session["configModel"] = configModel;
                }
            }
            else
            {
                ViewBag.lessonQuiz = null;
                Session["configModel"] = null;
            }

            ViewBag.Current2 = lesson2.lesson_name;
            ViewBag.Current1 = lesson.lesson_name;
            ViewBag.lesson = lesson;
            ViewBag.lessonType = lesson.lesson_type;

            return View("/Views/User/StudyOnline.cshtml");
        }

        public ActionResult checkExamTest(string test_password)
        {
            ConfigModel configmodel = Session["configModel"] as ConfigModel;
            try
            {
                double timer = configmodel.exam_duration;
                Session["test_exam"] = configmodel;
                Session["time_test_exam"] = timer * 60;
                var dateQuery = db.Database.SqlQuery<DateTime>("SELECT GETDATE()");
                if (Session["time_start_test_exam"] == null)
                {
                    DateTime serverStartDate = dateQuery.AsEnumerable().First();
                    Session["time_start_test_exam"] = serverStartDate;
                }
                else if (Session["time_start_test_exam"] != null)
                {
                    DateTime serverEndDate = dateQuery.AsEnumerable().First();
                    TimeSpan ts = TimeSpan.Parse((serverEndDate - (DateTime)Session["time_start_test_exam"]).ToString());
                    Session["time_during_exam_test"] = Math.Round(ts.TotalSeconds);
                }
            }
            catch (Exception e)
            {

                throw;
            }
            List<int> questionID = new List<int>();
            List<int> questionIDNew = new List<int>();
            List<QuestionModel> questionModels = new List<QuestionModel>();
            int indexq = 0;
            if (configmodel.domain_id != 0)
            {
                string question_level = configmodel.exam_level;
                questionID = db.Questions.Where(tq => tq.domain_id == configmodel.domain_id && tq.subject_id == configmodel.subject_id && tq.question_level == question_level).Select(tq => tq.question_id).ToList();
                if (configmodel.domain_size < questionID.Count)
                {
                    Random random = new Random();
                    for (int i = 0; i < configmodel.domain_size; i++)
                    {
                        indexq = random.Next(0, questionID.Count());
                        questionIDNew.Add(questionID[indexq]);
                        questionID.Remove(questionID[indexq]);
                    }
                    foreach (int idques in questionIDNew)
                    {
                        QuestionModel questions = (from q in db.Questions
                                                   where q.question_status == "Published" && q.question_id == idques
                                                   select new QuestionModel
                                                   {
                                                       questionID = q.question_id,
                                                       questiontext = q.question_name,
                                                       answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                       {
                                                           answerID = tq.answer_option_id,
                                                           answertext = tq.answer_text,
                                                           isCorrect = tq.answer_corect,
                                                       }).ToList()
                                                   }).FirstOrDefault();
                        questionModels.Add(questions);
                    }
                }
                else
                {
                    foreach (int idques in questionID)
                    {
                        QuestionModel questions = (from q in db.Questions
                                                   where q.question_status == "Published" && q.question_id == idques
                                                   select new QuestionModel
                                                   {
                                                       questionID = q.question_id,
                                                       questiontext = q.question_name,
                                                       answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                       {
                                                           answerID = tq.answer_option_id,
                                                           answertext = tq.answer_text,
                                                           isCorrect = tq.answer_corect,
                                                       }).ToList()
                                                   }).FirstOrDefault();
                        questionModels.Add(questions);
                    }
                }
                Session["ExamTest"] = questionModels;
            }
            else if (configmodel.lesson_id != 0)
            {
                int idles = configmodel.lesson_id;
                List<int> checkparentID = db.Lessons.Where(l => l.parent_id == idles && l.lesson_id != idles).Select(l => l.lesson_id).ToList();
                if (checkparentID == null)
                {
                    string question_level = configmodel.exam_level;
                    questionID = db.Questions.Where(tq => tq.lesson_id == configmodel.lesson_id && tq.subject_id == configmodel.subject_id && tq.question_level == question_level).Select(tq => tq.question_id).ToList();
                }else if(checkparentID != null)
                {
                    string question_level = configmodel.exam_level;
                    checkparentID.Add(configmodel.lesson_id);
                    foreach (int lesson in checkparentID.ToList())
                    {
                        List<int> questionidnew = db.Questions.Where(tq => tq.lesson_id == lesson && tq.subject_id == configmodel.subject_id && tq.question_level == question_level).Select(tq => tq.question_id).ToList();
                        questionID.AddRange(questionidnew);
                    }
                }
                if (configmodel.lesson_size < questionID.Count)
                {
                    Random random = new Random();
                    for (int i = 0; i < configmodel.domain_size; i++)
                    {
                        indexq = random.Next(0, questionID.Count());
                        questionIDNew.Add(questionID[indexq]);
                        questionID.Remove(questionID[indexq]);
                    }
                    foreach (int idques in questionIDNew)
                    {
                        QuestionModel questions = (from q in db.Questions
                                                   where q.question_status == "Published" && q.question_id == idques
                                                   select new QuestionModel
                                                   {
                                                       questionID = q.question_id,
                                                       questiontext = q.question_name,
                                                       answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                       {
                                                           answerID = tq.answer_option_id,
                                                           answertext = tq.answer_text,
                                                           isCorrect = tq.answer_corect,
                                                       }).ToList()
                                                   }).FirstOrDefault();
                        questionModels.Add(questions);
                    }
                }
                else
                {
                    foreach (int idques in questionID)
                    {
                        QuestionModel questions = (from q in db.Questions
                                                   where q.question_status == "Published" && q.question_id == idques
                                                   select new QuestionModel
                                                   {
                                                       questionID = q.question_id,
                                                       questiontext = q.question_name,
                                                       answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                       {
                                                           answerID = tq.answer_option_id,
                                                           answertext = tq.answer_text,
                                                           isCorrect = tq.answer_corect,
                                                       }).ToList()
                                                   }).FirstOrDefault();
                        questionModels.Add(questions);
                    }
                }
                Session["ExamTest"] = questionModels;
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAllPracticeTest()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            string email = Session["Email"].ToString();

            var testdetail = (from tr in db.TestResults
                              join ex in db.Exams.Where(ex => ex.exam_is_practice == true) on tr.exam_id equals ex.exam_id
                              join s in db.Subjects on ex.subject_id equals s.subject_id
                              join ur in db.Users.Where(ur => ur.user_email == email) on tr.user_id equals ur.user_id
                              join c in db.Courses.Where(c => c.course_status == true) on s.subject_id equals c.subject_id
                              select new ResultModel
                              {
                                  test_user_id = tr.test_user_id,
                                  course_name = c.course_name,
                                  subject_name = s.subject_name,
                                  average = tr.average,
                                  time_duration = ex.exam_duration ?? 0,
                                  tested_at = tr.tested_at,
                              }).ToList();

            int totalrows = testdetail.Count;
            int totalrowsafterfiltering = testdetail.Count;
            testdetail = testdetail.Skip(start).Take(length).ToList();
            testdetail = testdetail.OrderBy(sortColumnName + " " + sortDirection).ToList();
            return Json(new { success = true, data = testdetail, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PracticeReviewDetail(int id)
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            string email = Session["Email"].ToString();
            int userid = db.Users.Where(u => u.user_email == email).Select(u => u.user_id).FirstOrDefault();
            List<ResultModel> testdetail = (from t in db.TestResults.Where(t => t.test_user_id == id && t.user_id == userid)
                                            join q in db.TestAnswers on t.test_user_id equals q.test_user_id
                                            select new ResultModel
                                            {
                                                test_answer_id = q.test_answer_id,
                                                question_id = q.question_id,
                                                user_answer = q.user_answer,
                                            }).ToList();
            List<QuestionModel> questionModels = new List<QuestionModel>();
            foreach (ResultModel resultModel in testdetail)
            {
                string corrrectresult = db.AnswerOptions.Where(ao => ao.question_id == resultModel.question_id && ao.answer_corect == true).Select(ao => ao.answer_text).FirstOrDefault();
                QuestionModel questions = (from q in db.Questions
                                           where q.question_status == "Published" && q.question_id == resultModel.question_id
                                           select new QuestionModel
                                           {
                                               questionID = q.question_id,
                                               questiontext = q.question_name,
                                               useranswer = resultModel.user_answer,
                                               correctanswer = corrrectresult,
                                               answers = q.AnswerOptions.Select(tq => new AnswerModel
                                               {
                                                   answerID = tq.answer_option_id,
                                                   answertext = tq.answer_text,
                                                   isCorrect = tq.answer_corect,
                                               }).ToList()
                                           }).FirstOrDefault();
                questionModels.Add(questions);
            }
            ViewBag.questionModels = questionModels;
            return View("/Views/User/ReviewPraticeTest.cshtml");
        }

    }
}
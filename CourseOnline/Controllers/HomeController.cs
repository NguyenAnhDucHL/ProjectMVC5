using CourseOnline.Models;
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

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        public ActionResult Home_CMS()
        {
            return View("/Views/CMS/Home.cshtml");
        }
        public ActionResult Home_User()
        {

            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, s.picture, s.subject_brief_info from Course c  " +
                "join Subject s on c.subject_id = s.subject_id where convert(datetime,c.course_start_date) >= @datetimenow and c.course_status = 'True' and s.subject_status = 'Online'";
            List<CourseListModel> lstCourse = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).Take(3).ToList();
            ViewBag.lstCourse = lstCourse;

            var lstPost = db.Posts.Take(7).Where(n => n.post_status == "Published").ToList();
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
            Session["Name"] = name;
            Session["Email"] = email;
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
                                new SqlParameter("user_status", ""),
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
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        else
                        {

                            string userPicture = db.Users.Where(u => u.user_email == email).Select(u => u.user_image).FirstOrDefault();
                            Session["Picture"] = userPicture;
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
            if (Session["permission"].Equals("Permission 1") || Session["permission"].Equals("Permission 2"))
            {
                return RedirectToAction("Home_CMS", "Home");
            }
            else
            {
                return RedirectToAction("Home_User", "Home");
            }
        }

        public void GetPermission(string email)
        {
            List<String> Permission = new List<string>();
            var checkPermission = (from u in db.Users.Where(x => x.user_email == email)
                                   join ur in db.UserRoles on u.user_id equals ur.user_id
                                   join r in db.Roles on ur.role_id equals r.role_id
                                   join rp in db.RolePermissions on r.role_id equals rp.role_id
                                   join p in db.Permissions on rp.permission_id equals p.permission_id
                                   select p.permission_name);

            Session["permission"] = "";
            foreach (string permissionName in checkPermission)
            {
                if (permissionName.Equals("Permission 1"))
                {
                    Session["permission"] = "Permission 1";
                }
                else if (permissionName.Equals("Permission 2"))
                {
                    Session["permission"] = "Permission 2";
                }
                else
                {
                    Session["permission"] = "Permission 3";
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

                    if (ava == "/Assets/dist/img/" + "user" + editUser.id + ".png")
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
                        string relative_path = "/Assets/dist/img/" + "user" + editUser.id + ".png";
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
                TempData["ErrorMessage"] = "You need to login to join the Pratice Test";
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

            List<LessonModel> lstLesson = (from l in db.Lessons.Where(l => l.lesson_status == true)
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
                        Session["testquizz"] = questionsbydomain;
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else if (testType.Equals("test_lesson"))
            {
                int lessonID = Convert.ToInt32(testInfo.lessonValue);
                List<QuestionModel> questions = (from q in db.Questions
                                                 where q.lesson_id == lessonID && q.question_status == "Published" && q.subject_id == subjectID
                                                 join s in db.Subjects.Where(s => s.subject_status == "Online")
                                                 on q.subject_id equals s.subject_id
                                                 select new QuestionModel
                                                 {
                                                     questionID = q.question_id,
                                                     questiontext = q.question_name,
                                                     subjectname = s.subject_name,
                                                     answers = q.AnswerOptions.Select(tq => new AnswerModel
                                                     {
                                                         answerID = tq.answer_option_id,
                                                         answertext = tq.answer_text,
                                                         isCorrect = tq.answer_corect,
                                                     }).ToList()
                                                 }).ToList();
                if (numberQuestion > questions.Count())
                {
                    return Json(new { success = false, data = "Do not have enough number of question" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (questions != null)
                    {
                        Session["testquizz"] = questions;
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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
        public ActionResult TestOnline(int? count)
        {
            if (Session["testquizz"] != null)
            {
                List<QuestionModel> testquizz = Session["testquizz"] as List<QuestionModel>;
                ViewBag.examtest = testquizz;
                return View("/Views/User/PraticeOnlineTest.cshtml");
            }
            else
            {
                return RedirectToAction("Home_User", "Home");
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
            string myemail = Session["Email"].ToString();
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            string sql = "select s.subject_name, c.course_start_date , c.course_end_date , c.course_id, c.course_name, u.user_email, s.subject_category," +
                "s.picture, s.subject_brief_info, CASE WHEN convert(datetime,c.course_start_date) >= @datetimenow Then 'Waiting of Course Open' Else 'Join Course' END as status_course " +
                "from Course c join Subject s on c.subject_id = s.subject_id " +
                 "join  Registration re on c.course_id = re.course_id join [User] u on  re.user_id = u.user_id where c.course_status = 'True' and  re.registration_status = 'Approved' and s.subject_status = 'Online'";
            List<CourseListModel> lstMyCourse = db.Database.SqlQuery<CourseListModel>(sql, new SqlParameter("datetimenow", DateTime.Now)).Where(c => c.user_email == myemail).ToList();
            ViewBag.lstMyCourse = lstMyCourse.ToPagedList(pageNumber, pageSize);
            return View("/Views/User/MyCourse.cshtml");
        }

        [HttpPost]
        public ActionResult SubmitQuiz(List<QuizResultModel> resultQuiz)
        {
            List<QuizResultModel> finalResultQuiz = new List<QuizResultModel>();

            foreach (QuizResultModel answser in resultQuiz)
            {
                string corrrectresult = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID && ao.answer_corect == true).Select(ao => ao.answer_text).FirstOrDefault();

                QuizResultModel result = db.AnswerOptions.Where(ao => ao.question_id == answser.questionID && ao.answer_text == answser.answertext).Select(
                    ao => new QuizResultModel
                    {
                        questionID = ao.question_id,
                        answertext = ao.answer_text,
                        isCorrect = ao.answer_corect,
                        answercorrect = corrrectresult,
                    }).FirstOrDefault();
                finalResultQuiz.Add(result);
            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LessonDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Current2 = null;
            ViewBag.Current1 = null;
            ViewBag.lesson = null;
            Lesson lesson = db.Lessons.Where(n => n.lesson_id == id).FirstOrDefault();
            Lesson lesson2 = db.Lessons.Where(n => n.lesson_id == lesson.parent_id).FirstOrDefault();

            if (lesson.lesson_type == "Quiz")
            {
                LessonQuizModel lessonQuizModels = (from l in db.Lessons.Where(l => l.lesson_id == id && l.lesson_status == true)
                                                    join c in db.Courseworks.Where(c => c.coursework_status == true) on l.coursework_id equals c.coursework_id
                                                    join et in db.ExamTests on c.test_id equals et.test_id
                                                    join tq in db.TestQuestions on et.test_id equals tq.test_id
                                                    join ex in db.Exams on et.exam_id equals ex.exam_id
                                                    select new LessonQuizModel
                                                    {
                                                        test_id = tq.test_id,
                                                        test_name = et.test_name,
                                                        exam_duration = ex.exam_duration,
                                                        due_date = c.due_date,
                                                    }).FirstOrDefault();
                if (Convert.ToDateTime(lessonQuizModels.due_date) > DateTime.Now)
                {
                    ViewBag.lessonQuiz = null;
                }
                ViewBag.lessonQuiz = lessonQuizModels;
            }
            else
            {
                ViewBag.lessonQuiz = null;
            }

            ViewBag.Current2 = lesson2.lesson_name;
            ViewBag.Current1 = lesson.lesson_name;
            ViewBag.lesson = lesson;
            ViewBag.lessonType = lesson.lesson_type;

            return View("/Views/User/StudyOnline.cshtml");
        }

        public ActionResult checkExamTest(string test_id, string test_password)
        {
            ExamTest examTest = null;
            int idtest = Convert.ToInt32(test_id);
            try
            {
                examTest = db.ExamTests.Where(et => et.test_id == idtest && et.test_code == test_password).FirstOrDefault();
            }
            catch (Exception e)
            {

                throw;
            }
            if (examTest != null)
            {
                List<int> questionID = db.TestQuestions.Where(tq => tq.test_id == idtest).Select(tq => tq.question_id).ToList();
                List<QuestionModel> questionModels = new List<QuestionModel>();
                foreach (int idques in questionID)
                {
                    List<QuestionModel> questions = (from q in db.Questions
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
                                                     }).ToList();
                    questionModels.AddRange(questions);
                }
                Session["ExamTest"] = questionModels;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
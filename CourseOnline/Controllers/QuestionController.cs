using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class QuestionController : Controller
    {
        // GET: Question
        [Route("QuestionList")]
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listSubject = db.Subjects.Select(s => s.subject_name).Distinct().ToList();
                ViewBag.subjectName = listSubject;
                var listDomain = db.Domains.Select(d => d.domain_name).Distinct().ToList();
                ViewBag.domainName = listDomain;
                var listLesson = db.Lessons.Select(l => l.lesson_name).Distinct().ToList();
                ViewBag.lessonName = listLesson;
                var listLevel = db.Questions.Select(q => q.question_level).Distinct().ToList();
                ViewBag.questionLevel = listLevel;
                var listStatus = db.Questions.Select(q => q.question_status).Distinct().ToList();
                ViewBag.questionStatus = listStatus;
            }
            return View("/Views/CMS/Question/QuestionList.cshtml");
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
            string domain = filterByJson.domainName;
            string lesson = filterByJson.lessonName;
            string level = filterByJson.questionLevel;
            string status = filterByJson.questionStatus;

            if (subject.Equals(All.ALL_SUBJECT))
            {
                subject = "";
            }
            if (domain.Equals(All.ALL_DOMAIN))
            {
                domain = "";
            }
            if (lesson.Equals(All.ALL_LESSON))
            {
                lesson = "";
            }
            if (level.Equals(All.ALL_LEVEL))
            {
                level = "";
            }
            if (status.Equals(All.ALL_STATUS))
            {
                status = "";
            }

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var questionList = (from q in db.Questions
                                    join s in db.Subjects on q.subject_id equals s.subject_id
                                    join d in db.Domains on q.domain_id equals d.domain_id
                                    join l in db.Lessons on q.lesson_id equals l.lesson_id
                                    where q.question_level.Contains(level)
                                    && q.question_status.Contains(status)
                                    && s.subject_name.Contains(subject)
                                    && d.domain_name.Contains(domain)
                                    && l.lesson_name.Contains(lesson)
                                    select new QuestionListModel
                                    {
                                        question_id = q.question_id,
                                        question_name = q.question_name,
                                        question_level = q.question_level,
                                        question_status = q.question_status,
                                        subject_name = s.subject_name,
                                        domain_name = d.domain_name,
                                        lesson_name = l.lesson_name
                                    }).ToList();

                int totalrows = questionList.Count;
                int totalrowsafterfiltering = questionList.Count;
                questionList = questionList.Skip(start).Take(length).ToList();
                questionList = questionList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = questionList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAllQuestion()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select q.question_id, q.question_name, s.subject_name, d.domain_name, l.lesson_name, q.question_level, q.question_status " +
                                "from Question q join [Subject] s " +
                                "on q.subject_id = s.subject_id " +
                                "join Domain d " +
                                "on q.domain_id = d.domain_id " +
                                "join Lesson l " +
                                "on q.lesson_id = l.lesson_id";

                List<QuestionListModel> Questions = db.Database.SqlQuery<QuestionListModel>(sql).ToList();

                int totalrows = Questions.Count;
                int totalrowsafterfiltering = Questions.Count;
                Questions = Questions.Skip(start).Take(length).ToList();
                Questions = Questions.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Questions, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddQuestion()
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).ToList();
                ViewBag.subjectName = listSubject;
                List<Domain> listDomain = db.Domains.Where(d => d.domain_name != null).ToList();
                ViewBag.domainName = listDomain;
                List<Lesson> listLesson = db.Lessons.Where(l => l.lesson_name != null).ToList();
                ViewBag.lessonName = listLesson;
                var listStatus = db.Questions.Select(q => q.question_status).Distinct().ToList();
                ViewBag.statusName = listStatus;
                var listLevel = db.Questions.Select(q => q.question_level).Distinct().ToList();
                ViewBag.levelName = listLevel;
                return View("/Views/CMS/Question/QuestionAdding.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitAddQuestion(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addquestion = JValue.Parse(postJson);

                    Question q = new Question();
                    q.subject_id = addquestion.subjectId;
                    q.domain_id = addquestion.domainId;
                    q.lesson_id = addquestion.lessonId;
                    q.question_name = addquestion.questionName;
                    q.question_level = addquestion.questionLevel;
                    q.question_status = addquestion.questionStatus;
                    db.Questions.Add(q);
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
        public ActionResult deleteQuestion(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var question = db.Questions.Where(q => q.question_id == id).FirstOrDefault();
                if (question != null)
                {
                    db.Questions.Remove(question);
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
        public ActionResult EditQuestion(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select q.question_id, q.question_name, s.subject_name, d.domain_name, l.lesson_name, q.question_level, q.question_status " +
                                "from Question q join [Subject] s " +
                                "on q.subject_id = s.subject_id " +
                                "join Domain d " +
                                "on q.domain_id = d.domain_id " +
                                "join Lesson l " +
                                "on q.lesson_id = l.lesson_id where q.question_id = @id";

                QuestionListModel Questions = db.Database.SqlQuery<QuestionListModel>(sql, new SqlParameter("id" , id)).FirstOrDefault();
                ViewBag.Question = Questions;
                ViewBag.quesSubject = Questions.subject_name;
                ViewBag.quesDomain = Questions.domain_name;
                ViewBag.quesLesson = Questions.lesson_name;

                List<Subject> listSubject = db.Subjects.Where(s => s.subject_name != null).ToList();
                ViewBag.subjectName = listSubject;
                List<Domain> listDomain = db.Domains.Where(d => d.domain_name != null).ToList();
                ViewBag.domainName = listDomain;
                List<Lesson> listLesson = db.Lessons.Where(l => l.lesson_name != null).ToList();
                ViewBag.lessonName = listLesson;
                var listStatus = db.Questions.Select(q => q.question_status).Distinct().ToList();
                ViewBag.statusName = listStatus;
                var listLevel = db.Questions.Select(q => q.question_level).Distinct().ToList();
                ViewBag.levelName = listLevel;

                ViewBag.id = id;
                return View("/Views/CMS/Question/QuestionEditting.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitQuestion(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtQues = JValue.Parse(postJson);
                    int id = edtQues.id;

                    Question q = db.Questions.Where(ques => ques.question_id == id).FirstOrDefault();
                    if (q != null)
                    {
                        q.question_name = edtQues.quesName;
                        q.question_level = edtQues.quesLevel;
                        q.question_status = edtQues.quesStatus;
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
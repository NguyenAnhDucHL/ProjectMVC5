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
using System.Data;
//using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml.Core.ExcelPackage;

namespace CourseOnline.Controllers
{
    public class QuestionController : Controller
    {
        // GET: Question
        
        [Route("QuestionList")]
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/LTContent/Questions");
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
            }
        }

        //[HttpPost]
        //public ActionResult Download() // download template area/location
        //{
        //    string path = Server.MapPath("~/excelfolder/template/demoExcel.xlsx");

        //    FileInfo file = new FileInfo(path);
        //    ExcelPackage excelPackage = new ExcelPackage(file);
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment; filename= ~/excelfolder/template/demoExcel.xlsx");
        //        memoryStream.WriteTo(Response.OutputStream);
        //        //excelPackage.SaveAs(memoryStream);
        //        Response.Flush();
        //        Response.End();
        //    }
        //    return View();
        //}
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile, string subjectname, string lessonName, string domainName)
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

                if (excelfile == null || excelfile.ContentLength == 0)
                {
                    ViewBag.Error = "Please select a file excel file";
                    return View("/Views/CMS/Question/QuestionList.cshtml");
                }
                else
                {
                    if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                    {
                        string path = Server.MapPath("~/excelfolder/" + excelfile.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Error = "File has been exist";
                            return View("/Views/CMS/Question/QuestionList.cshtml");
                        }
                            excelfile.SaveAs(path);
                        //Read data from excel file
                        Excel.Application application = new Excel.Application();
                        Excel.Workbook workbook = application.Workbooks.Open(path);
                        Excel.Worksheet worksheet = workbook.ActiveSheet;
                        Excel.Range range = worksheet.UsedRange;
                        for (int row = 2; row <= range.Rows.Count; row++)
                        {
                            Question q = new Question();
                            q.subject_id = Convert.ToInt32(subjectname);
                            q.domain_id = Convert.ToInt32(domainName);
                            q.lesson_id = Convert.ToInt32(lessonName);
                            q.question_name = ((Excel.Range)range.Cells[row, 1]).Text;
                            q.question_level = ((Excel.Range)range.Cells[row, 6]).Text;
                            q.question_status = ((Excel.Range)range.Cells[row, 7]).Text;
                            db.Questions.Add(q);
                            db.SaveChanges();
                            AnswerOption a = new AnswerOption();
                            string answer = ((Excel.Range)range.Cells[row, 8]).Text;
                            int temp = db.Questions.DefaultIfEmpty().Max(pos => pos == null ? 0 : pos.question_id);
                            a.answer_text = ((Excel.Range)range.Cells[row, 2]).Text;
                            a.question_id = temp;
                            if (answer.Equals("A"))
                            {
                                a.answer_corect = true;
                            }
                            else
                            {
                                a.answer_corect = false;
                            }
                            db.AnswerOptions.Add(a);
                            db.SaveChanges();
                            a.answer_text = ((Excel.Range)range.Cells[row, 3]).Text;
                            a.question_id = temp;
                            if (answer.Equals("B"))
                            {
                                a.answer_corect = true;
                            }
                            else
                            {
                                a.answer_corect = false;
                            }
                            db.AnswerOptions.Add(a);
                            db.SaveChanges();
                            a.answer_text = ((Excel.Range)range.Cells[row, 4]).Text;
                            a.question_id = temp;
                            if (answer.Equals("C"))
                            {
                                a.answer_corect = true;
                            }
                            else
                            {
                                a.answer_corect = false;
                            }
                            db.AnswerOptions.Add(a);
                            db.SaveChanges();
                            a.answer_text = ((Excel.Range)range.Cells[row, 5]).Text;
                            a.question_id = temp;
                            if (answer.Equals("D"))
                            {
                                a.answer_corect = true;
                            }
                            else
                            {
                                a.answer_corect = false;
                            }
                            db.AnswerOptions.Add(a);
                            db.SaveChanges();
                            //System.IO.File.Delete("~/excelfolder/" + excelfile.FileName);
                        }
                        ViewBag.Error = "Import success";
                        return View("/Views/CMS/Question/QuestionList.cshtml");
                    }
                    else
                    {
                        ViewBag.Error = "Please select a excel file";
                        return View("/Views/CMS/Question/QuestionList.cshtml");
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
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "CMS/LTContent/Questions");
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
        public ActionResult SubmitAddQuestion(string postJson, string postJson2)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addquestion = JValue.Parse(postJson);
                    dynamic addanswer = JValue.Parse(postJson2);
                    Question q = new Question();
                    q.subject_id = addquestion.subjectId;
                    q.domain_id = addquestion.domainId;
                    q.lesson_id = addquestion.lessonId;
                    q.question_name = addquestion.questionName;
                    q.question_level = addquestion.questionLevel;
                    q.question_status = addquestion.questionStatus;
                    db.Questions.Add(q);
                    db.SaveChanges();
                    int id = db.Questions.Select(qu => qu.question_id).Max();
                 
                    foreach (var e in addanswer)
                    {
                        AnswerOption ao = new AnswerOption();
                        ao.answer_text = e.answer_text;
                        if (e.answer_corect == "on")
                        {
                            ao.answer_corect = true;
                        }
                        else
                        {
                            ao.answer_corect = false;
                        }
                        ao.question_id = id;
                        db.AnswerOptions.Add(ao);
                        db.SaveChanges();
                    }
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
                var answerOpt = db.AnswerOptions.Where(a => a.question_id == id).ToList();
                var count1 = answerOpt.Count();
                var testAns = db.TestAnswers.Where(ta => ta.question_id == id).ToList();
                var count2 = testAns.Count();
                var testQues = db.TestQuestions.Where(tq => tq.question_id == id).ToList();
                var count3 = testQues.Count();
                var question = db.Questions.Where(q => q.question_id == id).FirstOrDefault();
                if (question != null)
                {
                    if(answerOpt != null)
                    {
                        while(count1 > 0)
                        {
                            var ansOption = db.AnswerOptions.Where(a => a.question_id == id).FirstOrDefault();
                            db.AnswerOptions.Remove(ansOption);
                            db.SaveChanges();
                            count1--;
                        }
                    }
                    if (testAns != null)
                    {
                        while (count2 > 0)
                        {
                            var testAnswer = db.TestAnswers.Where(ta => ta.question_id == id).FirstOrDefault();
                            db.TestAnswers.Remove(testAnswer);
                            db.SaveChanges();
                            count2--;
                        }
                    }
                    if (testQues != null)
                    {
                        while (count3 > 0)
                        {
                            var testQuestion = db.TestQuestions.Where(tq => tq.question_id == id).FirstOrDefault();
                            db.TestQuestions.Remove(testQuestion);
                            db.SaveChanges();
                            count3--;
                        }
                    }
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
                var questionList = (from q in db.Questions
                                    join s in db.Subjects on q.subject_id equals s.subject_id
                                    join d in db.Domains on q.domain_id equals d.domain_id
                                    join l in db.Lessons on q.lesson_id equals l.lesson_id
                                    where q.question_name.Contains(type)
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

                var lstAnswer = db.AnswerOptions.Where(ao => ao.question_id == id).ToList();
                ViewBag.answers = lstAnswer;
                ViewBag.id = id;
                return View("/Views/CMS/Question/QuestionEditting.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitQuestion(string postJson, string postJson2)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtQues = JValue.Parse(postJson);
                    dynamic edtQues2 = JValue.Parse(postJson2);
                    int id = edtQues.id;

                    Question q = db.Questions.Where(ques => ques.question_id == id).FirstOrDefault();
                    List<AnswerOption> answerOptions = db.AnswerOptions.Where(a => a.question_id == id).ToList();
                    foreach(AnswerOption answerOption in answerOptions)
                    {
                        db.AnswerOptions.Remove(answerOption);
                        db.SaveChanges();
                    }
                    List<AnswerOption> answerOptionsNew = new List<AnswerOption>();
                    if (q != null && answerOptions != null)
                    {
                        
                        q.question_name = edtQues.quesName;
                        q.question_level = edtQues.quesLevel;
                        q.question_status = edtQues.quesStatus;
                        
                        foreach(var e in edtQues2)
                        {
                            AnswerOption answeroption = new AnswerOption();
                            answeroption.answer_text = e.answer_text ;
                            if(e.answer_corect == "on")
                            {
                                answeroption.answer_corect = true;
                            }
                            else
                            {
                                answeroption.answer_corect = false;
                            }
                            answeroption.question_id = id;
                            answerOptionsNew.Add(answeroption);

                        }
                        foreach (AnswerOption answerOption in answerOptionsNew)
                        {
                            db.AnswerOptions.Add(answerOption);
                        }
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
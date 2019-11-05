using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace CourseOnline.Controllers
{
    public class SubjectController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        // GET: Course
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

            if(subject == null)
            {
                return HttpNotFound();
            }
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
    }
}
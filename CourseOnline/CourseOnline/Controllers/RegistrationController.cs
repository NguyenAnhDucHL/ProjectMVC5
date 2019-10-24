using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        [Route("RegistrationList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/RegistrationList.cshtml");
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
    }
}
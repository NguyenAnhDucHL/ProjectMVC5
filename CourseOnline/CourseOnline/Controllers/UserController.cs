
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [Route ("UserList")]
        public ActionResult Index()
        {
            return View("~\\Views\\CMS\\UserList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllUser()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (MyCourseEntities1 db = new MyCourseEntities1())
            {
                List<user> arrUser = db.users.ToList();

                int totalrows = arrUser.Count;
                int totalrowsafterfiltering = arrUser.Count;
                arrUser = arrUser.Skip(start).Take(length).ToList();
                arrUser = arrUser.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = arrUser, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Models;

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

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select u.[user_id], u.user_fullname, u.[user_email], u.use_mobile, u.[user_gender], u.[user_status], r.role_name  " +
                            "from[User] u join [UserRole] ur " +
                            "on u.[user_id] = ur.user_id " +
                            "join Roles r " +
                            "on r.role_id = ur.role_id";
                    
                List<UserListModel> userListModels = db.Database.SqlQuery<UserListModel>(sql).ToList();

                int totalrows = userListModels.Count;
                int totalrowsafterfiltering = userListModels.Count;
                userListModels = userListModels.Skip(start).Take(length).ToList();
                userListModels = userListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = userListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
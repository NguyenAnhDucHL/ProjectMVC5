
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
        //[HttpPost]
        //public ActionResult GetAllUser()
        //{
        //    int start = Convert.ToInt32(Request["start"]);
        //    int length = Convert.ToInt32(Request["length"]);
        //    string searchValue = Request["search[value]"];
        //    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
        //    string sortDirection = Request["order[0][dir]"];

        //    using (StudyOnlineEntities db = new StudyOnlineEntities())
        //    {
        //        string sql = "select u.[user_id], u.[user_name], u.[user_email], u.[user_mobile], u.[user_gender], u.[user_status], r.roll_name " +
        //                    "from[user] u join[user_roll] ur " +
        //                    "on u.[user_id] = ur.id_user " +
        //                    "join roll r " +
        //                    "on r.roll_id = ur.id_roll";

        //        List<UserListModel> userListModels = db.Database.SqlQuery<UserListModel>(sql).ToList();

        //        int totalrows = userListModels.Count;
        //        int totalrowsafterfiltering = userListModels.Count;
        //        userListModels = userListModels.Skip(start).Take(length).ToList();
        //        userListModels = userListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
        //        return Json(new { success = true, data = userListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}
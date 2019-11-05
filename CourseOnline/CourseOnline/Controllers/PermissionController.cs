using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class PermissionController : Controller
    {
        // GET: Permissions
        [Route("PermissionList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/PermissionList.cshtml");
        }


        [HttpPost]
        public ActionResult GetAllPermission()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Permission";

                List<Permission> Permissions = db.Database.SqlQuery<Permission>(sql).ToList();

                int totalrows = Permissions.Count;
                int totalrowsafterfiltering = Permissions.Count;
                Permissions = Permissions.Skip(start).Take(length).ToList();
                Permissions = Permissions.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Permissions, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
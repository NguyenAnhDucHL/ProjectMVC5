using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class PermissionsController : Controller
    {
        // GET: Permissions
        [Route ("CMS/PermissionList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/PermissionsList.cshtml");
        }


        [HttpPost]
        public ActionResult GetAllPermissions()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var permissionList = (from p in db.Permissions
                                select new
                                {
                                    permission_id = p.permission_id,
                                    permission_name = p.permission_name,
                                    permission_link = p.permission_link
                                }).ToList();

                int totalrows = permissionList.Count;
                int totalrowsafterfiltering = permissionList.Count;
                permissionList = permissionList.Skip(start).Take(length).ToList();
                permissionList = permissionList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = permissionList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
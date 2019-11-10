using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class RolePermissionController : Controller
    {
        // GET: RolePermission
        [Route("RolePermissionList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/RolePermissionList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllRolePermission()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select rp.role_permission_id, p.[permission_name] " +
                            "from RolePermission rp join Permission p " +
                            "on rp.permission_id = p.permission_id";

                List<RolePermissionsModel> RolePermissions = db.Database.SqlQuery<RolePermissionsModel>(sql).ToList();

                int totalrows = RolePermissions.Count;
                int totalrowsafterfiltering = RolePermissions.Count;
                RolePermissions = RolePermissions.Skip(start).Take(length).ToList();
                RolePermissions = RolePermissions.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = RolePermissions, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
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
        [Route("CMS/PermissionList")]
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

        [Route("CMS/RolesPermission")]
        public ActionResult RolesPermission()
        {
            return View("/Views/CMS/RolesPermission.cshtml");
        }

        [HttpPost]
        public ActionResult GetRolePermission()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var result = (from r in db.RolePermissions
                              join p in db.Permissions on r.permission_id equals p.permission_id
                              select new
                              {
                                  role_permission_id = r.permission_id,
                                  permission_name = p.permission_name,
                                  role_name = r.role_name
                              }).ToList().Select(rp => new RolePermissionsModel
                              {
                                  role_permission_id = rp.role_permission_id,
                                  permissionName = rp.permission_name,
                                  role_name = rp.role_name,
                              }).ToList();

                int totalrows = result.Count;
                int totalrowsafterfiltering = result.Count;
                result = result.Skip(start).Take(length).ToList();
                result = result.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = result, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}
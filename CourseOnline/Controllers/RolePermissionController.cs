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
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "No Permission");
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
                    return View("/Views/CMS/RolePermissionList.cshtml");
                }
            }
        }
        [HttpPost]
        public ActionResult GetAllRolePermission()
        {
            if (Session["Email"] == null)
            {
                return null;
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "No Permission");
                if (result.Equals("Student"))
                {
                    return null;
                }
                if (result.Equals("Reject"))
                {
                    return null;
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
    }
}
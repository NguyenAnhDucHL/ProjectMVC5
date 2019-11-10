using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Models;

namespace CourseOnline.Controllers
{
    public class RoleMenuController : Controller
    {
        // GET: RoleMenu
        [Route("RoleMenuList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/RoleMenuList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllRoleMenu()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = " select rm.role_menu_id, m.menu_name " +
                                "from RoleMenu rm join Menu m " +
                                "on rm.menu_id = m.menu_id";

                List<RoleMenuModel> RoleMenus = db.Database.SqlQuery<RoleMenuModel>(sql).ToList();

                int totalrows = RoleMenus.Count;
                int totalrowsafterfiltering = RoleMenus.Count;
                RoleMenus = RoleMenus.Skip(start).Take(length).ToList();
                RoleMenus = RoleMenus.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = RoleMenus, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
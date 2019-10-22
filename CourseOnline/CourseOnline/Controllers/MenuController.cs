using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
namespace CourseOnline.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View("~\\Views\\CMS\\MenuList.cshtml");
        }
        //[HttpPost]
        //public ActionResult getAllMenu()
        //{
        //    int start = Convert.ToInt32(Request["start"]);
        //    int length = Convert.ToInt32(Request["length"]);
        //    string searchValue = Request["search[value]"];
        //    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
        //    string sortDirection = Request["order[0][dir]"];

        //    //using (StudyOnlineEntities db = new StudyOnlineEntities())
        //    //{
        //    //    string sql = "select m.menu_id,m.menu_name, m.menu_link, m.menu_order, m.menu_status,m.descriptions " +
        //    //        "from[menu] m";
        //    //    List<menu> menuList = db.Database.SqlQuery<menu>(sql).ToList();

        //    //    int totalrows = menuList.Count;
        //    //    int totalrowsafterfiltering = menuList.Count;
        //    //    menuList = menuList.Skip(start).Take(length).ToList();
        //    //    menuList = menuList.OrderBy(sortColumnName + " " + sortDirection).ToList();
        //    //    return Json(new { success = true, data = menuList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        //    //}
        //}
    }
}
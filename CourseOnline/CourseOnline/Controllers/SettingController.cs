using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        [Route("SettingList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/SettingList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllSetting()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Setting";

                List<Setting> Settings = db.Database.SqlQuery<Setting>(sql).ToList();

                int totalrows = Settings.Count;
                int totalrowsafterfiltering = Settings.Count;
                Settings = Settings.Skip(start).Take(length).ToList();
                Settings = Settings.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Settings, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class SliderController : Controller
    {
        // GET: Slider
        [Route("SliderList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/SliderList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllSlider()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Slider";

                List<Slider> Sliders = db.Database.SqlQuery<Slider>(sql).ToList();

                int totalrows = Sliders.Count;
                int totalrowsafterfiltering = Sliders.Count;
                Sliders = Sliders.Skip(start).Take(length).ToList();
                Sliders = Sliders.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Sliders, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
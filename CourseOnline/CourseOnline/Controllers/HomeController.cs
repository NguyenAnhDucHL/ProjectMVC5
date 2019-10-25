using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult HomePage()
        {
            return View("/Views/CMS/Home.cshtml");
        }
        public ActionResult Index()
        {
            //ViewBag.Link = TempData["ViewBagLink"];
            return View("/Views/Account/Login.cshtml");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
        // GET: Home
    
}
using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {

        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        public ActionResult Home_CMS()
        {
            return View("/Views/CMS/Home.cshtml");
        }
        public ActionResult Home_User()
        {
            //ViewBag.Link = TempData["ViewBagLink"];
            var lstSubject = db.Subjects.Take(5).ToList();
            ViewBag.lstSubject = lstSubject;
            return View("/Views/User/Home.cshtml");
        }

    }
    // GET: Home

}
using MvcPWy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Home_CMS()
        {
            
            ViewBag.UserName = Session["Name"];
            string email = (string)Session["Email"];
            return View("/Views/CMS/Home.cshtml");
        }
        public ActionResult Home_User()
        {
                //ViewBag.Link = TempData["ViewBagLink"];
                return View("/Views/User/Home.cshtml");
        }

    }
        // GET: Home
    
}
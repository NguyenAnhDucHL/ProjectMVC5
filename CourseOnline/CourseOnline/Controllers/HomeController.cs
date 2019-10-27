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
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        public ActionResult HomePage()
        {
            
            ViewBag.UserName = Session["Name"];
            string email = (string)Session["Email"];
            GetPermission(email);
            return View("/Views/CMS/Home.cshtml");
        }
        public ActionResult Index()
        {
            //ViewBag.Link = TempData["ViewBagLink"];
            return View("/Views/Account/Login.cshtml");
        }

        public void GetPermission(string email)
        {
            List<String> Permission = new List<string>();



            Session["Permission"] = Permission;
        }

    }
        // GET: Home
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class MenuDetailController : Controller
    {
        // GET: MenuDetail
        public ActionResult Index()
        {
            return View("/Views/CMS/MenuDetail.cshtml");
        }
    }
}
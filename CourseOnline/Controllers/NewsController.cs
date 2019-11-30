using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Models;
using PagedList;

namespace CourseOnline.Controllers
{
    public class NewsController : Controller
    {
        private STUDYONLINEEntities db = new STUDYONLINEEntities();
        // GET: News
        public ActionResult PostList(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            var lstPost = db.Posts.OrderBy(n => n.post_id).Where(post => post.post_status == "Published").ToPagedList(pageNumber, pageSize);
            ViewBag.lstPost = lstPost;
            return View("/Views/User/Post.cshtml");
        }
        public ActionResult ResourcesList(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            var lstPost = db.Posts.OrderBy(n => n.post_id).Where(post => post.post_status == "Published" && post.post_type == "Resourse").ToPagedList(pageNumber, pageSize);
            ViewBag.lstPost = lstPost;
            ViewData["PostType"] = "Resource Post";
            return View("/Views/User/Post.cshtml");
        }
        public ActionResult GuideList(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            var lstPost = db.Posts.OrderBy(n => n.post_id).Where(post => post.post_status == "Published" && post.post_type == "Guide").ToPagedList(pageNumber, pageSize);
            ViewBag.lstPost = lstPost;
            ViewData["PostType"] = "Guide Post";
            return View("/Views/User/Post.cshtml");
        }
    }
}
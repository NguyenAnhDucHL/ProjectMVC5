using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Data;
using MvcPWy.Models;
using System.Linq.Dynamic;

namespace CourseOnline.Controllers
{
    public class PostController : Controller
    {
        // GET: PostList
        public static int id_;
        public ActionResult Index()
        {
            return View("~\\Views\\CMS\\PostList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllPost()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                //string sql = "select p.[post_id],p.[post_thumbnail], p.[post_name], p.[post_category],p.[post_type],p.[post_brief],p.[post_status], p.[post_content]" +
                //             "from post p";
                var postList = (from p in db.Posts
                                select new
                                {
                                    post_id = p.post_id,
                                    post_thumbnail = p.post_thumbnail,
                                    post_name = p.post_name,
                                    post_category = p.post_category,
                                    post_type = p.post_type,
                                    post_brief_info = p.post_brief_info,
                                    post_status = p.post_status,
                                    post_detail_info = p.post_detail_info,
                                }).ToList();
                
                //List<post> postList = db.Database.SqlQuery<post>(sql).ToList();

                int totalrows = postList.Count;
                int totalrowsafterfiltering = postList.Count;
                postList = postList.Skip(start).Take(length).ToList();
                postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PostDetail(int id)
        {
            id_ = id;
            ViewBag.id = id;
            return View("/Views/CMS/PostDetail.cshtml");
        }

        [HttpPost]
        public ActionResult getDetailPost()
        {
            STUDYONLINEEntities db = new STUDYONLINEEntities();
            
                var detailPost = (from p in db.Posts
                                  where p.post_id == id_
                                  select new
                                  {
                                      post_name = p.post_name
                                  });
                return Json(new { success = true, data = detailPost , draw = Request["draw"] }, JsonRequestBehavior.AllowGet);        
            
        }


        [HttpGet]
        public ActionResult editPost()
        {
            return View();
        }



        [HttpPost]
        public ActionResult editPost(Post post)
        {
            post.post_id = id_;

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                    //db.Entry(post).State = EntityState.Modified;
                    //db.SaveChanges();
                
                return View();
            }
        }
    }
}
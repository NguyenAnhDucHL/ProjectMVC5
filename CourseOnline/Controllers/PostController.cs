using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Drawing;
using System.Data;
using CourseOnline.Global.Setting;
using Newtonsoft.Json.Linq;
using System.Data.Entity;

namespace CourseOnline.Controllers
{
    public class PostController : Controller
    {
        // GET: PostList
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                List<Setting> listType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.POST_TYPE)).ToList();
                ViewBag.postType = listType;

                List<Setting> listCategory = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.SUBJECT_CATEGORY) || s.setting_group_value.Equals(SettingGroup.GUIDE_CATEGORY)).ToList();
                ViewBag.postCategory = listCategory;

                List<Post> listStatus = db.Posts.Where(p => p.post_status != null).ToList();
                ViewBag.postStatus = listStatus;
            }
            return View("~/Views/CMS/Post/PostList.cshtml");
        }

        //filter by type post
        [HttpPost]
        public ActionResult FilterByPostType(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_TYPE)) // filter theo type
                {
                    var postList = (from p in db.Posts
                                    where p.post_type.Equals(type)
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


                    int totalrows = postList.Count;
                    int totalrowsafterfiltering = postList.Count;
                    postList = postList.Skip(start).Take(length).ToList();
                    postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
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


                    int totalrows = postList.Count;
                    int totalrowsafterfiltering = postList.Count;
                    postList = postList.Skip(start).Take(length).ToList();
                    postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        //filter by category
        [HttpPost]
        public ActionResult FilterByPostCategory(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_CATEGORY)) // filter theo category
                {
                    var postList = (from p in db.Posts
                                    where p.post_category.Equals(type)
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


                    int totalrows = postList.Count;
                    int totalrowsafterfiltering = postList.Count;
                    postList = postList.Skip(start).Take(length).ToList();
                    postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
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


                    int totalrows = postList.Count;
                    int totalrowsafterfiltering = postList.Count;
                    postList = postList.Skip(start).Take(length).ToList();
                    postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        //filter by status
        [HttpPost]
        public ActionResult FilterByPostStatus(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_STATUS)) // filter theo status
                {
                    var postList = (from p in db.Posts
                                    where p.post_status.Equals(type)
                                    select new
                                    {
                                        p.post_id,
                                        p.post_thumbnail,
                                        p.post_name,
                                        p.post_category,
                                        p.post_type,
                                        p.post_brief_info,
                                        p.post_status,
                                        p.post_detail_info,
                                    }).ToList();


                    int totalrows = postList.Count;
                    int totalrowsafterfiltering = postList.Count;
                    postList = postList.Skip(start).Take(length).ToList();
                    postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
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


                    int totalrows = postList.Count;
                    int totalrowsafterfiltering = postList.Count;
                    postList = postList.Skip(start).Take(length).ToList();
                    postList = postList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = postList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
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

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Post post = db.Posts.Where(p => p.post_id == id).FirstOrDefault();
                ViewBag.Post = post;

                List<Setting> listSetting = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.POST_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.Setting = listSetting;

                List<Setting> listSettingCategory = db.Settings.Where(s => (s.setting_group_value.Equals(SettingGroup.SUBJECT_CATEGORY) || s.setting_group_value.Equals(SettingGroup.GUIDE_CATEGORY)) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingCategory = listSettingCategory;

                ViewBag.id = id;
                return View("/Views/CMS/Post/PostDetail.cshtml");
            }
        }

        [HttpGet]
        public ActionResult AddPost()
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {

                List<Setting> listSetting = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.POST_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.Setting = listSetting;

                List<Setting> listSettingCategory = db.Settings.Where(s => (s.setting_group_value.Equals(SettingGroup.SUBJECT_CATEGORY) || s.setting_group_value.Equals(SettingGroup.GUIDE_CATEGORY)) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingCategory = listSettingCategory;

                return View("/Views/CMS/Post/AddPost.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitPost(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtpost = JValue.Parse(postJson);
                    int id = edtpost.id;

                    Post p = db.Posts.Where(pp => pp.post_id == id).FirstOrDefault();
                    if (p != null)
                    {
                        p.post_name = edtpost.postName;
                        p.post_brief_info = edtpost.shortDes;
                        p.post_type = edtpost.postType;
                        p.post_category = edtpost.postCategory;
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SubmitAddPost(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtpost = JValue.Parse(postJson);

                    Post p = new Post();
                    p.post_name = edtpost.postName;
                    p.post_brief_info = edtpost.shortDes;
                    p.post_type = edtpost.postType;
                    p.post_category = edtpost.postCategory;
                    db.Posts.Add(p);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult getDetailPost()
        {
            STUDYONLINEEntities db = new STUDYONLINEEntities();

            var detailPost = (from p in db.Posts
                              select new
                              {
                                  post_name = p.post_name
                              });
            return Json(new { success = true, data = detailPost, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult getDetailPost()
        //{
        //    STUDYONLINEEntities db = new STUDYONLINEEntities();

        //    var detailPost = (from p in db.Posts
        //                      select new
        //                      {
        //                          post_name = p.post_name
        //                      });
        //    return Json(new { success = true, data = detailPost, draw = Request["draw"] }, JsonRequestBehavior.AllowGet);

        //}



        [HttpGet]
        public ActionResult editPost()
        {
            return View();
        }



        [HttpPost]
        public ActionResult editPost(Post post)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();

                return View();
            }
        }


    }
}
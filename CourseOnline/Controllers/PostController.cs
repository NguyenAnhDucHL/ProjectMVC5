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

                var listStatus = db.Posts.Select(p => p.post_status).Distinct().ToList();
                ViewBag.postStatus = listStatus;
            }
            return View("~/Views/CMS/Post/PostList.cshtml");
        }

        //filter 
        [HttpPost]
        public ActionResult DoFilter(string filterBy="")
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            dynamic filterByJson = JValue.Parse(filterBy);

            string type = filterByJson.postType;
            string category = filterByJson.postCategory;
            string status = filterByJson.postStatus;

            if (type.Equals(All.ALL_TYPE))
            {
                type = "";
            }
            if (category.Equals(All.ALL_CATEGORY))
            {
                category = "";
            }
            if (status.Equals(All.ALL_STATUS))
            {
                status = "";
            }

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                    var postList = (from p in db.Posts
                                    where p.post_type.Contains(type)
                                    && p.post_category.Contains(category)
                                    && p.post_status.Contains(status)
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
                var postList = (from p in db.Posts
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
        }
        [HttpPost]
        public ActionResult SearchByName(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var postList = (from p in db.Posts
                                where p.post_name.Contains(type)
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
        [ValidateInput(false)]
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

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SubmitPost(string postJson )
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    
                    dynamic edtpost = JValue.Parse(postJson);
                    int id = edtpost.id;
                    Post p = db.Posts.Where(pp => pp.post_id == id).FirstOrDefault();
                    string imageValue = edtpost.postThumbnail;
                    var ava = imageValue.Substring(imageValue.IndexOf(",") + 1);
                    if (ava == "/Assets/dist/img/" + "post" + edtpost.id + ".png")
                    {
                        if (p != null)
                        {
                            p.post_name = edtpost.postName;
                            p.post_thumbnail = edtpost.postThumbnail;
                            p.post_brief_info = edtpost.shortDes;
                            p.post_type = edtpost.postType;
                            p.post_category = edtpost.postCategory;
                            p.post_detail_info = edtpost.postDetailInfo;
                            p.post_status = edtpost.postStatus;

                            db.SaveChanges();
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    } else
                    {
                        var hinhanh = Convert.FromBase64String(ava);
                        string relative_path = "/Assets/dist/img/" + "post" + edtpost.id + ".png";
                        string path = Server.MapPath(relative_path);
                        System.IO.File.WriteAllBytes(path, hinhanh);

                        if (p != null)
                        {
                            p.post_name = edtpost.postName;
                            p.post_thumbnail = relative_path;
                            p.post_brief_info = edtpost.shortDes;
                            p.post_type = edtpost.postType;
                            p.post_category = edtpost.postCategory;
                            p.post_detail_info = edtpost.postDetailInfo;
                            p.post_status = edtpost.postStatus;

                            db.SaveChanges();
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SubmitAddPost(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    Random rnd = new Random();
                    dynamic edtpost = JValue.Parse(postJson);
                    string imageValue = edtpost.postThumbnail;
                    var ava = imageValue.Substring(imageValue.IndexOf(",") + 1);
                    var hinhanh = Convert.FromBase64String(ava);
                    int num = rnd.Next(1000);
                    string relative_path = "/Assets/dist/img/" + "post" + num + ".png";
                    string path = Server.MapPath(relative_path);
                    System.IO.File.WriteAllBytes(path, hinhanh);
                    Post p = new Post();
                    if(p != null)
                    {
                        p.post_name = edtpost.postName;
                        p.post_thumbnail = relative_path;
                        p.post_brief_info = edtpost.shortDes;
                        p.post_type = edtpost.postType;
                        p.post_category = edtpost.postCategory;
                        p.post_detail_info = edtpost.postDetailInfo;
                        p.post_status = edtpost.postStatus;
                        db.Posts.Add(p);
                        db.SaveChanges();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult deletePost(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var post = db.Posts.Where(p => p.post_id == id).FirstOrDefault();
                if (post != null)
                {
                    db.Posts.Remove(post);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
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







    }
}
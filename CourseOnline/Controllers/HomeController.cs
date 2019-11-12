using CourseOnline.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            var lstSubject = db.Subjects.Take(5).Where(n => n.subject_status == "Submitted").ToList();
            ViewBag.lstSubject = lstSubject;

            var lstPost = db.Posts.Take(7).Where(n => n.post_status == "Submitted").ToList();
            ViewBag.lstPost = lstPost;

            var lstTeacher = (from u in db.Users
                              join ur in db.UserRoles.Where(ur => ur.role_id == 2) on u.user_id equals ur.user_id
                              select new UserListModel
                              {
                                  user_fullname = u.user_fullname,
                                  user_position = u.user_position,
                                  user_image = u.user_image,
                                  user_description = u.user_description,
                              }).Take(6).ToList();

            ViewBag.lstTeacher = lstTeacher;
            return View("/Views/User/Home.cshtml");
        }
        [HttpPost]
        public JsonResult GoogleLogin(string email, string name, string gender, string lastname, string location, string picture)
        {
            Session["Name"] = name;
            Session["Email"] = email;
            Session["Picture"] = picture;
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int duplicate = (from Users in db.Users where Users.user_email == email select Users).Count();
                        if (duplicate == 0)
                        {
                            String sql = "insert into [User](user_position,user_fullname,user_email,use_mobile,user_status,user_image) values (@user_position,@user_fullname,@user_email,@use_mobile,@user_status,@user_image)";
                            db.Database.ExecuteSqlCommand(sql,
                                new SqlParameter("user_position", ""),
                                new SqlParameter("user_fullname", name),
                                new SqlParameter("user_email", email),
                                new SqlParameter("use_mobile", ""),
                                new SqlParameter("user_status", ""),
                                 new SqlParameter("user_image", picture)
                                );
                            db.SaveChanges();
                            transaction.Commit();
                        }

                    }
                    catch (Exception e)
                    {
                        
                        transaction.Rollback();
                    }
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckAccount()
        {
            GetPermission(Session["Email"].ToString());
            if (Session["permission"].Equals("Permission 1") || Session["permission"].Equals("Permission 2"))
            {
                return RedirectToAction("Home_CMS", "Home");
            }
            else
            {
                return RedirectToAction("Home_User", "Home");
            }
        }

        public void GetPermission(string email)
        {
            List<String> Permission = new List<string>();
            var checkPermission = (from u in db.Users.Where(x => x.user_email == email)
                                   join ur in db.UserRoles on u.user_id equals ur.user_id
                                   join r in db.Roles on ur.role_id equals r.role_id
                                   join rp in db.RolePermissions on r.role_id equals rp.role_id
                                   join p in db.Permissions on rp.permission_id equals p.permission_id
                                   select p.permission_name);

            Session["permission"] = "";
            foreach (string permissionName in checkPermission)
            {
                if (permissionName.Equals("Permission 1"))
                {
                    Session["permission"] = "Permission 1";
                }
                else if (permissionName.Equals("Permission 2"))
                {
                    Session["permission"] = "Permission 2";
                }
                else
                {
                    Session["permission"] = "Permission 3";
                }
            }
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Home_User", "Home");
        }
        [HttpGet]
        public ActionResult YourAcountInformation()
        {
            string email = Session["Email"].ToString();
            User userInformation = db.Users.Where(u => u.user_email == email).FirstOrDefault();
            ViewBag.userInformation = userInformation;
            return View("/Views/User/AccountInformation.cshtml");
        }
        [HttpPost]
        public ActionResult UpdateUser(string userJson)
        {
          
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic editUser = JValue.Parse(userJson);
                    int id = editUser.userID;
                    User user = db.Users.Where(u => u.user_id == id).FirstOrDefault();
                    if(user!= null)
                    {
                        user.user_fullname = editUser.userName;
                        user.use_mobile = editUser.userMobile;
                        user.user_description = editUser.userDescription;
                        user.user_gender = editUser.userGender;
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

      
    }
}
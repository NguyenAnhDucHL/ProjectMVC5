
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Models;
using Newtonsoft.Json.Linq;

namespace CourseOnline.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View("/Views/CMS/User/UserList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllUser()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select u.[user_id], u.user_fullname, u.[user_email], u.use_mobile, u.[user_gender], u.[user_status], r.role_name  " +
                            "from[User] u join [UserRole] ur " +
                            "on u.[user_id] = ur.user_id " +
                            "join Roles r " +
                            "on r.role_id = ur.role_id";

                List<UserListModel> userListModels = db.Database.SqlQuery<UserListModel>(sql).ToList();

                int totalrows = userListModels.Count;
                int totalrowsafterfiltering = userListModels.Count;
                userListModels = userListModels.Skip(start).Take(length).ToList();
                userListModels = userListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = userListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult UserDetail(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var Roles = db.Roles.Select(r => r.role_name).Distinct().ToList();

                var User = (from u in db.Users.Where(m => m.user_id == id)
                            join ur in db.UserRoles on u.user_id equals ur.user_id
                            join r in db.Roles on ur.role_id equals r.role_id
                            select new UserListModel
                            {
                                user_fullname = u.user_fullname,
                                user_email = u.user_email,
                                use_mobile = u.use_mobile,
                                user_gender = u.user_gender,
                                user_description = u.user_description,
                                user_position = u.user_position,
                                role_name = r.role_name
                            }).FirstOrDefault();

                ViewBag.User = User;
                ViewBag.Roles = Roles;
                ViewBag.id = id;
                return View("/Views/CMS/User/UserDetail.cshtml");
            }
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var Roles = db.Roles.Select(r => r.role_name).Distinct().ToList();
                ViewBag.Roles = Roles;
            }
            return View("/Views/CMS/User/AddUser.cshtml");
        }

        [HttpPost]
        public ActionResult SubmitUser(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editUser = JValue.Parse(postJson);
                    int id = editUser.userID;
                    string roleName = editUser.userRole;
                    User user = db.Users.Where(p => p.user_id == id).FirstOrDefault();
                    UserRole userRole = db.UserRoles.Where(p => p.user_id == id).FirstOrDefault();
                    var idRole = db.Roles.Where(r => r.role_name == roleName).Select(r => r.role_id).FirstOrDefault();
                    if (user != null)
                    {
                        userRole.role_id = Convert.ToInt32(idRole);
                        userRole.user_id = id;
                        user.user_fullname = editUser.userName;
                        user.user_email = editUser.userMail;
                        user.use_mobile = editUser.userMobile;
                        user.user_position = editUser.userPosition;
                        user.user_description = editUser.userDescription;
                        user.check_recieveInformation = editUser.userCheckReceive;
                        user.user_gender = editUser.userGender;
                        temp = editUser.userStatus;
                        if (temp.Equals("Active"))
                        {
                            user.user_status = true;
                        }
                        else
                        {
                            user.user_status = false;
                        }
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
        public ActionResult SubmitAddUser(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editUser = JValue.Parse(postJson);
                    string roleName = editUser.userRole;
                    User user = new User();
                    UserRole userRole = new UserRole();
                    var idRole = db.Roles.Where(r => r.role_name == roleName).Select(r => r.role_id).FirstOrDefault();
                    if (user != null)
                    {
                        user.user_fullname = editUser.userName;
                        user.user_email = editUser.userMail;
                        user.use_mobile = editUser.userMobile;
                        user.user_position = editUser.userPosition;
                        user.user_description = editUser.userDescription;
                        user.check_recieveInformation = editUser.userCheckReceive;
                        user.user_gender = editUser.userGender;
                        temp = editUser.userStatus;
                        if (temp.Equals("Active"))
                        {
                            user.user_status = true;
                        }
                        else
                        {
                            user.user_status = false;
                        }
                        db.Users.Add(user);
                        db.SaveChanges();
                        string useremail = editUser.userMail;
                        userRole.role_id = Convert.ToInt32(idRole);
                        var idUsers = db.Users.Select(r => r.user_id).Max();
                        userRole.user_id = idUsers;                     
                        db.UserRoles.Add(userRole);
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
    }
}
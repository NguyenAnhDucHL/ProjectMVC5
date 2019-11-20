
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Models;
using Newtonsoft.Json.Linq;
using CourseOnline.Global.Setting;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var lstRoles = db.Roles.Select(r => r.role_name).Distinct().ToList();
                ViewBag.lstRoles = lstRoles;

                var lstStatus = db.Users.Select(r => r.user_status).Distinct().ToList();
                ViewBag.lstStatus = lstStatus;
            }
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
                                role_name = r.role_name,
                                user_image = u.user_image,
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
                    string imageValue = editUser.userImage;
                    var ava = imageValue.Substring(imageValue.IndexOf(",")+1);
                    var hinhanh = Convert.FromBase64String(ava);
                    string relative_path = "~/Path/" + "user"+ editUser.userID + ".png";
                    string path = Server.MapPath (relative_path);
                    System.IO.File.WriteAllBytes(path, hinhanh);
                    Session["Picture"] = relative_path;
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
                        user.user_image = relative_path;
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
                    string imageValue = editUser.userImage;
                    var ava = imageValue.Substring(imageValue.IndexOf(",") + 1);
                    var hinhanh = Convert.FromBase64String(ava);
                    string relative_path = "~/Path/" + editUser.userMail + ".png";
                    string path = Server.MapPath(relative_path);
                    System.IO.File.WriteAllBytes(path, hinhanh);
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
                        user.user_image = relative_path;
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

        //filter 
        [HttpPost]
        public ActionResult DoFilter(string filterBy = "")
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            dynamic filterByJson = JValue.Parse(filterBy);

            string role = filterByJson.userRole;
            string status = filterByJson.userStatus;

            if (role.Equals(All.ALL_ROLE))
            {
                role = "";
            }
            if (status.Equals(All.ALL_STATUS))
            {
                status = "";
            }
            else if (status.Equals("Active"))
            {
                status = "True";
            }
            else if (status.Equals("Inactive"))
            {
                status = "False";
            }

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var userList = (from ur in db.UserRoles
                                join u in db.Users on ur.user_id equals u.user_id
                                join r in db.Roles on ur.role_id equals r.role_id
                                where r.role_name.Contains(role)
                                && u.user_status.ToString().Contains(status)
                                   select new UserListModel
                                   {
                                       user_id = u.user_id,
                                       user_fullname = u.user_fullname,
                                       user_email = u.user_email,
                                       use_mobile = u.use_mobile,
                                       user_gender = u.user_gender,
                                       user_status = u.user_status,
                                       role_name = r.role_name
                                   }).ToList();

                int totalrows = userList.Count;
                int totalrowsafterfiltering = userList.Count;
                userList = userList.Skip(start).Take(length).ToList();
                userList = userList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = userList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FilterByUserStatus(string status)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!status.Equals(All.ALL_STATUS)) // filter theo status
                {
                    if (status.Equals(Status.ACTIVE))
                    {
                        string sql = "select u.[user_id], u.user_fullname, u.[user_email], u.use_mobile, u.[user_gender], u.[user_status], r.role_name  " +
                        "from[User] u join [UserRole] ur " +
                        "on u.[user_id] = ur.[user_id]" +
                        "join Roles r " +
                        "on r.role_id = ur.role_id  where u.[user_status] = 1";

                        List<UserListModel> userListModels = db.Database.SqlQuery<UserListModel>(sql).ToList();

                        int totalrows = userListModels.Count;
                        int totalrowsafterfiltering = userListModels.Count;
                        userListModels = userListModels.Skip(start).Take(length).ToList();
                        userListModels = userListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = userListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string sql = "select u.[user_id], u.user_fullname, u.[user_email], u.use_mobile, u.[user_gender], u.[user_status], r.role_name  " +
                        "from[User] u join [UserRole] ur " +
                        "on u.[user_id] = ur.user_id " +
                        "join Roles r " +
                        "on r.role_id = ur.role_id where u.[user_status] = 0 ";

                        List<UserListModel> userListModels = db.Database.SqlQuery<UserListModel>(sql).ToList();

                        int totalrows = userListModels.Count;
                        int totalrowsafterfiltering = userListModels.Count;
                        userListModels = userListModels.Skip(start).Take(length).ToList();
                        userListModels = userListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = userListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
                else // lay ra tat ca
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
        }

        public ActionResult FilterByUserRoles(string roleUser)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!roleUser.Equals(All.ALL_ROLE)) // filter theo status
                {
                    string sql = "select u.[user_id], u.user_fullname, u.[user_email], u.use_mobile, u.[user_gender], u.[user_status], r.role_name " +
                    "from[User] u join [UserRole] ur " +
                    "on u.[user_id] = ur.[user_id]" +
                    "join Roles r " +
                    "on r.role_id = ur.role_id  where r.[role_name] = @role_name";
                    List<UserListModel> userListModels = db.Database.SqlQuery<UserListModel>(sql, new SqlParameter("role_name", roleUser)).ToList();
                    int totalrows = userListModels.Count;
                    int totalrowsafterfiltering = userListModels.Count;
                    userListModels = userListModels.Skip(start).Take(length).ToList();
                    userListModels = userListModels.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = userListModels, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else
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
        }
        [HttpPost]
        public ActionResult deleteUser(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var user = db.Users.Where(u => u.user_id == id).FirstOrDefault();
                var userrole = db.UserRoles.Where(u => u.user_id == id).FirstOrDefault();

                if (user != null)
                {
                    db.UserRoles.Remove(userrole);
                    db.Users.Remove(user);
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
}
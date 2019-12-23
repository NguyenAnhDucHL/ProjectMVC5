using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using CourseOnline.Global.Setting;
namespace CourseOnline.Controllers
{
    public class PermissionsController : Controller
    {
        // GET: Permissions
        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return View("/Views/Error_404.cshtml");
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "No Permission");
                if (result.Equals("Student"))
                {
                    return View("/Views/Error_404.cshtml");
                }
                if (result.Equals("Reject"))
                {
                    return RedirectToAction("Home_CMS", "Home");
                }
                else
                {
                    return View("/Views/CMS/Permission/PermissionsList.cshtml");
                }
            }
        }


        [HttpPost]
        public ActionResult GetAllPermissions()
        {
            if (Session["Email"] == null)
            {
                return null;
            }
            else
            {
                VerifyAccController verifyAccController = new VerifyAccController();
                String result = verifyAccController.Menu(Session["Email"].ToString(), "No Permission");
                if (result.Equals("Student"))
                {
                    return null;
                }
                if (result.Equals("Reject"))
                {
                    return null;
                }
                else
                {
                    int start = Convert.ToInt32(Request["start"]);
                    int length = Convert.ToInt32(Request["length"]);
                    string searchValue = Request["search[value]"];
                    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                    string sortDirection = Request["order[0][dir]"];

                    using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                    {
                        var permissionList = (from p in db.Permissions
                                              select new
                                              {
                                                  permission_id = p.permission_id,
                                                  permission_name = p.permission_name,
                                                  permission_link = p.permission_link,
                                                  permission_status = p.permission_status
                                              }).ToList();

                        int totalrows = permissionList.Count;
                        int totalrowsafterfiltering = permissionList.Count;
                        permissionList = permissionList.Skip(start).Take(length).ToList();
                        permissionList = permissionList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = permissionList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
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
                var permissionList = (from p in db.Permissions
                                   where p.permission_name.Contains(type)
                                   select new
                                   {
                                       p.permission_id,
                                       p.permission_name,
                                       p.permission_link,
                                       p.permission_status,
                                       p.permission_describe,
                                   }).ToList();
                int totalrows = permissionList.Count;
                int totalrowsafterfiltering = permissionList.Count;
                permissionList = permissionList.Skip(start).Take(length).ToList();
                permissionList = permissionList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = permissionList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PermissionsDetail(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Permission permission = db.Permissions.Where(m => m.permission_id == id).FirstOrDefault();
                ViewBag.Permission = permission;
                ViewBag.id = id;
                return View("/Views/CMS/Permission/PermissionDetail.cshtml");
            }
        }
        [HttpGet]
        public ActionResult AddPermission()
        {
            return View("/Views/CMS/Permission/AddPermission.cshtml");        
        }
        [HttpPost]
        public ActionResult SubmitAddPermission(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addpermission = JValue.Parse(postJson);
                    string temp = null;

                    Permission permission = new Permission();
                    permission.permission_name = addpermission.permissionName;
                    permission.permission_link = addpermission.permissionLink;
                    temp = addpermission.permissionStatus;
                    if (temp.Equals("Active"))
                    {
                        permission.permission_status = true;
                    }
                    else
                    {
                        permission.permission_status = false;
                    }
                    permission.permission_describe = addpermission.permissionDescribe;
                    db.Permissions.Add(permission);
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
        public ActionResult SubmitEditPermission(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editpermission = JValue.Parse(postJson);
                    int id = editpermission.permissionid;

                    Permission permission = db.Permissions.Where(p => p.permission_id == id).FirstOrDefault();
                    if (permission != null)
                    {
                        permission.permission_name = editpermission.permissionName;
                        permission.permission_link = editpermission.permissionLink;
                        temp = editpermission.permissionStatus;
                        if (temp.Equals("Active"))
                        {
                            permission.permission_status = true;
                        }
                        else
                        {
                            permission.permission_status = false;
                        }
                        permission.permission_describe = editpermission.permissionDescribe;
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
        public ActionResult deletePermission(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var permssion = db.Permissions.Where(m => m.permission_id == id).FirstOrDefault();
                if (permssion != null)
                {
                    db.Permissions.Remove(permssion);
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
        public ActionResult FilterByPermissionStatus(string type)
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
                    if (type.Equals(Status.ACTIVE))
                    {
                        var Permissions = (from p in db.Permissions
                                     where p.permission_status == true
                                     select new
                                     {
                                         p.permission_id,
                                         p.permission_name,
                                         p.permission_link,
                                         p.permission_status,
                                         p.permission_describe,
                                     }).ToList();

                        int totalrows = Permissions.Count;
                        int totalrowsafterfiltering = Permissions.Count;
                        Permissions = Permissions.Skip(start).Take(length).ToList();
                        Permissions = Permissions.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Permissions, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var Permissions = (from p in db.Permissions
                                     where p.permission_status == false
                                     select new
                                     {
                                         p.permission_id,
                                         p.permission_name,
                                         p.permission_link,
                                         p.permission_status,
                                         p.permission_describe,
                                     }).ToList();

                        int totalrows = Permissions.Count;
                        int totalrowsafterfiltering = Permissions.Count;
                        Permissions = Permissions.Skip(start).Take(length).ToList();
                        Permissions = Permissions.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Permissions, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
                else // lay ra tat ca
                {
                    string sql = "select * from Permission";

                    List<Permission> Permissions = db.Database.SqlQuery<Permission>(sql).ToList();

                    int totalrows = Permissions.Count;
                    int totalrowsafterfiltering = Permissions.Count;
                    Permissions = Permissions.Skip(start).Take(length).ToList();
                    Permissions = Permissions.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = Permissions, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public ActionResult RolesPermission()
        {
            return View("/Views/CMS/RolesPermission.cshtml");
        }

        [HttpPost]
        public ActionResult GetRolePermission()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var result = (from r in db.RolePermissions
                              join p in db.Permissions on r.permission_id equals p.permission_id
                              select new
                              {
                                  role_permission_id = r.permission_id,
                                  permission_name = p.permission_name,
                                  role_name = r.role_name
                              }).ToList().Select(rp => new RolePermissionsModel
                              {
                                  role_permission_id = rp.role_permission_id,
                                  permission_name = rp.permission_name,
                                  role_name = rp.role_name,
                              }).ToList();

                int totalrows = result.Count;
                int totalrowsafterfiltering = result.Count;
                result = result.Skip(start).Take(length).ToList();
                result = result.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = result, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}
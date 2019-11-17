using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using CourseOnline.Global.Setting;
using Newtonsoft.Json.Linq;

namespace CourseOnline.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        [Route("MenuList")]
        public ActionResult Index()
        {
            return View("/Views/CMS/Menu/MenuList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllMenu()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Menu";

                List<Menu> Menus = db.Database.SqlQuery<Menu>(sql).ToList();

                int totalrows = Menus.Count;
                int totalrowsafterfiltering = Menus.Count;
                Menus = Menus.Skip(start).Take(length).ToList();
                Menus = Menus.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Menus, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        //filter by status
        [HttpPost]
        public ActionResult FilterByMenuStatus(string type)
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
                        var Menus = (from m in db.Menus
                                     where m.menu_status == true
                                     select new
                                     {
                                         m.menu_id,
                                         m.menu_name,
                                         m.menu_link,
                                         m.menu_order,
                                         m.menu_status,
                                         m.menu_description,
                                     }).ToList();

                        int totalrows = Menus.Count;
                        int totalrowsafterfiltering = Menus.Count;
                        Menus = Menus.Skip(start).Take(length).ToList();
                        Menus = Menus.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Menus, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var Menus = (from m in db.Menus
                                     where m.menu_status == false
                                     select new
                                     {
                                         m.menu_id,
                                         m.menu_name,
                                         m.menu_link,
                                         m.menu_order,
                                         m.menu_status,
                                         m.menu_description,
                                     }).ToList();

                        int totalrows = Menus.Count;
                        int totalrowsafterfiltering = Menus.Count;
                        Menus = Menus.Skip(start).Take(length).ToList();
                        Menus = Menus.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = Menus, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                }
                else // lay ra tat ca
                {
                    string sql = "select * from Menu";

                    List<Menu> Menus = db.Database.SqlQuery<Menu>(sql).ToList();

                    int totalrows = Menus.Count;
                    int totalrowsafterfiltering = Menus.Count;
                    Menus = Menus.Skip(start).Take(length).ToList();
                    Menus = Menus.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = Menus, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult deleteMenu(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var menu = db.Menus.Where(m => m.menu_id == id).FirstOrDefault();
                if(menu != null)
                {
                    db.Menus.Remove(menu);
                    db.SaveChanges();
                    return Json(new { success = true}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
               

            }

        }


        [HttpGet]
        public ActionResult AddMenu()
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                return View("/Views/CMS/Menu/AddMenu.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitAddMenu(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addmenu = JValue.Parse(postJson);
                    string temp=null;

                    Menu m = new Menu();
                    m.menu_name = addmenu.menuName;
                    m.menu_link = addmenu.menuLink;
                    m.menu_order = addmenu.menuOrder;
                    temp = addmenu.menuStatus;
                    if (temp.Equals("Active"))
                    {
                        m.menu_status = true;
                    } else {
                        m.menu_status = false;
                    }
                    m.menu_description = addmenu.shortDes;
                    db.Menus.Add(m);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult MenuDetail(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Menu menu = db.Menus.Where(m => m.menu_id == id).FirstOrDefault();
                ViewBag.Menu = menu;
                ViewBag.id = id;
                return View("/Views/CMS/Menu/MenuDetail.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitEditMenu(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editmenu = JValue.Parse(postJson);
                    int id = editmenu.menuid;

                    Menu m = db.Menus.Where(menu => menu.menu_id == id).FirstOrDefault();
                    if (m != null)
                    {
                        m.menu_name = editmenu.menuName;
                        m.menu_link = editmenu.menuLink;
                        m.menu_order = editmenu.menuOrder;
                        temp = editmenu.menuStatus;
                        if (temp.Equals("Active"))
                        {
                            m.menu_status = true;
                        }
                        else
                        {
                            m.menu_status = false;
                        }
                        m.menu_description = editmenu.shortDes;
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
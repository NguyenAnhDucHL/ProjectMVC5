﻿using CourseOnline.Models;
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
    public class SettingController : Controller
    {
        // GET: Setting
        [Route("SettingList")]
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {

                var listGroup = db.Settings.Select(s => s.setting_group_value).Distinct().ToList();
                ViewBag.settingGroup = listGroup;
            }

            return View("/Views/CMS/Setting/SettingList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllSetting()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Setting";

                List<Setting> Settings = db.Database.SqlQuery<Setting>(sql).ToList();

                int totalrows = Settings.Count;
                int totalrowsafterfiltering = Settings.Count;
                Settings = Settings.Skip(start).Take(length).ToList();
                Settings = Settings.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Settings, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddSetting()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listGroup = db.Settings.Select(s => s.setting_group_value).Distinct().ToList();
                ViewBag.settingGroup = listGroup;
                return View("/Views/CMS/Setting/SettingDetail.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SubmitAddSetting(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic addSetting = JValue.Parse(postJson);

                    Setting s = new Setting();
                    s.setting_group_value = addSetting.settingGroup;
                    s.setting_name = addSetting.settingName;
                    s.setting_order = addSetting.settingOrder;
                    s.setting_description = addSetting.settingDescription;
                    s.setting_status = false;
                    db.Settings.Add(s);
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
        public ActionResult SubmitSetting(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtSetting = JValue.Parse(postJson);
                    int id = edtSetting.id;
                    string temp = null;

                    Setting s = db.Settings.Where(ss => ss.setting_id == id).FirstOrDefault();
                    if (s != null)
                    {
                        s.setting_group_value = edtSetting.settingGroup;
                        s.setting_name = edtSetting.settingName;
                        s.setting_order = edtSetting.settingOrder;
                        s.setting_description = edtSetting.settingDescription;
                        temp = edtSetting.settingStatus;
                        if (temp.Equals("Active"))
                        {
                            s.setting_status = true;
                        }
                        else
                        {
                            s.setting_status = false;
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

        [HttpGet]
        public ActionResult EditSetting(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Setting setting = db.Settings.Where(s => s.setting_id == id).FirstOrDefault();
                ViewBag.Setting = setting;

                var listGroup = db.Settings.Select(s => s.setting_group_value).Distinct().ToList();
                ViewBag.settingGroup = listGroup;
                ViewBag.id = id;
                return View("/Views/CMS/Setting/SettingEdit.cshtml");
            }
        }

        //filter by status
        [HttpPost]
        public ActionResult FilterBySettingStatus(string type)
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
                        var settingList = (from s in db.Settings
                                           where s.setting_status == true
                                           select new
                                           {
                                               setting_id = s.setting_id,
                                               setting_group_value = s.setting_group_value,
                                               setting_name = s.setting_name,
                                               setting_order = s.setting_order,
                                               setting_description = s.setting_description,
                                               setting_status = s.setting_status
                                           }).ToList();

                        int totalrows = settingList.Count;
                        int totalrowsafterfiltering = settingList.Count;
                        settingList = settingList.Skip(start).Take(length).ToList();
                        settingList = settingList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = settingList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var settingList = (from s in db.Settings
                                           where s.setting_status == false
                                           select new
                                           {
                                               setting_id = s.setting_id,
                                               setting_group_value = s.setting_group_value,
                                               setting_name = s.setting_name,
                                               setting_order = s.setting_order,
                                               setting_description = s.setting_description,
                                               setting_status = s.setting_status
                                           }).ToList();

                        int totalrows = settingList.Count;
                        int totalrowsafterfiltering = settingList.Count;
                        settingList = settingList.Skip(start).Take(length).ToList();
                        settingList = settingList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                        return Json(new { success = true, data = settingList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                    }

                }
                else // lay ra tat ca
                {
                    var settingList = (from s in db.Settings
                                       select new
                                       {
                                           setting_id = s.setting_id,
                                           setting_group_value = s.setting_group_value,
                                           setting_name = s.setting_name,
                                           setting_order = s.setting_order,
                                           setting_description = s.setting_description,
                                           setting_status = s.setting_status
                                       }).ToList();


                    int totalrows = settingList.Count;
                    int totalrowsafterfiltering = settingList.Count;
                    settingList = settingList.Skip(start).Take(length).ToList();
                    settingList = settingList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = settingList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult FilterBySettingGroup(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_TYPE)) // filter theo group
                {
                    var settingList = (from s in db.Settings
                                       where s.setting_group_value.Equals(type)
                                       select new
                                       {
                                           setting_id = s.setting_id,
                                           setting_group_value = s.setting_group_value,
                                           setting_name = s.setting_name,
                                           setting_order = s.setting_order,
                                           setting_description = s.setting_description,
                                           setting_status = s.setting_status
                                       }).ToList();


                    int totalrows = settingList.Count;
                    int totalrowsafterfiltering = settingList.Count;
                    settingList = settingList.Skip(start).Take(length).ToList();
                    settingList = settingList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = settingList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else // lay ra tat ca
                {
                    var settingList = (from s in db.Settings
                                       select new
                                       {
                                           setting_id = s.setting_id,
                                           setting_group_value = s.setting_group_value,
                                           setting_name = s.setting_name,
                                           setting_order = s.setting_order,
                                           setting_description = s.setting_description,
                                           setting_status = s.setting_status
                                       }).ToList();


                    int totalrows = settingList.Count;
                    int totalrowsafterfiltering = settingList.Count;
                    settingList = settingList.Skip(start).Take(length).ToList();
                    settingList = settingList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = settingList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        //dell setting by id
        [HttpPost]
        public ActionResult delSetting(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var setting = db.Settings.Where(s => s.setting_id == id).FirstOrDefault();
                if (setting != null)
                {
                    db.Settings.Remove(setting);
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
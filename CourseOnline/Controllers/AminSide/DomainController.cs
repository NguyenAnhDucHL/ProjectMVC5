using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using CourseOnline.Global.Setting;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class DomainController : Controller
    {
        // GET: Domain
        [Route("DomainList")]
        public ActionResult Index(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.id = id;
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;
                return View("/Views/CMS/Subject/DomainList.cshtml");
            }
        }

        [HttpPost]
        public ActionResult GetAllDomain(int id = -1)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {

                string sql = "select * from Domain where domain.subject_id = @id";

                List<Domain> Domains = db.Database.SqlQuery<Domain>(sql, new SqlParameter("id", id)).ToList();
                
                int totalrows = Domains.Count;
                int totalrowsafterfiltering = Domains.Count;
                Domains = Domains.Skip(start).Take(length).ToList();
                Domains = Domains.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Domains, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        //search
        [HttpPost]
        public ActionResult SearchByName(string type, int subject_id)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var domainList = (from d in db.Domains
                                  where d.domain_name.Contains(type) & d.subject_id == subject_id
                                  select new
                                  {
                                      d.domain_id,
                                      d.subject_id,
                                      d.domain_name,
                                      d.domain_status,
                                  }).ToList();
                int totalrows = domainList.Count;
                int totalrowsafterfiltering = domainList.Count;
                domainList = domainList.Skip(start).Take(length).ToList();
                domainList = domainList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = domainList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddDomain(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.SubjectId = id;

                return View("/Views/CMS/Subject/DomainAdding.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitAddDomain(string postJson)
        {

            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic adddomain = JValue.Parse(postJson);
                    string temp = null;

                    Domain d = new Domain();
                    d.subject_id = adddomain.subjectId;
                    d.domain_name = adddomain.domainName;
                    d.domain_description = adddomain.domainDes;
                    temp = adddomain.domainStatus;
                    if (temp.Equals("Active"))
                    {
                        d.domain_status = true;
                    }
                    else
                    {
                        d.domain_status = false;
                    }
                    db.Domains.Add(d);
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
        public ActionResult BackDomainList(int id)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                ViewBag.id = id;
                Subject subject = db.Subjects.Where(s => s.subject_id == id).FirstOrDefault();
                ViewBag.Subject = subject;
                List<Setting> listSettingType = db.Settings.Where(s => s.setting_group_value.Equals(SettingGroup.SUBJECT_TYPE) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingType = listSettingType;

                List<Setting> listSettingCategory = db.Settings.Where(s => (s.setting_group_value.Equals(SettingGroup.SUBJECT_CATEGORY) || s.setting_group_value.Equals(SettingGroup.GUIDE_CATEGORY)) && s.setting_status == SettingStatus.ACTIVE).ToList();
                ViewBag.SettingCategory = listSettingCategory;
                return View("/Views/CMS/Subject/DomainList.cshtml");
            }
        }

        [HttpGet]
        public ActionResult EditDomain(int domainId, int subjectId)
        {

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Domain domain = db.Domains.Where(d => d.domain_id == domainId).FirstOrDefault();
                ViewBag.Domain = domain;
                ViewBag.Domain_id = domainId;
                ViewBag.Subject_id = subjectId;
                return View("/Views/CMS/Subject/DomainEditting.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitEditDomain(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    string temp = null;
                    dynamic editdomain = JValue.Parse(postJson);
                    int domain_id = editdomain.domainid;

                    Domain d = db.Domains.Where(domain => domain.domain_id == domain_id).FirstOrDefault();
                    if (d != null)
                    {
                        d.subject_id = editdomain.subjectid;
                        d.domain_name = editdomain.domainName;
                        temp = editdomain.domainStatus;
                        if (temp.Equals("Active"))
                        {
                            d.domain_status = true;
                        }
                        else
                        {
                            d.domain_status = false;
                        }
                        d.domain_description = editdomain.domainDes;
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
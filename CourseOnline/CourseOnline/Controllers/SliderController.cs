using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using CourseOnline.Global.Setting;
using System.Linq.Expressions;

namespace CourseOnline.Controllers
{
    public class SliderController : Controller
    {
        // GET: Slider
        [Route("SliderList")]
        public ActionResult Index()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var listStatus = db.Sliders.Select(s => s.slider_status).Distinct().ToList();
                ViewBag.sliderStatus = listStatus;

            }

            return View("/Views/CMS/Slider/SliderList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllSlider()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select * from Slider";

                List<Slider> Sliders = db.Database.SqlQuery<Slider>(sql).ToList();

                int totalrows = Sliders.Count;
                int totalrowsafterfiltering = Sliders.Count;
                Sliders = Sliders.Skip(start).Take(length).ToList();
                Sliders = Sliders.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { success = true, data = Sliders, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddSlider()
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                return View("/Views/CMS/Slider/SliderDetail.cshtml");

            }
        }
        [HttpPost]
        public ActionResult SubmitAddSlider(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {

                    dynamic addSlider = JValue.Parse(postJson);
                    Slider s = new Slider();
                    s.slider_title = addSlider.sliderTitle;
                    s.slider_caption = addSlider.sliderCaption;
                    s.slider_back_link = addSlider.sliderLink;
                    s.slider_status = addSlider.sliderStatus;
                    db.Sliders.Add(s);
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
        public ActionResult FilterBySliderStatus(string type)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                if (!type.Equals(All.ALL_STATUS))
                {
                    var sliderList = (from s in db.Sliders
                                      where s.slider_status.Equals(type)
                                      select new
                                      {
                                          slider_id = s.slider_id,
                                          slider_picture_url = s.slider_picture_url,
                                          slider_title = s.slider_title,
                                          slider_back_link = s.slider_back_link,
                                          slider_caption = s.slider_caption,
                                          slider_status = s.slider_status
                                      }).ToList();

                    int totalrows = sliderList.Count;
                    int totalrowsafterfiltering = sliderList.Count;
                    sliderList = sliderList.Skip(start).Take(length).ToList();
                    sliderList = sliderList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = sliderList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var sliderList = (from s in db.Sliders
                                      select new
                                      {
                                          slider_id = s.slider_id,
                                          slider_picture_url = s.slider_picture_url,
                                          slider_title = s.slider_title,
                                          slider_back_link = s.slider_back_link,
                                          slider_caption = s.slider_caption,
                                          slider_status = s.slider_status
                                      }).ToList();

                    int totalrows = sliderList.Count;
                    int totalrowsafterfiltering = sliderList.Count;
                    sliderList = sliderList.Skip(start).Take(length).ToList();
                    sliderList = sliderList.OrderBy(sortColumnName + " " + sortDirection).ToList();
                    return Json(new { success = true, data = sliderList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpGet]
        public ActionResult EditSlider(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                Slider slider = db.Sliders.Where(s => s.slider_id == id).FirstOrDefault();
                ViewBag.Slider = slider;

                ViewBag.id = id;
                return View("/Views/CMS/Slider/SliderEdit.cshtml");
            }
        }
        [HttpPost]
        public ActionResult SubmitEditSlider(string postJson)
        {
            try
            {
                using (STUDYONLINEEntities db = new STUDYONLINEEntities())
                {
                    dynamic edtSlider = JValue.Parse(postJson);
                    int id = edtSlider.id;

                    Slider s = db.Sliders.Where(ss => ss.slider_id == id).FirstOrDefault();
                    if (s != null)
                    {
                        s.slider_title = edtSlider.sliderTitle;
                        s.slider_caption = edtSlider.sliderCaption;
                        s.slider_back_link = edtSlider.sliderLink;
                        s.slider_status = edtSlider.sliderStatus;
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
        public ActionResult delSlider(int id)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                var slider = db.Sliders.Where(s => s.slider_id == id).FirstOrDefault();
                if (slider != null)
                {
                    db.Sliders.Remove(slider);
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
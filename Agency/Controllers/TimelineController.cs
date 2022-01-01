using CRM.ViewModel;
using CRM.Auth;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Helpers;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class TimelineController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Timeline
        public ActionResult Index()
        {

            if (Request.IsAjaxRequest())
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                //var from_date = Request.Form.GetValues("columns[0][search][value]")[0];
                //var to_date = Request.Form.GetValues("columns[1][search][value]")[0];
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                // Getting all data    
                var TimelineData = (from Timeline in db.Timelines
                                    select new TimelineViewModel
                                  {
                                      id = Timeline.id,
                                      name = Timeline.name,
                                      description = Timeline.description,
                                      active = Timeline.active,
                                      created_at = Timeline.created_at,
                                      created_at_string = Timeline.created_at.ToString()
                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    TimelineData = TimelineData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = TimelineData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = TimelineData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }

            return View();
        }
        [HttpPost]
        public JsonResult saveTimeline(TimelineViewModel TimelineVM)
        {

            if (TimelineVM.id == 0)
            {
                Timeline Timeline = AutoMapper.Mapper.Map<TimelineViewModel, Timeline>(TimelineVM);

                Timeline.created_at = DateTime.Now;
                //Timeline.created_by = Session["id"].ToString().ToInt();

                db.Timelines.Add(Timeline);
                db.SaveChanges();
            }
            else
            {

                Timeline oldTimeline = db.Timelines.Find(TimelineVM.id);

                oldTimeline.name = TimelineVM.name;
                //oldTimeline.updated_by = Session["id"].ToString().ToInt();
                oldTimeline.updated_at = DateTime.Now;

                db.Entry(oldTimeline).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteTimeline(int id)
        {
            Timeline deleteTimeline = db.Timelines.Find(id);
            db.Timelines.Remove(deleteTimeline);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}
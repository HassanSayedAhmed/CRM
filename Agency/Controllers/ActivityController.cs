using CRM.ViewModel;
using CRM.Auth;
using CRM.Models;
using CRM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class ActivityController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Activity
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
                var activityData = (from activity in db.Activities
                                select new ActivityViewModel
                                {
                                    id = activity.id,
                                    //activity_type = activity.activity_type,
                                    //activity_date_time = activity.activity_date_time,
                                    //activity_duration = activity.activity_duration,
                                    //note = activity.note,
                                    active = activity.active,
                                });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    activityData = activityData.Where(m => m.note.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = activityData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = activityData.Count();

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
    }
}
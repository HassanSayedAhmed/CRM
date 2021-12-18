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
using CRM.Helpers;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class StatusController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Status
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
                var statusData = (from status in db.LeadStages
                                  select new StatusViewModel
                                  {
                                      id = status.id,
                                      name = status.name,
                                      description = status.description,
                                      active = status.active,
                                      created_at = status.created_at,
                                      created_at_string = status.created_at.ToString()
                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    statusData = statusData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = statusData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = statusData.Count();

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
        public JsonResult saveStatus(StatusViewModel statusVM)
        {

            if (statusVM.id == 0)
            {
                LeadStage status = AutoMapper.Mapper.Map<StatusViewModel, LeadStage>(statusVM);

                status.created_at = DateTime.Now;
                status.created_by = Session["id"].ToString().ToInt();

                db.LeadStages.Add(status);
                db.SaveChanges();
            }
            else
            {

                LeadStage oldStatus = db.LeadStages.Find(statusVM.id);

                oldStatus.name = statusVM.name;
                oldStatus.updated_by = Session["id"].ToString().ToInt();
                oldStatus.updated_at = DateTime.Now;

                db.Entry(oldStatus).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteStatus(int id)
        {
            LeadStage deleteStatus = db.LeadStages.Find(id);
            db.LeadStages.Remove(deleteStatus);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}
using CRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Helpers;
using CRM.Auth;
using CRM.ViewModel;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class LeadStageController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: LeadStage
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
                var LeadStageData = (from LeadStage in db.LeadStages
                                  select new LeadStageViewModel
                                  {
                                      id = LeadStage.id,
                                      name = LeadStage.name,
                                      description = LeadStage.description,
                                      active = LeadStage.active,
                                      created_at_string = LeadStage.created_at.ToString()
                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    LeadStageData = LeadStageData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = LeadStageData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = LeadStageData.Count();

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
        public JsonResult saveLeadStage(LeadStageViewModel LeadStageVM)
        {

            if (LeadStageVM.id == 0)
            {
                LeadStage LeadStage = AutoMapper.Mapper.Map<LeadStageViewModel, LeadStage>(LeadStageVM);

                LeadStage.created_at = DateTime.Now;
                LeadStage.created_by = Session["id"].ToString().ToInt();

                db.LeadStages.Add(LeadStage);
                db.SaveChanges();
            }
            else
            {

                LeadStage oldLeadStage = db.LeadStages.Find(LeadStageVM.id);

                oldLeadStage.name = LeadStageVM.name;
                oldLeadStage.updated_by = Session["id"].ToString().ToInt();
                oldLeadStage.updated_at = DateTime.Now;

                db.Entry(oldLeadStage).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteLeadStage(int id)
        {
            LeadStage LeadStage = db.LeadStages.Find(id);
            db.LeadStages.Remove(LeadStage);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
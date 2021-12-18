using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Helpers;
using CRM.Models;
using CRM.ViewModel;

namespace CRM.Controllers
{
    //[CustomAuthenticationFilter]
    public class LeadCategoryController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: LeadCategory
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
                var LeadCategoryData = (from LeadCategory in db.LeadCategories
                                        select new LeadCategoryViewModel
                                  {
                                      id = LeadCategory.id,
                                      name = LeadCategory.name,
                                      description = LeadCategory.description,
                                      active = LeadCategory.active,
                                      created_at_string = LeadCategory.created_at.ToString()

                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    LeadCategoryData = LeadCategoryData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = LeadCategoryData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = LeadCategoryData.Count();

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
        public JsonResult saveLeadCategory(LeadCategoryViewModel LeadCategoryVM)
        {

            if (LeadCategoryVM.id == 0)
            {
                LeadCategory LeadCategory = AutoMapper.Mapper.Map<LeadCategoryViewModel, LeadCategory>(LeadCategoryVM);

                LeadCategory.created_at = DateTime.Now;
                //LeadCategory.created_by = Session["id"].ToString().ToInt();

                db.LeadCategories.Add(LeadCategory);
                db.SaveChanges();
            }
            else
            {

                LeadCategory oldLeadCategory = db.LeadCategories.Find(LeadCategoryVM.id);

                oldLeadCategory.name = LeadCategoryVM.name;
                //oldLeadCategory.updated_by = Session["id"].ToString().ToInt();
                oldLeadCategory.updated_at = DateTime.Now;

                db.Entry(oldLeadCategory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteLeadCategory(int id)
        {
            LeadCategory LeadCategory = db.LeadCategories.Find(id);
            db.LeadCategories.Remove(LeadCategory);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
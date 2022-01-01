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
    public class RequirementController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Requirement
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
                var RequirementData = (from Requirement in db.Requirements
                                  select new RequirementViewModel
                                  {
                                      id = Requirement.id,
                                      name = Requirement.name,
                                      description = Requirement.description,
                                      active = Requirement.active,
                                      created_at_string = Requirement.created_at.ToString()

                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    RequirementData = RequirementData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = RequirementData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = RequirementData.Count();

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
        public JsonResult saveRequirement(RequirementViewModel RequirementVM)
        {

            if (RequirementVM.id == 0)
            {
                Requirement Requirement = AutoMapper.Mapper.Map<RequirementViewModel, Requirement>(RequirementVM);

                Requirement.created_at = DateTime.Now;
                //Requirement.created_by = Session["id"].ToString().ToInt();

                db.Requirements.Add(Requirement);
                db.SaveChanges();
            }
            else
            {

                Requirement oldRequirement = db.Requirements.Find(RequirementVM.id);

                oldRequirement.name = RequirementVM.name;
                //oldRequirement.updated_by = Session["id"].ToString().ToInt();
                oldRequirement.updated_at = DateTime.Now;

                db.Entry(oldRequirement).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteRequirement(int id)
        {
            Requirement Requirement = db.Requirements.Find(id);
            db.Requirements.Remove(Requirement);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
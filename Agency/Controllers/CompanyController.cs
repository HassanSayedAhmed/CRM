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
    //[CustomAuthenticationFilter]
    public class CompanyController : Controller
    {
        CRMDbContext db = new CRMDbContext();
        // GET: LeadType
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
                var companyData = (from company in db.Companies
                                select new CompanyViewModel
                                {
                                    id = company.id,
                                    name = company.name,
                                    description = company.description,
                                    active = company.active,
                                    created_at = company.created_at,
                                    created_at_string = company.created_at.ToString()
                                });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    companyData = companyData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = companyData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = companyData.Count();

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
        public JsonResult saveCompany(CompanyViewModel companyVM)
        {

            if (companyVM.id == 0)
            {
                Company company = AutoMapper.Mapper.Map<CompanyViewModel, Company>(companyVM);

                company.created_at = DateTime.Now;
                //company.created_by = Session["id"].ToString().ToInt();

                db.Companies.Add(company);
                db.SaveChanges();
            }
            else
            {

                Company oldCompany = db.Companies.Find(companyVM.id);

                oldCompany.name = companyVM.name;
                //oldCompany.updated_by = Session["id"].ToString().ToInt();
                oldCompany.updated_at = DateTime.Now;

                db.Entry(oldCompany).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteCompany(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}
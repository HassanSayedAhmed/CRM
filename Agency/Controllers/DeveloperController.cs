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
    public class DeveloperController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Developer
        public ActionResult Index()
        {
            int companyId = Session["companyID"].ToString().ToInt();
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
                var DeveloperData = (from Developer in db.Developers
                                     join user in db.Users on Developer.created_by equals user.id
                                     select new DeveloperViewModel
                                  {
                                      id = Developer.id,
                                      name = Developer.name,
                                      description = Developer.description,
                                      active = Developer.active,
                                      created_at_string = Developer.created_at.ToString(),
                                      company_id = user.company_id

                                     }).Where(d=>d.company_id == companyId);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    DeveloperData = DeveloperData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = DeveloperData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = DeveloperData.Count();

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
        public JsonResult saveDeveloper(DeveloperViewModel DeveloperVM)
        {

            if (DeveloperVM.id == 0)
            {
                Developer DeveloperData = AutoMapper.Mapper.Map<DeveloperViewModel, Developer>(DeveloperVM);

                DeveloperData.created_at = DateTime.Now;
                DeveloperData.created_by = Session["id"].ToString().ToInt();

                db.Developers.Add(DeveloperData);
                db.SaveChanges();
            }
            else
            {

                Developer oldDeveloper = db.Developers.Find(DeveloperVM.id);

                oldDeveloper.name = DeveloperVM.name;
                oldDeveloper.updated_by = Session["id"].ToString().ToInt();
                oldDeveloper.updated_at = DateTime.Now;

                db.Entry(oldDeveloper).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteDeveloper(int id)
        {
            Developer Developer = db.Developers.Find(id);
            db.Developers.Remove(Developer);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
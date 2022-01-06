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
    public class EmploymentTypeController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: EmploymentType
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
                var EmploymentTypeData = (from EmploymentType in db.EmploymentTypes
                                          join user in db.Users on EmploymentType.created_by equals user.id
                                          select new EmploymentTypeViewModel
                                          {
                                              id = EmploymentType.id,
                                              name = EmploymentType.name,
                                              description = EmploymentType.description,
                                              active = EmploymentType.active,
                                              created_at_string = EmploymentType.created_at.ToString(),
                                              company_id = user.company_id

                                          }).Where(et=>et.company_id == companyId);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    EmploymentTypeData = EmploymentTypeData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = EmploymentTypeData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = EmploymentTypeData.Count();

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
        public JsonResult saveEmploymentType(EmploymentTypeViewModel EmploymentTypeVM)
        {

            if (EmploymentTypeVM.id == 0)
            {
                EmploymentType EmploymentType = AutoMapper.Mapper.Map<EmploymentTypeViewModel, EmploymentType>(EmploymentTypeVM);

                EmploymentType.created_at = DateTime.Now;
                EmploymentType.created_by = Session["id"].ToString().ToInt();

                db.EmploymentTypes.Add(EmploymentType);
                db.SaveChanges();
            }
            else
            {

                EmploymentType oldEmploymentType = db.EmploymentTypes.Find(EmploymentTypeVM.id);

                oldEmploymentType.name = EmploymentTypeVM.name;
                oldEmploymentType.updated_by = Session["id"].ToString().ToInt();
                oldEmploymentType.updated_at = DateTime.Now;

                db.Entry(oldEmploymentType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteEmploymentType(int id)
        {
            EmploymentType EmploymentType = db.EmploymentTypes.Find(id);
            db.EmploymentTypes.Remove(EmploymentType);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
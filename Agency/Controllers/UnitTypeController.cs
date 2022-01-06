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
    public class UnitTypeController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: UnitType
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
                var UnitTypeData = (from UnitType in db.UnitTypes
                                    join user in db.Users on UnitType.created_by equals user.id
                                    select new UnitTypeViewModel
                                    {
                                        id = UnitType.id,
                                        name = UnitType.name,
                                        description = UnitType.description,
                                        active = UnitType.active,
                                        created_at_string = UnitType.created_at.ToString(),
                                        company_id = user.company_id
                                    }).Where(ut => ut.company_id == companyId);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    UnitTypeData = UnitTypeData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = UnitTypeData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = UnitTypeData.Count();

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
        public JsonResult saveUnitType(UnitTypeViewModel UnitTypeVM)
        {

            if (UnitTypeVM.id == 0)
            {
                UnitType UnitType = AutoMapper.Mapper.Map<UnitTypeViewModel, UnitType>(UnitTypeVM);

                UnitType.created_at = DateTime.Now;
                UnitType.created_by = Session["id"].ToString().ToInt();

                db.UnitTypes.Add(UnitType);
                db.SaveChanges();
            }
            else
            {

                UnitType oldUnitType = db.UnitTypes.Find(UnitTypeVM.id);

                oldUnitType.name = UnitTypeVM.name;
                oldUnitType.updated_by = Session["id"].ToString().ToInt();
                oldUnitType.updated_at = DateTime.Now;

                db.Entry(oldUnitType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteUnitType(int id)
        {
            UnitType UnitType = db.UnitTypes.Find(id);
            db.UnitTypes.Remove(UnitType);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
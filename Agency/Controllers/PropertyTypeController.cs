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
    public class PropertyTypeController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: PropertyType
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
                var PropertyTypeData = (from PropertyType in db.PropertyTypes
                                  select new PropertyTypeViewModel
                                  {
                                      id = PropertyType.id,
                                      name = PropertyType.name,
                                      description = PropertyType.description,
                                      active = PropertyType.active,
                                      created_at_string = PropertyType.created_at.ToString()

                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    PropertyTypeData = PropertyTypeData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = PropertyTypeData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = PropertyTypeData.Count();

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
        public JsonResult savePropertyType(PropertyTypeViewModel PropertyTypeVM)
        {

            if (PropertyTypeVM.id == 0)
            {
                PropertyType PropertyType = AutoMapper.Mapper.Map<PropertyTypeViewModel, PropertyType>(PropertyTypeVM);

                PropertyType.created_at = DateTime.Now;
                //PropertyType.created_by = Session["id"].ToString().ToInt();

                db.PropertyTypes.Add(PropertyType);
                db.SaveChanges();
            }
            else
            {

                PropertyType oldPropertyType = db.PropertyTypes.Find(PropertyTypeVM.id);

                oldPropertyType.name = PropertyTypeVM.name;
                //oldPropertyType.updated_by = Session["id"].ToString().ToInt();
                oldPropertyType.updated_at = DateTime.Now;

                db.Entry(oldPropertyType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deletePropertyType(int id)
        {
            PropertyType PropertyType = db.PropertyTypes.Find(id);
            db.PropertyTypes.Remove(PropertyType);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
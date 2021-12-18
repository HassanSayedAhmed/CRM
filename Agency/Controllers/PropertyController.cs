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
    public class PropertyController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Property
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
                var PropertyData = (from Property in db.Properties
                                    join pro in db.Developers on Property.project_id equals pro.id into po
                                    from project in po.DefaultIfEmpty()
                                    select new PropertyViewModel
                                    {
                                      id = Property.id,
                                      name = Property.name,
                                      description = Property.description,
                                      price = Property.price,
                                      area = Property.area,
                                      beds = Property.beds,
                                      rooms = Property.rooms,
                                      type = Property.type,
                                      status = Property.status,
                                      image = Property.image,
                                      active = Property.active,
                                      created_at_string = Property.created_at.ToString(),
                                      project_id = Property.project_id,
                                      project_name = project.name,
                                      baths = Property.baths

                                    });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    PropertyData = PropertyData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = PropertyData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = PropertyData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }
            ViewBag.projects = db.Projects.Select(c => new { c.id, c.name }).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult saveProperty(PropertyViewModel PropertyVM)
        {

            if (PropertyVM.id == 0)
            {
                Property Property = AutoMapper.Mapper.Map<PropertyViewModel, Property>(PropertyVM);

                Property.created_at = DateTime.Now;
                //Property.created_by = Session["id"].ToString().ToInt();

                db.Properties.Add(Property);
                db.SaveChanges();
            }
            else
            {

                Property oldProperty = db.Properties.Find(PropertyVM.id);

                oldProperty.name = PropertyVM.name;
                oldProperty.project_id = PropertyVM.project_id;
                oldProperty.description = PropertyVM.description;
                oldProperty.price = PropertyVM.price;
                oldProperty.area = PropertyVM.area;
                oldProperty.beds = PropertyVM.beds;
                oldProperty.rooms = PropertyVM.rooms;
                oldProperty.baths = PropertyVM.baths;
                oldProperty.type = PropertyVM.type;
                oldProperty.status = PropertyVM.status;

                //oldProperty.updated_by = Session["id"].ToString().ToInt();
                oldProperty.updated_at = DateTime.Now;

                db.Entry(oldProperty).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteProperty(int id)
        {
            Property Property = db.Properties.Find(id);
            db.Properties.Remove(Property);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
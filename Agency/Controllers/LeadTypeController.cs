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
    public class LeadTypeController : Controller
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
                var typeData = (from type in db.TypeOfVisitors
                                select new LeadTypeViewModel
                                {
                                    id = type.id,
                                    name = type.name,
                                    description = type.description,
                                    active = type.active,
                                    created_at = type.created_at,
                                    created_at_string = type.created_at.ToString()
                                });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    typeData = typeData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = typeData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = typeData.Count();

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
        public JsonResult saveType(LeadTypeViewModel typeVM)
        {

            if (typeVM.id == 0)
            {
                TypeOfVisitor type = AutoMapper.Mapper.Map<LeadTypeViewModel, TypeOfVisitor>(typeVM);

                type.created_at = DateTime.Now;
                type.created_by = Session["id"].ToString().ToInt();

                db.TypeOfVisitors.Add(type);
                db.SaveChanges();
            }
            else
            {

                TypeOfVisitor oldType = db.TypeOfVisitors.Find(typeVM.id);

                oldType.name = typeVM.name;
                oldType.updated_by = Session["id"].ToString().ToInt();
                oldType.updated_at = DateTime.Now;

                db.Entry(oldType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteType(int id)
        {
            TypeOfVisitor deleteTypes = db.TypeOfVisitors.Find(id);
            db.TypeOfVisitors.Remove(deleteTypes);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}
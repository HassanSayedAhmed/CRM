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
    public class SourceController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Source
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
                var sourceData = (from source in db.Sources
                                select new SourceViewModel
                                {
                                    id = source.id,
                                    name = source.name,
                                    description = source.description,
                                    link = source.link,
                                    active = source.active,
                                    created_at_string = source.created_at.ToString()

                                });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    sourceData = sourceData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = sourceData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = sourceData.Count();

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
        public JsonResult saveSource(SourceViewModel SourceVM)
        {

            if (SourceVM.id == 0)
            {
                Source Source = AutoMapper.Mapper.Map<SourceViewModel, Source>(SourceVM);

                Source.created_at = DateTime.Now;
                //Source.created_by = Session["id"].ToString().ToInt();

                db.Sources.Add(Source);
                db.SaveChanges();
            }
            else
            {

                Source oldSource = db.Sources.Find(SourceVM.id);

                oldSource.name = SourceVM.name;
                //oldSource.updated_by = Session["id"].ToString().ToInt();
                oldSource.updated_at = DateTime.Now;

                db.Entry(oldSource).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteSource(int id)
        {
            Source Source = db.Sources.Find(id);
            db.Sources.Remove(Source);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
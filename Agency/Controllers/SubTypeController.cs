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
    public class SubTypeController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: SubType
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
                var SubTypeData = (from SubType in db.SubTypes
                                  select new SubTypeViewModel
                                  {
                                      id = SubType.id,
                                      name = SubType.name,
                                      description = SubType.description,
                                      active = SubType.active,
                                      created_at_string = SubType.created_at.ToString()

                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    SubTypeData = SubTypeData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = SubTypeData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = SubTypeData.Count();

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
        public JsonResult saveSubType(SubTypeViewModel SubTypeVM)
        {

            if (SubTypeVM.id == 0)
            {
                SubType SubType = AutoMapper.Mapper.Map<SubTypeViewModel, SubType>(SubTypeVM);

                SubType.created_at = DateTime.Now;
                //SubType.created_by = Session["id"].ToString().ToInt();

                db.SubTypes.Add(SubType);
                db.SaveChanges();
            }
            else
            {

                SubType oldSubType = db.SubTypes.Find(SubTypeVM.id);

                oldSubType.name = SubTypeVM.name;
                //oldSubType.updated_by = Session["id"].ToString().ToInt();
                oldSubType.updated_at = DateTime.Now;

                db.Entry(oldSubType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteSubType(int id)
        {
            SubType SubType = db.SubTypes.Find(id);
            db.SubTypes.Remove(SubType);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
using CRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Helpers;
using CRM.ViewModel;
using CRM.Auth;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class TypeOfVisitorController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: TypeOfVisitor
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
                var TypeOfVisitorData = (from TypeOfVisitor in db.TypeOfVisitors
                                         join user in db.Users on TypeOfVisitor.created_by equals user.id
                                         select new TypeOfVisitorViewModel
                                         {
                                             id = TypeOfVisitor.id,
                                             name = TypeOfVisitor.name,
                                             description = TypeOfVisitor.description,
                                             active = TypeOfVisitor.active,
                                             created_at_string = TypeOfVisitor.created_at.ToString(),
                                             company_id = user.company_id
                                         }).Where(tov=>tov.company_id == companyId);

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    TypeOfVisitorData = TypeOfVisitorData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = TypeOfVisitorData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = TypeOfVisitorData.Count();

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
        public JsonResult saveTypeOfVisitor(TypeOfVisitorViewModel TypeOfVisitorVM)
        {

            if (TypeOfVisitorVM.id == 0)
            {
                TypeOfVisitor TypeOfVisitor = AutoMapper.Mapper.Map<TypeOfVisitorViewModel, TypeOfVisitor>(TypeOfVisitorVM);

                TypeOfVisitor.created_at = DateTime.Now;
                TypeOfVisitor.created_by = Session["id"].ToString().ToInt();

                db.TypeOfVisitors.Add(TypeOfVisitor);
                db.SaveChanges();
            }
            else
            {

                TypeOfVisitor oldTypeOfVisitor = db.TypeOfVisitors.Find(TypeOfVisitorVM.id);

                oldTypeOfVisitor.name = TypeOfVisitorVM.name;
                oldTypeOfVisitor.updated_by = Session["id"].ToString().ToInt();
                oldTypeOfVisitor.updated_at = DateTime.Now;

                db.Entry(oldTypeOfVisitor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteTypeOfVisitor(int id)
        {
            TypeOfVisitor TypeOfVisitor = db.TypeOfVisitors.Find(id);
            db.TypeOfVisitors.Remove(TypeOfVisitor);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
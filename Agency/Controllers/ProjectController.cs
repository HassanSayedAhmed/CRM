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
    public class ProjectController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Project
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
                var ProjectData = (from Project in db.Projects
                                   join dev in db.Developers on Project.developer_id equals dev.id into de
                                   from developer in de.DefaultIfEmpty()
                                  select new ProjectViewModel
                                  {
                                      id = Project.id,
                                      name = Project.name,
                                      description = Project.description,
                                      active = Project.active,
                                      created_at_string = Project.created_at.ToString(),
                                      developer_id = Project.developer_id,
                                      developer_name = developer.name
                                  });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    ProjectData = ProjectData.Where(m => m.name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.description.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = ProjectData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = ProjectData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }
            ViewBag.developers = db.Developers.Select(c => new { c.id, c.name }).ToList();

            return View();
        }

        [HttpPost]
        public JsonResult saveProject(ProjectViewModel ProjectVM)
        {

            if (ProjectVM.id == 0)
            {
                Project Project = AutoMapper.Mapper.Map<ProjectViewModel, Project>(ProjectVM);

                Project.created_at = DateTime.Now;
                //Project.created_by = Session["id"].ToString().ToInt();

                db.Projects.Add(Project);
                db.SaveChanges();
            }
            else
            {

                Project oldProject = db.Projects.Find(ProjectVM.id);

                oldProject.name = ProjectVM.name;
                oldProject.developer_id = ProjectVM.developer_id;
                //oldProject.updated_by = Session["id"].ToString().ToInt();
                oldProject.updated_at = DateTime.Now;

                db.Entry(oldProject).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult deleteProject(int id)
        {
            Project Project = db.Projects.Find(id);
            db.Projects.Remove(Project);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

    }
}
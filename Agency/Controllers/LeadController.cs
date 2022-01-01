using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Auth;
using CRM.Helpers;
using CRM.Models;
using CRM.ViewModel;
using Newtonsoft.Json;
namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class LeadController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Lead
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
                var LeadData = (from lead in db.Leads
                                join leadS in db.LeadStages on lead.lead_stage_id equals leadS.id into ls
                                from leadStage in ls.DefaultIfEmpty()
                                join leadC in db.LeadCategories on lead.lead_category_id equals leadC.id into lc
                                from Lead in lc.DefaultIfEmpty()
                                join sou in db.Sources on lead.source_id equals sou.id into s
                                from source in s.DefaultIfEmpty()
                                join tl in db.Timelines on lead.timeline_id equals tl.id into t
                                from timeline in t.DefaultIfEmpty()
                                join empTy in db.EmploymentTypes on lead.employment_type_id equals empTy.id into empTy
                                from employeeType in empTy.DefaultIfEmpty()
                                join com in db.Companies on lead.company_id equals com.id into c
                                from company in c.DefaultIfEmpty()

                                join crBy in db.Users on lead.created_by equals crBy.id into cr
                                from createdBy in cr.DefaultIfEmpty()
                                join upBy in db.Users on lead.updated_by equals upBy.id into up
                                from updatedBy in up.DefaultIfEmpty()
                                join de in db.Users on lead.deleted_by equals de.id into d
                                from deletedBy in d.DefaultIfEmpty()
                                select new LeadViewModel
                                {
                                    id = lead.id,
                                    first_name = lead.first_name,
                                    last_name = lead.last_name,
                                    email = lead.email,
                                    phone1 = lead.phone1,
                                    phone2 = lead.phone2,
                                    alternative_numbers = lead.alternative_numbers,
                                    type_of_visitor_id = lead.type_of_visitor_id,
                                    lead_stage_id = lead.lead_stage_id,
                                    source_id = lead.source_id,
                                    lead_category_id = lead.lead_category_id,
                                    date_of_birth = lead.date_of_birth,
                                    date_of_anniversary = lead.date_of_anniversary,
                                    sales_agent = lead.sales_agent,
                                    address = lead.address,
                                    country = lead.country,
                                    property_type_id = lead.property_type_id,
                                    requirement_id = lead.requirement_id,
                                    budget_min = lead.budget_min,
                                    budget_max = lead.budget_max,
                                    minimum_area = lead.minimum_area,
                                    maximum_area = lead.maximum_area,
                                    area_metric = lead.area_metric,
                                    remark = lead.remark,
                                    street_address = lead.street_address,
                                    location = lead.location,
                                    sub_location = lead.sub_location,
                                    state = lead.state,
                                    pincode = lead.pincode,
                                    location_country = lead.location_country,
                                    latitude = lead.latitude,
                                    longitude = lead.longitude,
                                    timeline_id = lead.timeline_id,
                                    employment_type_id = lead.employment_type_id,
                                    income = lead.income,
                                    designation = lead.designation,
                                    company_id = lead.company_id,
                                    active = lead.active,
                                    created_by = lead.created_by,
                                    updated_by = lead.updated_by,
                                    deleted_by = lead.deleted_by,
                                    created_at_string = lead.created_at.ToString(),

                                    lead_stage_name = leadStage.name,
                                    lead_category_name = Lead.name,
                                    source_name = source.name,
                                    time_line_name = timeline.name,
                                    employment_type_name = employeeType.name,
                                    company_name = company.name,
                                    created_by_name = createdBy.first_name,
                                    updated_by_name = updatedBy.first_name,
                                    deleted_by_name = deletedBy.first_name,

                                    leadActivities = (from leadActivity in db.LeadActivities
                                                      join activity in db.Activities on leadActivity.activity_id equals activity.id
                                                      select new LeadActivityViewModel
                                                      {
                                                          id = leadActivity.id,
                                                          lead_id = leadActivity.lead_id,
                                                          activity_id = leadActivity.activity_id,
                                                          activity_date_time = leadActivity.activity_date_time,
                                                          activity_duration = leadActivity.activity_duration,
                                                          note = leadActivity.note,
                                                          activity_name = activity.name,
                                                      }).ToList(),

                                    leadDevelopers = (from leadDeveloper in db.LeadDevelopers
                                                      join developer in db.Developers on leadDeveloper.developer_id equals developer.id
                                                      select new LeadDeveloperViewModel
                                                      {
                                                          id = leadDeveloper.id,
                                                          developer_id = leadDeveloper.developer_id,
                                                          note = leadDeveloper.note,
                                                          lead_id = leadDeveloper.lead_id,
                                                          developer_name = developer.name,
                                                      }).ToList(),

                                    leadSubTypes = (from leadSubType in db.LeadSubTypes
                                                    join subType in db.SubTypes on leadSubType.sub_type_id equals subType.id
                                                    select new LeadSubTypeViewModel
                                                    {
                                                        id = leadSubType.id,
                                                        description = leadSubType.description,
                                                        name = leadSubType.name,
                                                        lead_id = leadSubType.lead_id,
                                                        sub_type_name = subType.name,
                                                    }).ToList(),

                                    leadProjects = (from leadProjects in db.LeadProjects
                                                    join project in db.Projects on leadProjects.project_id equals project.id
                                                    select new LeadProjectViewModel
                                                    {
                                                        id = leadProjects.id,
                                                        lead_id = leadProjects.lead_id,
                                                        project_name = project.name,
                                                        project_description = project.description,
                                                        project_image = project.image,
                                                    }).ToList(),

                                    leadProperties = (from leadProperties in db.LeadProperties
                                                      join properties in db.Properties on leadProperties.property_id equals properties.id
                                                      select new LeadPropertyViewModel
                                                      {
                                                          id = leadProperties.id,
                                                          lead_id = leadProperties.lead_id,
                                                          note = leadProperties.note,
                                                          property_name = properties.name,
                                                          property_price = properties.price,
                                                          property_baths = properties.baths,
                                                          property_rooms = properties.rooms,
                                                          property_beds = properties.beds,
                                                          property_area = properties.area,
                                                          property_type = properties.type,
                                                          property_status = properties.status,
                                                          property_image = properties.image,
                                                          property_project_id = properties.project_id,
                                                      }).ToList(),

                                    leadUnitTypes = (from leadUnitTypes in db.LeadUnitTypes
                                                     join unitType in db.UnitTypes on leadUnitTypes.unit_type_id equals unitType.id
                                                     select new LeadUnitTypeViewModel
                                                     {
                                                         id = leadUnitTypes.id,
                                                         lead_id = leadUnitTypes.lead_id,
                                                         unit_type_name = unitType.name,
                                                         unit_type_description = unitType.description,
                                                     }).ToList(),


                                });

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    LeadData = LeadData.Where(m => m.first_name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.last_name.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                var displayResult = LeadData.OrderByDescending(u => u.id).Skip(skip)
                     .Take(pageSize).ToList();
                var totalRecords = LeadData.Count();

                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = displayResult

                }, JsonRequestBehavior.AllowGet);

            }

            ViewBag.Users = db.Users.Select(u => new { u.id, u.full_name }).ToList();
            ViewBag.Acitivities = db.Activities.Select(u => new { u.id, u.name }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult manageLead(int? id)
        {

            ViewBag.types_of_visitor = db.TypeOfVisitors.Select(c => new { c.id, c.name }).ToList();
            ViewBag.lead_stages = db.LeadStages.Select(c => new { c.id, c.name }).ToList();
            ViewBag.lead_categories = db.LeadCategories.Select(c => new { c.id, c.name }).ToList();
            ViewBag.sources = db.Sources.Select(c => new { c.id, c.name }).ToList();
            ViewBag.property_types = db.PropertyTypes.Select(c => new { c.id, c.name }).ToList();
            ViewBag.lead_sub_types = db.SubTypes.Select(c => new { c.id, c.name }).ToList();
            ViewBag.lead_unit_types = db.UnitTypes.Select(c => new { c.id, c.name }).ToList();
            ViewBag.requirements = db.Requirements.Select(c => new { c.id, c.name }).ToList();
            ViewBag.projects = db.Projects.Select(c => new { c.id, c.name }).ToList();
            ViewBag.timelines = db.Timelines.Select(c => new { c.id, c.name }).ToList();
            ViewBag.employment_types = db.EmploymentTypes.Select(c => new { c.id, c.name }).ToList();

            LeadViewModel leadData = new LeadViewModel();
            if (id != null)
            {
                leadData = (from lead in db.Leads
                            join leadS in db.LeadStages on lead.lead_stage_id equals leadS.id into ls
                            from leadStage in ls.DefaultIfEmpty()
                            join leadC in db.LeadCategories on lead.lead_category_id equals leadC.id into lc
                            from Lead in lc.DefaultIfEmpty()
                            join sou in db.Sources on lead.source_id equals sou.id into s
                            from source in s.DefaultIfEmpty()
                            join tl in db.Timelines on lead.timeline_id equals tl.id into t
                            from timeline in t.DefaultIfEmpty()
                            join empTy in db.EmploymentTypes on lead.employment_type_id equals empTy.id into empTy
                            from employeeType in empTy.DefaultIfEmpty()
                            join com in db.Companies on lead.company_id equals com.id into c
                            from company in c.DefaultIfEmpty()

                            join crBy in db.Users on lead.created_by equals crBy.id into cr
                            from createdBy in cr.DefaultIfEmpty()
                            join upBy in db.Users on lead.updated_by equals upBy.id into up
                            from updatedBy in up.DefaultIfEmpty()
                            join de in db.Users on lead.deleted_by equals de.id into d
                            from deletedBy in d.DefaultIfEmpty()
                            select new LeadViewModel
                            {
                                id = lead.id,
                                first_name = lead.first_name,
                                last_name = lead.last_name,
                                email = lead.email,
                                phone1 = lead.phone1,
                                phone2 = lead.phone2,
                                alternative_numbers = lead.alternative_numbers,
                                type_of_visitor_id = lead.type_of_visitor_id,
                                lead_stage_id = lead.lead_stage_id,
                                source_id = lead.source_id,
                                lead_category_id = lead.lead_category_id,
                                date_of_birth = lead.date_of_birth,
                                date_of_anniversary = lead.date_of_anniversary,
                                sales_agent = lead.sales_agent,
                                address = lead.address,
                                country = lead.country,
                                property_type_id = lead.property_type_id,
                                requirement_id = lead.requirement_id,
                                budget_min = lead.budget_min,
                                budget_max = lead.budget_max,
                                minimum_area = lead.minimum_area,
                                maximum_area = lead.maximum_area,
                                area_metric = lead.area_metric,
                                remark = lead.remark,
                                street_address = lead.street_address,
                                location = lead.location,
                                sub_location = lead.sub_location,
                                company_name = lead.company_name,
                                state = lead.state,
                                pincode = lead.pincode,
                                location_country = lead.location_country,
                                latitude = lead.latitude,
                                longitude = lead.longitude,
                                timeline_id = lead.timeline_id,
                                employment_type_id = lead.employment_type_id,
                                income = lead.income,
                                designation = lead.designation,
                                company_id = lead.company_id,
                                active = lead.active,
                                created_by = lead.created_by,
                                updated_by = lead.updated_by,
                                deleted_by = lead.deleted_by,
                                created_at_string = lead.created_at.ToString(),
                                lead_stage_name = leadStage.name,
                                lead_category_name = Lead.name,
                                source_name = source.name,
                                time_line_name = timeline.name,
                                employment_type_name = employeeType.name,
                                created_by_name = createdBy.first_name,
                                updated_by_name = updatedBy.first_name,
                                deleted_by_name = deletedBy.first_name,

                                leadActivities = (from leadActivity in db.LeadActivities
                                                  join activity in db.Activities on leadActivity.activity_id equals activity.id
                                                  select new LeadActivityViewModel
                                                  {
                                                      id = activity.id,
                                                      lead_id = leadActivity.lead_id,
                                                      activity_id = leadActivity.activity_id,
                                                      activity_date_time = leadActivity.activity_date_time,
                                                      activity_duration = leadActivity.activity_duration,
                                                      note = leadActivity.note,
                                                      activity_name = activity.name,
                                                  }).Where(la => la.lead_id == lead.id).ToList(),

                                leadDevelopers = (from leadDeveloper in db.LeadDevelopers
                                                  join developer in db.Developers on leadDeveloper.developer_id equals developer.id
                                                  select new LeadDeveloperViewModel
                                                  {
                                                      id = developer.id,
                                                      developer_id = leadDeveloper.developer_id,
                                                      note = leadDeveloper.note,
                                                      lead_id = leadDeveloper.lead_id,
                                                      developer_name = developer.name,
                                                  }).Where(la => la.lead_id == lead.id).ToList(),

                                leadSubTypes = (from leadSubType in db.LeadSubTypes
                                                join subType in db.SubTypes on leadSubType.sub_type_id equals subType.id
                                                select new LeadSubTypeViewModel
                                                {
                                                    id = subType.id,
                                                    description = leadSubType.description,
                                                    name = leadSubType.name,
                                                    lead_id = leadSubType.lead_id,
                                                    sub_type_name = subType.name,
                                                }).Where(la => la.lead_id == lead.id).ToList(),

                                leadProjects = (from leadProjects in db.LeadProjects
                                                join project in db.Projects on leadProjects.project_id equals project.id
                                                select new LeadProjectViewModel
                                                {
                                                    id = project.id,
                                                    lead_id = leadProjects.lead_id,
                                                    project_name = project.name,
                                                    project_description = project.description,
                                                    project_image = project.image,
                                                }).Where(la => la.lead_id == lead.id).ToList(),

                                leadProperties = (from leadProperties in db.LeadProperties
                                                  join properties in db.Properties on leadProperties.property_id equals properties.id
                                                  select new LeadPropertyViewModel
                                                  {
                                                      id = properties.id,
                                                      lead_id = leadProperties.lead_id,
                                                      note = leadProperties.note,
                                                      property_name = properties.name,
                                                      property_price = properties.price,
                                                      property_baths = properties.baths,
                                                      property_rooms = properties.rooms,
                                                      property_beds = properties.beds,
                                                      property_area = properties.area,
                                                      property_type = properties.type,
                                                      property_status = properties.status,
                                                      property_image = properties.image,
                                                      property_project_id = properties.project_id,
                                                  }).Where(la => la.lead_id == lead.id).ToList(),

                                leadUnitTypes = (from leadUnitTypes in db.LeadUnitTypes
                                                 join unitType in db.UnitTypes on leadUnitTypes.unit_type_id equals unitType.id
                                                 select new LeadUnitTypeViewModel
                                                 {
                                                     id = unitType.id,
                                                     lead_id = leadUnitTypes.lead_id,
                                                     unit_type_name = unitType.name,
                                                     unit_type_description = unitType.description,
                                                 }).Where(la => la.lead_id == lead.id).ToList(),


                            }).Where(l => l.id == id).AsEnumerable().Select(l => new LeadViewModel
                            {
                                id = l.id,
                                first_name = l.first_name,
                                last_name = l.last_name,
                                email = l.email,
                                phone1 = l.phone1,
                                phone2 = l.phone2,
                                alternative_numbers = l.alternative_numbers,
                                type_of_visitor_id = l.type_of_visitor_id,
                                lead_stage_id = l.lead_stage_id,
                                source_id = l.source_id,
                                lead_category_id = l.lead_category_id,
                                date_of_birth = l.date_of_birth,
                                date_of_anniversary = l.date_of_anniversary,
                                sales_agent = l.sales_agent,
                                address = l.address,
                                country = l.country,
                                property_type_id = l.property_type_id,
                                requirement_id = l.requirement_id,
                                budget_min = l.budget_min,
                                budget_max = l.budget_max,
                                minimum_area = l.minimum_area,
                                maximum_area = l.maximum_area,
                                area_metric = l.area_metric,
                                remark = l.remark,
                                street_address = l.street_address,
                                location = l.location,
                                sub_location = l.sub_location,
                                state = l.state,
                                pincode = l.pincode,
                                location_country = l.location_country,
                                latitude = l.latitude,
                                longitude = l.longitude,
                                timeline_id = l.timeline_id,
                                employment_type_id = l.employment_type_id,
                                income = l.income,
                                designation = l.designation,
                                company_id = l.company_id,
                                active = l.active,
                                created_by = l.created_by,
                                updated_by = l.updated_by,
                                deleted_by = l.deleted_by,
                                created_at_string = l.created_at.ToString(),
                                lead_stage_name = l.lead_stage_name,
                                lead_category_name = l.lead_category_name,
                                source_name = l.source_name,
                                time_line_name = l.time_line_name,
                                employment_type_name = l.employment_type_name,
                                company_name = l.company_name,
                                created_by_name = l.created_by_name,
                                updated_by_name = l.updated_by_name,
                                deleted_by_name = l.deleted_by_name,

                                leadActivities = l.leadActivities,
                                leadActivitiesJson = JsonConvert.SerializeObject(l.leadActivities),

                                leadDevelopers = l.leadDevelopers,
                                leadDevelopersJson = JsonConvert.SerializeObject(l.leadDevelopers),

                                leadSubTypesJson = JsonConvert.SerializeObject(l.leadSubTypes),
                                leadSubTypes = l.leadSubTypes,

                                leadProjects = l.leadProjects,
                                leadProjectsJson = JsonConvert.SerializeObject(l.leadProjects),

                                leadProperties = l.leadProperties,
                                leadPropertiesJson = JsonConvert.SerializeObject(l.leadProperties),

                                leadUnitTypes = l.leadUnitTypes,
                                leadUnitTypesJson = JsonConvert.SerializeObject(l.leadUnitTypes),

                            }).FirstOrDefault();

            }
            
            return View(leadData);
        }

        [HttpPost]
        public ActionResult saveLead(LeadViewModel LeadVM)
        {

            if (LeadVM.id == 0)
            {
                Lead Lead = AutoMapper.Mapper.Map<LeadViewModel, Lead>(LeadVM);

                Lead.created_at = DateTime.Now;
                Lead.created_by = Session["id"].ToString().ToInt();

                db.Leads.Add(Lead);
                db.SaveChanges();
                if (LeadVM.lead_sub_types != null)
                {
                    foreach (var subType in LeadVM.lead_sub_types)
                    {
                        LeadSubType leadSubType = new LeadSubType();
                        leadSubType.sub_type_id = subType;
                        leadSubType.lead_id = Lead.id;
                        leadSubType.created_at = DateTime.Now;
                        leadSubType.created_by = Session["id"].ToString().ToInt();
                        db.LeadSubTypes.Add(leadSubType);
                    }
                }
                if (LeadVM.lead_unit_types != null)
                {
                    foreach (var unitType in LeadVM.lead_unit_types)
                    {
                        LeadUnitType leadUnitType = new LeadUnitType();
                        leadUnitType.unit_type_id = unitType;
                        leadUnitType.lead_id = Lead.id;
                        leadUnitType.created_at = DateTime.Now;
                        leadUnitType.created_by = Session["id"].ToString().ToInt();
                        db.LeadUnitTypes.Add(leadUnitType);
                    }
                }
                if (LeadVM.project_ids != null)
                {
                    foreach (var project in LeadVM.project_ids)
                    {
                        LeadProject leadProject = new LeadProject();
                        leadProject.project_id = project;
                        leadProject.lead_id = Lead.id;
                        leadProject.created_at = DateTime.Now;
                        leadProject.created_by = Session["id"].ToString().ToInt();
                        db.LeadProjects.Add(leadProject);
                    }
                }
                db.SaveChanges();
            }
            else
            {

                Lead oldLead = db.Leads.Find(LeadVM.id);
                oldLead.first_name = LeadVM.first_name;
                oldLead.last_name = LeadVM.last_name;
                oldLead.email = LeadVM.email;
                oldLead.phone1 = LeadVM.phone1;
                oldLead.phone2 = LeadVM.phone2;
                oldLead.alternative_numbers = LeadVM.alternative_numbers;
                oldLead.type_of_visitor_id = LeadVM.type_of_visitor_id;
                oldLead.lead_stage_id = LeadVM.lead_stage_id;
                oldLead.lead_category_id = LeadVM.lead_stage_id;
                oldLead.source_id = LeadVM.source_id;
                oldLead.date_of_birth = LeadVM.date_of_birth;
                oldLead.date_of_anniversary = LeadVM.date_of_anniversary;
                oldLead.sales_agent = LeadVM.sales_agent;
                oldLead.address = LeadVM.address;
                oldLead.country = LeadVM.country;
                oldLead.property_type_id = LeadVM.property_type_id;
                oldLead.requirement_id = LeadVM.requirement_id;
                oldLead.budget_min = LeadVM.budget_min;
                oldLead.budget_max = LeadVM.budget_max;
                oldLead.minimum_area = LeadVM.minimum_area;
                oldLead.maximum_area = LeadVM.maximum_area;
                oldLead.area_metric = LeadVM.area_metric;
                oldLead.remark = LeadVM.remark;
                oldLead.street_address = LeadVM.street_address;
                oldLead.location = LeadVM.location;
                oldLead.sub_location = LeadVM.sub_location;
                oldLead.state = LeadVM.state;
                oldLead.pincode = LeadVM.pincode;
                oldLead.location_country = LeadVM.location_country;
                oldLead.latitude = LeadVM.latitude;
                oldLead.longitude = LeadVM.longitude;
                oldLead.timeline_id = LeadVM.timeline_id;
                oldLead.employment_type_id = LeadVM.employment_type_id;
                oldLead.income = LeadVM.income;
                oldLead.designation = LeadVM.designation;
                oldLead.company_name = LeadVM.company_name;
                oldLead.company_id = LeadVM.company_id;
                oldLead.company_name = LeadVM.company_name;
                oldLead.updated_at = DateTime.Now;

                db.Entry(oldLead).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                db.LeadSubTypes.Where(lsub => lsub.lead_id == oldLead.id).ToList().ForEach(lsub => db.LeadSubTypes.Remove(lsub));
                db.SaveChanges();

                if (LeadVM.lead_sub_types != null)
                {
                    foreach (var subType in LeadVM.lead_sub_types)
                    {
                        LeadSubType leadSubType = new LeadSubType();
                        leadSubType.sub_type_id = subType;
                        leadSubType.lead_id = oldLead.id;
                        leadSubType.created_at = DateTime.Now;
                        leadSubType.created_by = Session["id"].ToString().ToInt();
                        db.LeadSubTypes.Add(leadSubType);
                    }
                }

                db.LeadUnitTypes.Where(lub => lub.lead_id == oldLead.id).ToList().ForEach(lub => db.LeadUnitTypes.Remove(lub));
                db.SaveChanges();

                if (LeadVM.lead_unit_types != null)
                {
                    foreach (var unitType in LeadVM.lead_unit_types)
                    {
                        LeadUnitType leadUnitType = new LeadUnitType();
                        leadUnitType.unit_type_id = unitType;
                        leadUnitType.lead_id = oldLead.id;
                        leadUnitType.created_at = DateTime.Now;
                        leadUnitType.created_by = Session["id"].ToString().ToInt();
                        db.LeadUnitTypes.Add(leadUnitType);
                    }
                }

                db.LeadProjects.Where(lp => lp.lead_id == oldLead.id).ToList().ForEach(lp => db.LeadProjects.Remove(lp));
                db.SaveChanges();

                if (LeadVM.project_ids != null)
                {
                    foreach (var project in LeadVM.project_ids)
                    {
                        LeadProject leadProject = new LeadProject();
                        leadProject.project_id = project;
                        leadProject.lead_id = oldLead.id;
                        leadProject.created_at = DateTime.Now;
                        leadProject.created_by = Session["id"].ToString().ToInt();
                        db.LeadProjects.Add(leadProject);
                    }
                }

            }
            db.SaveChanges();
            return Redirect(Url.Action("Index", "Lead"));

        }

        [HttpGet]
        public JsonResult deleteLead(int id)
        {
            Lead Lead = db.Leads.Find(id);
            db.Leads.Remove(Lead);
            db.SaveChanges();

            return Json(new { message = "done" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult assignLeads(LeadViewModel leadViewModel)
        {
            string [] leads_ids = leadViewModel.lead_ids.Split(',');
            foreach(string lead_id in leads_ids)
            {
                int id = int.Parse(lead_id);
                Lead currentLead = db.Leads.Find(id);
                currentLead.assigned_user_id = leadViewModel.assigned_user_id;
                db.SaveChanges();
            }
            return Json(new { msg = "done" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult makeActivity(LeadActivityViewModel leadActivityViewModel)
        {
            LeadActivity leadActivity = AutoMapper.Mapper.Map<LeadActivityViewModel, LeadActivity>(leadActivityViewModel);

            leadActivity.created_at = DateTime.Now;
            leadActivity.created_by = Session["id"].ToString().ToInt();

            db.LeadActivities.Add(leadActivity);
            db.SaveChanges();
            return Json(new { msg = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}
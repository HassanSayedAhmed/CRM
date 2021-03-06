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
using Newtonsoft.Json;
using CRM.Enums;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class LeadController : Controller
    {
        CRMDbContext db = new CRMDbContext();

        // GET: Lead
        public ActionResult Index()
        {
            int companyID = ((int)Session["companyID"]);
            if (Request.IsAjaxRequest())
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                var search_type_of_visitor_id = Request.Form.GetValues("columns[0][search][value]")[0];
                var search_lead_stage_id = Request.Form.GetValues("columns[1][search][value]")[0];
                var search_lead_category_id = Request.Form.GetValues("columns[2][search][value]")[0];
                var search_source_id = Request.Form.GetValues("columns[3][search][value]")[0];
                var search_property_type_id = Request.Form.GetValues("columns[4][search][value]")[0];
                var search_requirement_id = Request.Form.GetValues("columns[5][search][value]")[0];
                var search_timeline_id = Request.Form.GetValues("columns[6][search][value]")[0];
                var search_employment_type_id = Request.Form.GetValues("columns[7][search][value]")[0];
                var search_assigned_user_id = Request.Form.GetValues("columns[8][search][value]")[0];
                var search_from_date = Request.Form.GetValues("columns[9][search][value]")[0];
                var search_to_date = Request.Form.GetValues("columns[10][search][value]")[0];
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                

                // Getting all data    
                var LeadData = (from lead in db.Leads
                                join leadS in db.LeadStages on lead.lead_stage_id equals leadS.id into ls
                                from leadStage in ls.DefaultIfEmpty()
                                join propt in db.PropertyTypes on lead.property_type_id equals propt.id into prt
                                from proptype in prt.DefaultIfEmpty()
                                join req in db.Requirements on lead.requirement_id equals req.id into rq
                                from requiremtent in prt.DefaultIfEmpty()
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
                                join asn in db.Users on lead.assigned_user_id equals asn.id into asur
                                from assignedUser in asur.DefaultIfEmpty()
                                join crBy in db.Users on lead.created_by equals crBy.id //into cr
                                //from createdBy in cr.DefaultIfEmpty()
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
                                    property_type_name = proptype.name,
                                    requirement_id = lead.requirement_id,
                                    requirement_name = requiremtent.name,
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
                                    created_at = lead.created_at,
                                    created_at_string = lead.created_at.ToString(),
                                    decision_id = lead.decision_id,
                                    assigned_user_id = lead.assigned_user_id,
                                    deal_property_id =lead.deal_property_id,
                                    deal_property_price = lead.deal_property_price,
                                    deal_make_user_id = lead.deal_make_user_id,
                                    lead_stage_name = leadStage.name,
                                    lead_category_name = Lead.name,
                                    source_name = source.name,
                                    time_line_name = timeline.name,
                                    employment_type_name = employeeType.name,
                                    assigned_user_name = assignedUser.full_name,
                                    company_name = company.name,
                                    created_by_name = crBy.first_name,
                                    created_by_company_id = (int)crBy.company_id,
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
                                                          string_activity_date_time = leadActivity.activity_date_time.ToString(),
                                                          activity_duration = leadActivity.activity_duration,
                                                          note = leadActivity.note,
                                                          activity_name = activity.name,
                                                      }).Where(l=>l.lead_id == lead.id).ToList(),

                                    leadDevelopers = (from leadDeveloper in db.LeadDevelopers
                                                      join developer in db.Developers on leadDeveloper.developer_id equals developer.id
                                                      select new LeadDeveloperViewModel
                                                      {
                                                          id = leadDeveloper.id,
                                                          developer_id = leadDeveloper.developer_id,
                                                          note = leadDeveloper.note,
                                                          lead_id = leadDeveloper.lead_id,
                                                          developer_name = developer.name,
                                                      }).Where(l => l.lead_id == lead.id).ToList(),

                                    leadSubTypes = (from leadSubType in db.LeadSubTypes
                                                    join subType in db.SubTypes on leadSubType.sub_type_id equals subType.id
                                                    select new LeadSubTypeViewModel
                                                    {
                                                        id = leadSubType.id,
                                                        description = leadSubType.description,
                                                        name = leadSubType.name,
                                                        lead_id = leadSubType.lead_id,
                                                        sub_type_name = subType.name,
                                                    }).Where(l => l.lead_id == lead.id).ToList(),

                                    leadProjects = (from leadProjects in db.LeadProjects
                                                    join project in db.Projects on leadProjects.project_id equals project.id
                                                    select new LeadProjectViewModel
                                                    {
                                                        id = leadProjects.id,
                                                        lead_id = leadProjects.lead_id,
                                                        project_name = project.name,
                                                        project_description = project.description,
                                                        project_image = project.image,
                                                    }).Where(l => l.lead_id == lead.id).ToList(),

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
                                                      }).Where(l => l.lead_id == lead.id).ToList(),

                                    leadUnitTypes = (from leadUnitTypes in db.LeadUnitTypes
                                                     join unitType in db.UnitTypes on leadUnitTypes.unit_type_id equals unitType.id
                                                     select new LeadUnitTypeViewModel
                                                     {
                                                         id = leadUnitTypes.id,
                                                         lead_id = leadUnitTypes.lead_id,
                                                         unit_type_name = unitType.name,
                                                         unit_type_description = unitType.description,
                                                     }).Where(l => l.lead_id == lead.id).ToList(),


                                }).Where(u => u.created_by_company_id == companyID);

                if(isA.salesAgent())
                {
                    int userId = Session["id"].ToString().ToInt();
                    LeadData = LeadData.Where(l => l.created_by == userId || l.assigned_user_id == userId);
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    LeadData = LeadData.Where(m => m.first_name.ToLower().Contains(searchValue.ToLower()) || m.id.ToString().ToLower().Contains(searchValue.ToLower()) ||
                     m.last_name.ToLower().Contains(searchValue.ToLower()));
                }

                if (!string.IsNullOrEmpty(search_type_of_visitor_id))
                {
                    int search_type_of_visitor_id_int = int.Parse(search_type_of_visitor_id);
                    LeadData = LeadData.Where(s => s.type_of_visitor_id == search_type_of_visitor_id_int);
                }

                if (!string.IsNullOrEmpty(search_lead_stage_id))
                {
                    int search_lead_stage_id_int = int.Parse(search_lead_stage_id);
                    LeadData = LeadData.Where(s => s.lead_stage_id == search_lead_stage_id_int);
                }

                if (!string.IsNullOrEmpty(search_lead_category_id))
                {
                    int search_lead_category_id_int = int.Parse(search_lead_category_id);
                    LeadData = LeadData.Where(s => s.lead_category_id == search_lead_category_id_int);
                }
                if (!string.IsNullOrEmpty(search_source_id))
                {
                    int search_source_id_int = int.Parse(search_source_id);
                    LeadData = LeadData.Where(s => s.source_id == search_source_id_int);
                }
                if (!string.IsNullOrEmpty(search_property_type_id))
                {
                    int search_property_type_id_int = int.Parse(search_property_type_id);
                    LeadData = LeadData.Where(s => s.property_type_id == search_property_type_id_int);
                }
                if (!string.IsNullOrEmpty(search_requirement_id))
                {
                    int search_requirement_id_int = int.Parse(search_requirement_id);
                    LeadData = LeadData.Where(s => s.requirement_id == search_requirement_id_int);
                }
                if (!string.IsNullOrEmpty(search_timeline_id))
                {
                    int search_timeline_id_int = int.Parse(search_timeline_id);
                    LeadData = LeadData.Where(s => s.timeline_id == search_timeline_id_int);
                }
                if (!string.IsNullOrEmpty(search_employment_type_id))
                {
                    int search_employment_type_id_int = int.Parse(search_employment_type_id);
                    LeadData = LeadData.Where(s => s.employment_type_id == search_employment_type_id_int);
                }
                if (!string.IsNullOrEmpty(search_assigned_user_id))
                {
                    int search_assigned_user_id_int = int.Parse(search_assigned_user_id);
                    LeadData = LeadData.Where(s => s.assigned_user_id == search_assigned_user_id_int);
                }

                if (!string.IsNullOrEmpty(search_from_date))
                {
                    if (Convert.ToDateTime(search_from_date) != DateTime.MinValue)
                    {
                        DateTime from = Convert.ToDateTime(search_from_date);
                        LeadData = LeadData.Where(s => s.created_at >= from);
                    }
                }
                if (!string.IsNullOrEmpty(search_to_date))
                {
                    if (Convert.ToDateTime(search_to_date) != DateTime.MinValue)
                    {
                        DateTime to = Convert.ToDateTime(search_to_date);
                        LeadData = LeadData.Where(s => s.created_at <= to);
                    }
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
            ViewBag.DealUsers = db.Users.Where(u=>u.company_id == companyID).Select(u => new { u.id, u.full_name }).ToList();
            ViewBag.TypeOfVisitors = (from typeofvisitor in db.TypeOfVisitors
                                   join user in db.Users on typeofvisitor.created_by equals user.id
                                   select new TypeOfVisitorViewModel
                                   {
                                       id = typeofvisitor.id,
                                       name = typeofvisitor.name,
                                       company_id = user.company_id

                                   }).Where(a => a.company_id == companyID).ToList();
            ViewBag.LeadCategories = (from leadcategory in db.LeadCategories
                                      join user in db.Users on leadcategory.created_by equals user.id
                                      select new LeadCategoryViewModel
                                      {
                                          id = leadcategory.id,
                                          name = leadcategory.name,
                                          company_id = user.company_id

                                      }).Where(a => a.company_id == companyID).ToList();
            ViewBag.PropertyTypes = (from propertytype in db.PropertyTypes
                                      join user in db.Users on propertytype.created_by equals user.id
                                      select new PropertyTypeViewModel
                                      {
                                          id = propertytype.id,
                                          name = propertytype.name,
                                          company_id = user.company_id

                                      }).Where(a => a.company_id == companyID).ToList();
            ViewBag.Requirements = (from requirement in db.Requirements
                                     join user in db.Users on requirement.created_by equals user.id
                                     select new RequirementViewModel
                                     {
                                         id = requirement.id,
                                         name = requirement.name,
                                         company_id = user.company_id

                                     }).Where(a => a.company_id == companyID).ToList();
            ViewBag.Timelines = (from timeline in db.Timelines
                                    join user in db.Users on timeline.created_by equals user.id
                                    select new TimelineViewModel
                                    {
                                        id = timeline.id,
                                        name = timeline.name,
                                        company_id = user.company_id

                                    }).Where(a => a.company_id == companyID).ToList();
            ViewBag.EmploymentTypes = (from employmenttype in db.EmploymentTypes
                                 join user in db.Users on employmenttype.created_by equals user.id
                                 select new EmploymentTypeViewModel
                                 {
                                     id = employmenttype.id,
                                     name = employmenttype.name,
                                     company_id = user.company_id

                                 }).Where(a => a.company_id == companyID).ToList();

            ViewBag.Acitivities = (from activity in db.Activities
                                   join user in db.Users on activity.created_by equals user.id
                                   select new ActivityViewModel
                                   {
                                       id = activity.id,
                                       name = activity.name,
                                       company_id = user.company_id

                                   }).Where(a => a.company_id == companyID).ToList();
            ViewBag.Properties = (from property in db.Properties
                                  join user in db.Users on property.created_by equals user.id
                                  select new PropertyViewModel
                                  {
                                      id = property.id,
                                      name = property.name,
                                      company_id = user.company_id

                                  }).Where(a => a.company_id == companyID).ToList();

            ViewBag.Sources = (from source in db.Sources
                               join user in db.Users on source.created_by equals user.id
                               select new SourceViewModel
                               {
                                   id = source.id,
                                   name = source.name,
                                   company_id = user.company_id

                               }).Where(a => a.company_id == companyID).ToList();

            List<LeadViewModel> companyLeads = (from lead in db.Leads
                                      join user in db.Users on lead.created_by equals user.id
                                      select new LeadViewModel
                                      {
                                          id = lead.id,
                                          lead_stage_id = lead.lead_stage_id,
                                          company_id = user.company_id

                                      }).Where(a => a.company_id == companyID).ToList();

            ViewBag.newLeadsCounter = companyLeads.Where(l => l.lead_stage_id == (int)LeadStages.New || l.lead_stage_id == null).Count();
            ViewBag.followUpLeadsCounter = companyLeads.Where(l => l.lead_stage_id == (int)LeadStages.FollowUp).Count();
            ViewBag.negotiationLeadsCounter = companyLeads.Where(l => l.lead_stage_id == (int)LeadStages.Negotiation).Count();
            ViewBag.meetingLeadsCounter = companyLeads.Where(l => l.lead_stage_id == (int)LeadStages.Meeting).Count();
            ViewBag.deadLeadsCounter = companyLeads.Where(l => l.lead_stage_id == (int)LeadStages.Deal).Count();

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
            ViewBag.properties = db.Properties.Select(u => new { u.id, u.name }).ToList();
            ViewBag.developers = db.Developers.Select(u => new { u.id, u.name }).ToList();

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
                Lead.lead_stage_id = (int)LeadStages.New;
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

                if (LeadVM.property_ids != null)
                {
                    foreach (var propertyId in LeadVM.property_ids)
                    {
                        LeadProperty leadProperty = new LeadProperty();
                        leadProperty.property_id = propertyId;
                        leadProperty.lead_id = Lead.id;
                        leadProperty.created_at = DateTime.Now;
                        leadProperty.created_by = Session["id"].ToString().ToInt();
                        db.LeadProperties.Add(leadProperty);
                    }
                }

                if (LeadVM.developer_ids != null)
                {
                    foreach (var developerId in LeadVM.developer_ids)
                    {
                        LeadDeveloper leadDeveloper = new LeadDeveloper();
                        leadDeveloper.developer_id = developerId;
                        leadDeveloper.lead_id = Lead.id;
                        leadDeveloper.created_at = DateTime.Now;
                        leadDeveloper.created_by = Session["id"].ToString().ToInt();
                        db.LeadDevelopers.Add(leadDeveloper);
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
                oldLead.lead_category_id = LeadVM.lead_category_id;
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
                oldLead.updated_by = Session["id"].ToString().ToInt();
                oldLead.updated_at = DateTime.Now;

                db.Entry(oldLead).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                db.LeadSubTypes.Where(lsub => lsub.lead_id == LeadVM.id).ToList().ForEach(lsub => db.LeadSubTypes.Remove(lsub));
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

                db.LeadUnitTypes.Where(lub => lub.lead_id == LeadVM.id).ToList().ForEach(lub => db.LeadUnitTypes.Remove(lub));
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

                db.LeadProjects.Where(lp => lp.lead_id == LeadVM.id).ToList().ForEach(lp => db.LeadProjects.Remove(lp));
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

                db.LeadProperties.Where(lp => lp.lead_id == LeadVM.id).ToList().ForEach(lp => db.LeadProperties.Remove(lp));
                if (LeadVM.property_ids != null)
                {
                    foreach (var propertyId in LeadVM.property_ids)
                    {
                        LeadProperty leadProperty = new LeadProperty();
                        leadProperty.property_id = propertyId;
                        leadProperty.lead_id = oldLead.id;
                        leadProperty.created_at = DateTime.Now;
                        leadProperty.created_by = Session["id"].ToString().ToInt();
                        db.LeadProperties.Add(leadProperty);
                    }
                }

                db.LeadDevelopers.Where(lp => lp.lead_id == LeadVM.id).ToList().ForEach(lp => db.LeadDevelopers.Remove(lp));
                if (LeadVM.developer_ids != null)
                {
                    foreach (var developerId in LeadVM.developer_ids)
                    {
                        LeadDeveloper leadDeveloper = new LeadDeveloper();
                        leadDeveloper.developer_id = developerId;
                        leadDeveloper.lead_id = oldLead.id;
                        leadDeveloper.created_at = DateTime.Now;
                        leadDeveloper.created_by = Session["id"].ToString().ToInt();
                        db.LeadDevelopers.Add(leadDeveloper);
                    }
                }
            }
            db.SaveChanges();
            return Redirect(Url.Action("Index", "Lead"));

        }

        public JsonResult saveQuickLead(LeadViewModel LeadVM)
        {

            if (LeadVM.id == 0)
            {
                Lead Lead = AutoMapper.Mapper.Map<LeadViewModel, Lead>(LeadVM);
                Lead.lead_stage_id = (int)LeadStages.New;
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

                if (LeadVM.property_ids != null)
                {
                    foreach (var propertyId in LeadVM.property_ids)
                    {
                        LeadProperty leadProperty = new LeadProperty();
                        leadProperty.property_id = propertyId;
                        leadProperty.lead_id = Lead.id;
                        leadProperty.created_at = DateTime.Now;
                        leadProperty.created_by = Session["id"].ToString().ToInt();
                        db.LeadProperties.Add(leadProperty);
                    }
                }

                db.SaveChanges();
            }
            return Json(new { msg = "done" }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult makeDecision(LeadViewModel leadViewModel)
        {
            Lead lead = db.Leads.Find(leadViewModel.id);
            lead.decision_id = leadViewModel.decision_id;
            db.SaveChanges();
            return Json(new { msg = "done" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult makeDeal(LeadViewModel leadViewModel)
        {
            Lead lead = db.Leads.Find(leadViewModel.id);
            lead.deal_property_price = leadViewModel.deal_property_price;
            lead.deal_make_user_id = leadViewModel.deal_make_user_id;
            db.SaveChanges();
            return Json(new { msg = "done" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeStage(LeadViewModel leadViewModel)
        {
            Lead lead = db.Leads.Find(leadViewModel.id);
            lead.lead_stage_id = leadViewModel.lead_stage_id;
            db.SaveChanges();
            return Json(new { msg = "done" }, JsonRequestBehavior.AllowGet);
        }
    }
}
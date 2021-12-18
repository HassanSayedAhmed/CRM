using CRM.Models;
using CRM.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CRM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.CreateMap<UserViewModel, User>();
            Mapper.CreateMap<ActivityViewModel, Activity>();
            Mapper.CreateMap<SourceViewModel, Source>();
            Mapper.CreateMap<CompanyViewModel, Company>();
            Mapper.CreateMap<LeadStageViewModel, LeadStage>();
            Mapper.CreateMap<TimelineViewModel, Timeline>();
            Mapper.CreateMap<ChatViewModel, Chat>();
            Mapper.CreateMap<DeveloperViewModel, Developer>();
            Mapper.CreateMap<TypeOfVisitorViewModel, TypeOfVisitor>();
            Mapper.CreateMap<LeadCategoryViewModel, LeadCategory>();
            Mapper.CreateMap<PropertyTypeViewModel, PropertyType>();
            Mapper.CreateMap<RequirementViewModel, Requirement>();
            Mapper.CreateMap<EmploymentTypeViewModel, EmploymentType>();
            Mapper.CreateMap<ProjectViewModel, Project> ();
            

        }
    }
}

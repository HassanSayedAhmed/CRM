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
            Mapper.CreateMap<LeadTypeViewModel, TypeOfVisitor>();
            Mapper.CreateMap<StatusViewModel, LeadStage>();
            Mapper.CreateMap<LeadTypeViewModel, Lead>();
            Mapper.CreateMap<ChatViewModel, Chat>();
            

        }
    }
}

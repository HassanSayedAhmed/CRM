using CRM.Auth;
using CRM.Helpers;
using CRM.Models;
using CRM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [CustomAuthenticationFilter]
    public class DashboardController : Controller
    {
        CRMDbContext db = new CRMDbContext();
        // GET: Dashboard
        public ActionResult Index()
        {
            User currentUser = Session["user"] as User;
            UserViewModel userData = (from user in db.Users
                                      select new UserViewModel
                                      {
                                          id = user.id,
                                          user_name = user.user_name,
                                          full_name = user.full_name,
                                          password = user.password,
                                          type = user.type,
                                          phone1 = user.phone1,
                                          phone2 = user.phone2,
                                          imagePath = user.image,
                                          address1 = user.address1,
                                          address2 = user.address2,
                                          birthDate = user.birthDate,
                                          code = user.code,
                                          email = user.email,
                                          gender = user.gender,
                                          active = user.active,
                                          company_id = user.company_id
                                      }).Where(u => u.id == currentUser.id).FirstOrDefault();
            
            return View(userData);
        }

      

    }
}
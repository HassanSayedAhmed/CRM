using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
namespace CRM.Controllers
{
    public class AccountController : Controller
    {
        CRMDbContext db = new CRMDbContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            User currentUser = db.Users.Where(s => s.user_name == user.user_name && s.password == user.password).FirstOrDefault();
            if (currentUser != null)
            {
                Session["user_name"] = currentUser.user_name;
                Session["type"] = currentUser.type;
                Session["id"] = currentUser.id;
                Session["companyID"] = currentUser.company_id;
                Session["user"] = currentUser;

                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
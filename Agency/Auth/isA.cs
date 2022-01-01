using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Helpers;
using CRM.Enums;
namespace CRM.Auth
{
    public class isA
    {
        public static bool superAdmin()
        {
            if (Convert.ToInt32(HttpContext.Current.Session["type"].ToString()) == (int)UserRole.SuperAdmin)
                return true;
            return false;
        }
        public static bool admin()
        {
            if (Convert.ToInt32(HttpContext.Current.Session["type"].ToString()) == (int)UserRole.Admin)
                return true;
            return false;
        }
        public static bool salesAgent()
        {
            if (Convert.ToInt32(HttpContext.Current.Session["type"].ToString()) == (int)UserRole.SalesAgent)
                return true;
            return false;
        }
    }
}
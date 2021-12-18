using CRM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Auth
{
    public class can
    {
        public static bool access()
        {
            return true;
            if (HttpContext.Current.Session["user_name"] != null)
                return true;
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using taka.Models.Enitities;
using taka.Utils;

namespace taka
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_Start(object sender, EventArgs erg)
        {

            Application.Lock();
            if (Application["Views"] == null)
                Application["Views"] = 1;
            else
                Application["Views"] = (int)Application["Views"] + 1;
            Application.UnLock();
        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            try { 
            USER user = (USER)Session[C.SESSION.UserInfo];
            if (user != null)
                Session[C.SESSION.Cart] = new Models.DatabaseInteractive.TakaDB().GetListCarts(user.ID).Count;
            }
            catch (Exception) { }
        }


    }

}
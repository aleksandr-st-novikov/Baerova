using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain.Context;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer<mainDbContext>(new DbInitializer());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //все буквы в адресе прописные
            string url = Request.Url.ToString();
            if (Request.HttpMethod == "GET" && Regex.Match(url, "(?<=^[^?]*)[A-Z]").Success)
            {
                Response.RedirectPermanent(url.ToLower(), true);
            }
        }
    }
}

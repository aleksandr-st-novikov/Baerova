using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using Hangfire;
using WebUI.Helpers.Hangfire;
using System.Web;
using WebUI.Controllers;

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

            MvcHandler.DisableMvcResponseHeader = true;

            GlobalConfiguration.Configuration.UseSqlServerStorage("mainDbContext");
            HangfireBootstrapper.Instance.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            HangfireBootstrapper.Instance.Stop();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //все буквы в адресе прописные
            string url = Request.Url.ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                System.Web.HttpContext.Current.Response.Redirect("/");

            if (Request.HttpMethod == "GET" && Regex.Match(url, "(?<=^[^?]*)[A-Z]").Success)
            {
                Response.RedirectPermanent(url.ToLower(), true);
            }
        }

    }
}

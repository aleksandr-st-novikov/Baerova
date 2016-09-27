using Microsoft.Owin;
using Owin;
using Hangfire;
using WebUI.Helpers.Hangfire;
using WebUI.Helpers;

[assembly: OwinStartupAttribute(typeof(WebUI.Startup))]
namespace WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("mainDbContext");
            app.UseHangfireDashboard("/settings/jobs", new DashboardOptions()
            {
                Authorization = new[] { new HangFireAuthorizationFilter() },
                AppPath = "/settings/menusetmanage"
            });

            //RecurringJob.AddOrUpdate("task-id1", () => Services.SendMessage("test","text","novikov.it@bobruysk.korona.by"), "39 15 * * *");
        }
    }
}

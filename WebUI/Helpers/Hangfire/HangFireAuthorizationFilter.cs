using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.Web;

namespace WebUI.Helpers.Hangfire
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.IsInRole("Администратор");
        }
    }
}
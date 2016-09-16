using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class GlobalizationController : Controller
    {
        public ActionResult Globalization(string culture)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture ?? "ru-RU");            return View();
        }
    }
}
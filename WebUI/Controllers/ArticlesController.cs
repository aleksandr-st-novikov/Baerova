using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ArticlesController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditArticle(Guid Id)
        {
            return View();
        }

        public ActionResult ImageBrowser()
        {
            return View();
        }
    }
}
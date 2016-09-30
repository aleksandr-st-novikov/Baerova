using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "articles/article/{link}",
                new { controller = "Articles", action = "Article", link = UrlParameter.Optional }
            );

            routes.MapRoute(null,
                "articles/{page}",
                new { controller = "Articles", action = "index", page = UrlParameter.Optional },
                new { page = @"\d+" }
            );

            //routes.MapRoute(null,
            //    "articles/listarticle/{page}",
            //    new { controller = "Articles", action = "listarticle", page = UrlParameter.Optional },
            //    new { page = @"\d+" }
            //);

            routes.MapRoute(null,
                "{controller}/{action}/{page}",
                new { controller = "home,articles", action = "managepartners,listarticle,subscribers,MainArticles", page = UrlParameter.Optional },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}

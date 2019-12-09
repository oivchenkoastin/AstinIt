using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AstinIt
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute
            (
                name: "Projects",
                url: "{controller}/{id}",
                defaults: new { action = "Project", id = UrlParameter.Optional },
                constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Name",
                url: "{controller}/{action}/{name}",
                defaults: new { controller = "Home", action = "Index", name = UrlParameter.Optional }
            );

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Proiect_DSG
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Groups", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Custom",
                url: "{controller}/{action}/{id}/{name}",
                defaults: new { controller = "Groups", action = "Index", id = UrlParameter.Optional, name=UrlParameter.Optional }
            );


        }
    }
}

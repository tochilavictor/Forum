using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Topic",
                url: "{action}/{id}/topage{page}",
                defaults: new { controller = "Topic" }
            );

            routes.MapRoute(
                name: "Section",
                url: "{action}/{id}/page{page}",
                defaults: new {controller =  "Home"}
            );
            routes.MapRoute(
            name: null,
                url: "{controller}/{action}/{id}/{page}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional,page= UrlParameter.Optional }
            );
        }
    }
}

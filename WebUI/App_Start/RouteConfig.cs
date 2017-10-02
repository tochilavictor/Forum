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
                name: null,
                url: "Message/DisplayAttachedPicture/{id}/{name}/{extension}",
                defaults: new { controller = "Message", action = "DisplayAttachedPicture" }
                );

            routes.MapRoute(
                name: "Topic",
                url: "{action}/{id}/topage{page}",
                defaults: new { controller = "Topic" }
            );

            routes.MapRoute(
                name: "Section",
                url: "{action}/{id}/page{page}",
                defaults: new {controller =  "Section"}
            );

            routes.MapRoute(
                name: "UserProfile",
                url: "User/{Username}",
                defaults: new { controller = "Account", action="Account"}
            );

            routes.MapRoute(
            name: null,
                url: "Message/Create/{id}/{parentMessageId}",
                defaults: new { controller = "Message", action = "Create", parentMessageId = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: null,
                url: "Topic/Create/{sectionid}",
                defaults: new { controller = "Topic",action="Create" }
            );
            routes.MapRoute(
            name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Section", action = "Index", id = UrlParameter.Optional}
            );
            routes.MapRoute(
            name: null,
                url: "{controller}/{action}/{id}/{page}",
                defaults: new { controller = "Section", action = "Index", id = UrlParameter.Optional,page= UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "withRedirect",
                url: "{controller}/{action}/{sourceid}/{page}/{id}"
            );
        }
    }
}

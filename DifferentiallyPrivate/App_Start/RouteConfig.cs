using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DifferentiallyPrivate
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "WhatIs",
                url: "whatis",
                defaults: new { controller = "Home", action = "WhatIs" }
            );

            routes.MapRoute(
                name: "HowTo",
                url: "howto",
                defaults: new { controller = "Home", action = "HowTo" }
            );

            routes.MapRoute(
                name: "Charting",
                url: "charting",
                defaults: new { controller = "Home", action = "Charting" }
            );

            routes.MapRoute(
                name: "ASync",
                url: "async",
                defaults: new { controller = "Home", action = "ASync", v1 = UrlParameter.Optional, v2 = UrlParameter.Optional }
            );

        }
    }
}
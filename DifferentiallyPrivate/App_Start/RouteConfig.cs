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
                name: "Login",
                url: "login",
                defaults: new { controller = "Home", action = "Login" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Home", action = "Logout" }
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
                name: "ChartingSimple",
                url: "charting/simple",
                defaults: new { controller = "Chart", action = "ChartingSimple" }
            );

            routes.MapRoute(
                name: "ASync",
                url: "async",
                defaults: new { controller = "Home", action = "ASync"}
            );

            routes.MapRoute(
                name: "Test",
                url: "async",
                defaults: new { controller = "Home", action = "Test" }
            );

        }
    }
}
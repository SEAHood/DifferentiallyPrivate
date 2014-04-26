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

            //Route for root page
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            //Route for login
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Home", action = "Login" }
            );

            //Route for logout
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Home", action = "Logout" }
            );

            //Route for about page
            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "About" }
            );

            //Route for how to page
            routes.MapRoute(
                name: "HowTo",
                url: "howto",
                defaults: new { controller = "Home", action = "HowTo" }
            );

            //Route for charting page
            routes.MapRoute(
                name: "Charting",
                url: "charting",
                defaults: new { controller = "Home", action = "Charting" }
            );

            //Route for simple charting
            routes.MapRoute(
                name: "ChartingSimple",
                url: "charting/simple",
                defaults: new { controller = "Chart", action = "ChartingSimple" }
            );

            //Route for home charting
            routes.MapRoute(
                name: "ChartingHome",
                url: "charting/home",
                defaults: new { controller = "Chart", action = "ChartingHome" }
            );
        }
    }
}
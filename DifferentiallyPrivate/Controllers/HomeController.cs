using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using System.Drawing;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using System.Threading.Tasks;
using System.Web.Security;

namespace DifferentiallyPrivate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome";
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid())
                {
                    //["loggedin"] = true;
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    //Response.Redirect("/");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login details are incorrect!");
                }
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult WhatIs()
        {
            ViewBag.Message = "What is Differential Privacy?";
            return View();
        }

        [Authorize]
        public ActionResult HowTo()
        {
            ViewBag.Message = "How To";
            return View();
        }

        [Authorize]
        public ActionResult Charting()
        {
            ViewBag.Message = "Charting";

            return View();
        }        
    }
}

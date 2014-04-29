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
    /// <summary>
    /// HomeController - Main controller for the basic areas of the site
    /// Handles routing requests for index, login/logout, and about/howto/charting sections
    /// </summary>
    public class HomeController : Controller
    {
        //Root page
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome";
            return View();
        }

        //GET : Login page, return login view
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //POST : Attempt to login
        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                //Check if user is valid (username + pwd matches DB)
                if (user.IsValid())
                {
                    //Set authentication for user and redirect to home
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else //Invalid user
                {
                    ModelState.AddModelError("", "Login details are incorrect!");
                }
            }
            return View(user);
        }

        //Logout method
        public ActionResult Logout()
        {
            //Sign authentication out, redirect to home
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //About page
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "What is Differential Privacy?";
            return View();
        }

        //How To page
        //[Authorize]
        public ActionResult HowTo()
        {
            ViewBag.Message = "How To";
            return View();
        }

        //Charting page
        //[Authorize]
        public ActionResult Charting()
        {
            ViewBag.Message = "Charting";
            return View();
        }        
    }
}

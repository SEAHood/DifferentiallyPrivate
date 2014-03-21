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

namespace DifferentiallyPrivate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Home!";
            return View();
        }

        public ActionResult WhatIs()
        {
            ViewBag.Message = "What is Differential Privacy?";
            return View();
        }

        public ActionResult HowTo()
        {
            ViewBag.Message = "How To!";
            return View();
        }

        public ActionResult Charting()
        {
            ViewBag.Message = "Charting!";

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
            .InitChart(new Chart 
            {
                DefaultSeriesType = ChartTypes.Column
            })
            .SetXAxis(new DotNet.Highcharts.Options.XAxis
            {
                Categories = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
            })
            .SetSeries(new DotNet.Highcharts.Options.Series
            {
                Data = new DotNet.Highcharts.Helpers.Data(new object[] { 29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 })
            });
            chart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "Simple Chart" });

            return View(chart);
        }

        public ActionResult ASync()
        {
            if (Request.IsAjaxRequest())
            {
                if (Request.Form.Count == 1)
                {
                    DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = ChartTypes.Column
                    })
                    .SetXAxis(new DotNet.Highcharts.Options.XAxis
                    {
                        Categories = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                    })
                    .SetSeries(new DotNet.Highcharts.Options.Series
                    {
                        Data = new DotNet.Highcharts.Helpers.Data(new object[] { 29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 })
                    });
                    chart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "Simple Chart" });

                    return PartialView("_Chart", chart);
                }
                else
                {
                    var c = Int32.Parse(Request.Form["input1"].ToString());
                    var d = Int32.Parse(Request.Form["input2"].ToString());
                    DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                           .InitChart(new Chart
                           {
                               DefaultSeriesType = ChartTypes.Column
                           })
                           .SetXAxis(new DotNet.Highcharts.Options.XAxis
                           {
                               Categories = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
                           })
                           .SetSeries(new DotNet.Highcharts.Options.Series
                           {
                               Data = new DotNet.Highcharts.Helpers.Data(new object[] { c, d, c, d, c, d, c, d, c, d, c, d })
                           });
                    chart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "Simple Chart" });

                    return PartialView("_Chart", chart);
                }
            }
            else
            {
                return View();
            }
        }


        public async Task<ActionResult> ASyncTest()
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            var service = new Models.ASyncService();
            var theTruth = await service.GetTheTruth();

            return View(theTruth);
        }

        
    }
}

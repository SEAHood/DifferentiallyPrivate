using DifferentiallyPrivate.Models;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DifferentiallyPrivate.Controllers
{
    public class ChartController : Controller
    {
        //
        // GET: /Chart/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChartingSimple()
        {
            if (Request.IsAjaxRequest())
            {
                if (Request.Form.Count != 1)
                {
                    double[] iList = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                    int iterations = Int32.Parse(Request.Form["iterations"].ToString()); 
                    double epsilon = Double.Parse(Request.Form["epsilon"].ToString()); 
                    int binCount = Int32.Parse(Request.Form["bins"].ToString()); 

                    ChartModels cm = new ChartModels();

                    DotNet.Highcharts.Highcharts chart = cm.Begin(iList, iterations, epsilon, binCount);
                    
                    return PartialView("_Chart", chart);
                }
                else
                {
                    //User-defined list
                    Thread.Sleep(3000);
                    //var c = Int32.Parse(Request.Form["input1"].ToString());
                    //var d = Int32.Parse(Request.Form["input2"].ToString());
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
                               Data = new DotNet.Highcharts.Helpers.Data(new object[] { 1,2,3,4,5,6,7,8,9,10 })
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

        public ActionResult ChartingUnknown()
        {
            return View();
        }

        public ActionResult ChartingHome()
        {
            return View();
        }

    }
}

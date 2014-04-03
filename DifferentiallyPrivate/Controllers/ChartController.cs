using DifferentiallyPrivate.Models;
using DotNet.Highcharts;
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


        [Authorize]
        [HttpGet]
        public ActionResult ChartingSimple() //RETURN NEW MODEL WITH EMPTY CHART
        {
            MultiChart mc = new MultiChart();
            ChartModel chart = new ChartModel(0);
            mc.allCharts.Add(chart);
            return View(mc);
        }


        [Authorize]
        [HttpPost]
        public ActionResult ChartingSimple(MultiChart mc)
        {
            if (Request.IsAjaxRequest()) //BUILD CHARTS IN ALLCHARTS
            {
                foreach (var chart in mc.allCharts)
                {
                    if (chart.IsValid())
                    {
                        chart.InitChart();
                        chart.FillChart();
                    }
                }

                ChartModel cm = new ChartModel(mc.allCharts.Count);
                mc.allCharts.Add(cm);
                return PartialView("_Chart", mc);
            }
            else //ADD CHART TO ALLCHARTS
            {
                if (mc.allCharts.Last().IsValid()) //Added chart is valid - add new chart to be edited
                {
                    ChartModel cm = new ChartModel(mc.allCharts.Count);
                    mc.allCharts.Add(cm);
                    foreach (var c in mc.allCharts)
                    {
                        c.InitChart();
                    }
                    return View(mc);
                }
                else //Added chart is invalid
                {
                    ModelState.AddModelError("", "Chart values invalid!");
                    mc.allCharts.Remove(mc.allCharts.Last());
                    ChartModel cm = new ChartModel(mc.allCharts.Count - 1);
                    mc.allCharts.Add(cm);
                    return View(mc);
                }
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

using DifferentiallyPrivate.Models;
using DifferentiallyPrivate.Services;
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
        //GET : Create new MultiChart and return to view
        [Authorize]
        [HttpGet]
        public ActionResult ChartingSimple()
        {
            MultiChart mc = new MultiChart();
            ChartModel chart = new ChartModel(0);
            mc.allCharts.Add(chart);
            return View(mc);
        }

        //POST : Route to appropriate functionality
        [Authorize]
        [HttpPost]
        public ActionResult ChartingSimple(MultiChart mc)
        {
            if (Request.IsAjaxRequest()) //AJAX Request - Initialise and build charts
            {
                //Build charts
                foreach (var chart in mc.allCharts)
                {
                    if (chart.IsValid())
                    {
                        chart.InitChart();
                        chart.FillChart();
                    }
                }

                //Return MultiChart (built) to view
                ChartModel cm = new ChartModel(mc.allCharts.Count);
                mc.allCharts.Add(cm);
                return PartialView("_Chart", mc);
            }
            else //Non-AJAX Request - Remove or add chart
            {
                if (Request["removeId"] != null) //Remove chart - Request key = id of removed chart
                {
                    //Clear model state to avoid cached values
                    ModelState.Clear();

                    //Remove chart
                    int id = Int32.Parse(Request["removeId"].ToString());
                    mc.allCharts.RemoveAt(id);

                    //If no charts exist, recreate first chart (for editing)
                    if (mc.allCharts.Count == 0)
                    {
                        ChartModel chart = new ChartModel(0);
                        mc.allCharts.Add(chart);
                        return View(mc);
                    }
                    else //Otherwise add a new chart (for editing)
                    {
                        int newId = mc.allCharts.Last().id + 1;
                        ChartModel chart = new ChartModel(newId);
                        mc.allCharts.Add(chart);
                        return View(mc);
                    }
                }
                else //Add chart
                {
                    if (mc.allCharts.Last().IsValid()) //Added chart is valid
                    {
                        //Add new chart (for editing) and return to view
                        ChartModel cm = new ChartModel(mc.allCharts.Last().id + 1);
                        mc.allCharts.Add(cm);
                        foreach (var c in mc.allCharts)
                        {
                            c.InitChart();
                        }
                        return View(mc);
                    }
                    else //Added chart is invalid
                    {
                        //Clear model state to avoid cached values
                        ModelState.Clear();

                        //Return error, remove invalid chart, add new chart (for editing)
                        ModelState.AddModelError("", "Cannot add query - values invalid!");
                        int lastID = mc.allCharts.Last().id;
                        mc.allCharts.Remove(mc.allCharts.Last());
                        ChartModel cm = new ChartModel(lastID);
                        mc.allCharts.Add(cm);
                        return View(mc);
                    }
                }
            }
        }

        public ActionResult ChartingUnknown()
        {
            return View();
        }

        //GET : Create new MultiChart and return to view
        [Authorize]
        [HttpGet]
        public ActionResult ChartingHome()
        {
            MultiChart mc = new MultiChart();
            HomeChartModel homeChart = new HomeChartModel(0);
            mc.allHomeCharts.Add(homeChart);
            return View(mc);
        }

        //POST : Route to appropriate functionality
        [Authorize]
        [HttpPost]
        public ActionResult ChartingHome(MultiChart mc)
        {
            if (Request.IsAjaxRequest()) //AJAX Request - Initialise and build charts
            {
                foreach (var chart in mc.allHomeCharts)
                {
                    //Get data from database and set chart data
                    DBInterface DBI = new DBInterface();
                    double[] data = DBI.GetDoublesFromDB(chart.data_cat_input, chart.timespan_input);
                    chart.setData(data);

                    //Build charts
                    if (chart.IsValid())
                    {
                        chart.InitChart();
                        chart.FillChart();
                    }
                }

                //Return MultiChart (built) to view
                HomeChartModel cm = new HomeChartModel(mc.allHomeCharts.Count);
                mc.allHomeCharts.Add(cm);
                return PartialView("_Chart", mc);
            }
            else //Non-AJAX Request - Remove or add chart
            {
                if (Request["removeId"] != null) //Remove chart - Request key = id of removed chart
                {
                    //Clear model state to avoid cached values
                    ModelState.Clear();

                    //Remove chart
                    int id = Int32.Parse(Request["removeId"].ToString());
                    mc.allHomeCharts.RemoveAt(id);

                    //If no charts exist, recreate first chart (for editing)
                    if (mc.allHomeCharts.Count == 0)
                    {
                        HomeChartModel homeChart = new HomeChartModel(0);
                        mc.allHomeCharts.Add(homeChart);
                        return View(mc);
                    }
                    else //Otherwise add new chart (for editing)
                    {
                        int newId = mc.allHomeCharts.Last().id + 1;
                        HomeChartModel homeChart = new HomeChartModel(newId);
                        mc.allHomeCharts.Add(homeChart);
                        return View(mc);
                    }
                }
                else //Add chart
                {
                    if (mc.allHomeCharts.Last().IsValid()) //Added chart is valid
                    {
                        //Add new chart to be edited
                        HomeChartModel cm = new HomeChartModel(mc.allHomeCharts.Last().id + 1);
                        mc.allHomeCharts.Add(cm);
                        foreach (var chart in mc.allHomeCharts)
                        {
                            chart.InitChart();
                        }
                        return View(mc);
                    }
                    else //Added chart is invalid
                    {
                        //Return error, remove invalid chart, add new chart (for editing)
                        ModelState.AddModelError("", "Chart values invalid!");
                        mc.allHomeCharts.Remove(mc.allHomeCharts.Last());
                        HomeChartModel cm = new HomeChartModel(mc.allHomeCharts.Last().id);
                        mc.allHomeCharts.Add(cm);
                        return View(mc);
                    }
                }
            }
        }
    }
}

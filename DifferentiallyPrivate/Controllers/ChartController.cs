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
                if (Request["removeId"] != null) //Remove query, 1 form key = id of removed query
                {
                    ModelState.Clear();
                    int id = Int32.Parse(Request["removeId"].ToString());
                    mc.allCharts.RemoveAt(id);
                    //mc.UpdateIDs(); //Update IDs since one was removed

                    if (mc.allCharts.Count == 0)
                    {
                        ChartModel chart = new ChartModel(0);
                        mc.allCharts.Add(chart);
                        return View(mc);
                    }
                    else
                    {
                        int newId = mc.allCharts.Last().id + 1;
                        ChartModel chart = new ChartModel(newId);
                        mc.allCharts.Add(chart);
                        return View(mc);
                    }
                }
                else
                {
                    if (mc.allCharts.Last().IsValid()) //Added chart is valid - add new chart to be edited
                    {
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
                        ModelState.AddModelError("", "Chart values invalid!");
                        mc.allCharts.Remove(mc.allCharts.Last());
                        ChartModel cm = new ChartModel(mc.allCharts.Last().id);
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

        [Authorize]
        [HttpGet]
        public ActionResult ChartingHome()
        {
            MultiChart mc = new MultiChart();
            HomeChartModel homeChart = new HomeChartModel(0);
            mc.allHomeCharts.Add(homeChart);
            return View(mc);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartingHome(MultiChart mc)
        {
            if (Request.IsAjaxRequest()) //BUILD CHARTS IN ALLCHARTS
            {
                foreach (var chart in mc.allHomeCharts)
                {
                    //GET DATA FROM DATABASE **TO DO**
                    DBInterface DBI = new DBInterface();
                    double[] data = DBI.GetDoublesFromDB(chart.data_cat_input, chart.timespan_input);

                    while (data == null) { }

                    chart.setData(data);

                    if (chart.IsValid())
                    {
                        chart.InitChart();
                        chart.FillChart();
                    }
                }

                HomeChartModel cm = new HomeChartModel(mc.allHomeCharts.Count);
                mc.allHomeCharts.Add(cm);
                return PartialView("_Chart", mc);
            }
            else 
            {
                if (Request["removeId"] != null) //Remove query, 1 form key = id of removed query
                {
                    ModelState.Clear();
                    int id = Int32.Parse(Request["removeId"].ToString());
                    mc.allHomeCharts.RemoveAt(id);
                    //mc.UpdateIDs(); //Update IDs since one was removed

                    if (mc.allHomeCharts.Count == 0)
                    {
                        HomeChartModel homeChart = new HomeChartModel(0);
                        mc.allHomeCharts.Add(homeChart);
                        return View(mc);
                    }
                    else
                    {
                        int newId = mc.allHomeCharts.Last().id + 1;
                        HomeChartModel homeChart = new HomeChartModel(newId);
                        mc.allHomeCharts.Add(homeChart);
                        return View(mc);
                    }
                }
                else
                {
                    //ADD CHART TO ALLCHARTS
                    if (mc.allHomeCharts.Last().IsValid()) //Added chart is valid - add new chart to be edited
                    {
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

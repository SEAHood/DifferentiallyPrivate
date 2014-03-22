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
                    if (Request.Form["type"].ToString() == "Average")
                    {
                        double[] iList = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                        //Needs validation
                        int iterations = Int32.Parse(Request.Form["iterations"].ToString());
                        double epsilon = Double.Parse(Request.Form["epsilon"].ToString());
                        int binCount = Int32.Parse(Request.Form["bins"].ToString());
                        string data = Request.Form["data"].ToString();
                        string type = Request.Form["type"].ToString();

                        data = data.Replace(" ", "");

                        string[] tokenisedData = data.Split(',');
                        double[] customList = new double[tokenisedData.Count()];

                        for (int i = 0; i < tokenisedData.Count(); i++)
                        {
                            customList[i] = Double.Parse(tokenisedData[i]);
                        }

                        ChartModels cm = new ChartModels(ChartTypes.Column);

                        DotNet.Highcharts.Highcharts chart = cm.FillChart(type, customList, iterations, epsilon, binCount);

                        return PartialView("_Chart", chart);
                    }
                    else
                    {
                        double[] iList = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                        //Needs validation
                        int iterations = Int32.Parse(Request.Form["iterations"].ToString());
                        double epsilon = Double.Parse(Request.Form["epsilon"].ToString());
                        int binCount = Int32.Parse(Request.Form["bins"].ToString());
                        string data = Request.Form["data"].ToString();
                        string type = Request.Form["type"].ToString();

                        data = data.Replace(" ", "");

                        string[] tokenisedData = data.Split(',');
                        double[] customList = new double[tokenisedData.Count()];

                        for (int i = 0; i < tokenisedData.Count(); i++)
                        {
                            customList[i] = Double.Parse(tokenisedData[i]);
                        }

                        ChartModels cm = new ChartModels(ChartTypes.Column);

                        DotNet.Highcharts.Highcharts chart = cm.FillChart(type, customList, iterations, epsilon, binCount);

                        return PartialView("_Chart", chart);
                    }
                }
                else
                {
                    double[] iList = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                    //Needs validation
                    int iterations = Int32.Parse(Request.Form["iterations"].ToString());
                    double epsilon = Double.Parse(Request.Form["epsilon"].ToString());
                    int binCount = Int32.Parse(Request.Form["bins"].ToString());
                    string data = Request.Form["data"].ToString();

                    data = data.Replace(" ", "");

                    string[] tokenisedData = data.Split(',');
                    double[] customList = new double[tokenisedData.Count()];

                    for (int i = 0; i < tokenisedData.Count(); i++)
                    {
                        customList[i] = Double.Parse(tokenisedData[i]);
                    }

                    ChartModels cm = new ChartModels(ChartTypes.Column);

                    DotNet.Highcharts.Highcharts chart = cm.FillChart("med", customList, iterations, epsilon, binCount);

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

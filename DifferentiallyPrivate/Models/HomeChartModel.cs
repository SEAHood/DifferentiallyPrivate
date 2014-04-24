using DifferentiallyPrivate.Services;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DifferentiallyPrivate.Models
{
    public class HomeChartModel
    {
        //Chart ID
        public int id { get; set; }

        //Chart
        public DotNet.Highcharts.Highcharts highchart;

        //Form fields
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Query Type: ")]
        public string queryType_input { get; set; }
        public IEnumerable<SelectListItem> query_types { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Distribution: ")]
        public string noiseType_input { get; set; }
        public IEnumerable<SelectListItem> noise_types { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Delta: ")]
        public string delta_input { get; set; }

        [Required]
        [Display(Name = "Data category: ")]
        public string data_cat_input { get; set; }
        public IEnumerable<SelectListItem> data_cats { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Iterations: ")]
        public string iterations_input { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Epsilon: ")]
        public string epsilon_input { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "# of Bins: ")]
        public string binCount_input { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Timespan: ")]
        public string timespan_input { get; set; }
        public IEnumerable<SelectListItem> timespans { get; set; }

        public double actualResult { get; set; }

        private double[] data;
        private string data_input;
        private int iterations;
        private double epsilon;
        private int binCount;
        private string queryType;
        private int noiseType;
        private double delta;
        private string timespan;
        private string data_cat;

        public HomeChartModel()
        {

        }

        public HomeChartModel(int _id)
        {
            data_cats = new[] {
                new SelectListItem { Value = "temp", Text = "Temperature" },
                new SelectListItem { Value = "humidity", Text = "Humidity" },
                //new SelectListItem { Value = "power", Text = "Power" },        ISSUE NEEDS FIXING
                new SelectListItem { Value = "windspeed", Text = "Wind Speed" }
            };
            noise_types = new[] {
                new SelectListItem { Value = "laplace", Text = "Laplace" },
                new SelectListItem { Value = "gaussian", Text = "Gaussian" }
            };
            query_types = new[] {
                new SelectListItem { Value = "avg", Text = "Average" },
                new SelectListItem { Value = "med", Text = "Median" }
            };
            timespans = new[] {
                new SelectListItem { Value = "3600", Text = "Hour" },
                new SelectListItem { Value = "86400", Text = "Day" },
                new SelectListItem { Value = "604800", Text = "Week" },
                new SelectListItem { Value = "2629740", Text = "Month" }
            };

            id = _id;
            queryType_input = "avg";
            data_cat_input = "temp";
            timespan_input = "hour";

            //Defaults
            iterations_input = "1000";
            epsilon_input = "1";
            binCount_input = "10";
            queryType_input = "avg";
            noiseType_input = "laplace";
            delta_input = "0.0001";
        }

        public void InitChart()
        {
            string chartName = "chart" + id.ToString();
            highchart = new DotNet.Highcharts.Highcharts(chartName)
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = ChartTypes.Column,
                        Width = 600,
                        Height = 350
                    });
        }

        public void setData(double[] _data_input)
        {
            data = _data_input;
        }

        public bool IsValid()
        {
            try
            {
                //Iterations
                iterations = Int32.Parse(iterations_input);

                //Epsilon
                epsilon = Double.Parse(epsilon_input);

                //Delta
                delta = Double.Parse(delta_input);

                //Bins
                binCount = Int32.Parse(binCount_input);

                //Query Type
                queryType = queryType_input;

                //Noise Type
                if (noiseType_input == "laplace")
                    noiseType = 0;
                else if (noiseType_input == "gaussian")
                    noiseType = 1;
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Highcharts FillChart()
        {
                PINQAnalyser PINQA = new PINQAnalyser() { iData = data, 
                                                          iterations = iterations,
                                                          epsilon = epsilon,
                                                          binCount = binCount,
                                                          noiseType = noiseType,
                                                          delta = delta};

                object[][] results = null;


                if (queryType == "avg")
                {
                    results = PINQA.DoAverageAnalysis();
                    actualResult = data.Average();
                }
                else if (queryType == "med")
                {
                    results = PINQA.DoMedianAnalysis();
                    int count = data.Count();
                    var orderedData = data.OrderBy(x => x);
                    double median = orderedData.ElementAt(count / 2) + orderedData.ElementAt((count - 1) / 2);
                    median /= 2;
                    actualResult = median;
                }

                object[] xAxis = results[0];
                object[] yAxis = results[1];

                /*for (int i = 0; i < xAxis.Count() - 1; i++)
                {
                    xAxis[i] = Math.Round(Double.Parse(xAxis[i].ToString()), 2).ToString();
                }*/

                highchart.SetXAxis(new DotNet.Highcharts.Options.XAxis
                                {
                                    Categories = xAxis.OfType<string>().Select((o) => (string)o).ToArray()
                                })
                                .SetSeries(new DotNet.Highcharts.Options.Series
                                {
                                    Data = new DotNet.Highcharts.Helpers.Data(yAxis)
                                });
                highchart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "Simple Chart" });
                highchart.SetTitle(new Title()
                {
                    Text = "PINQ " + queryType + "s (" + iterations + " iterations; " +
                                                binCount + " bins; " +
                                                "ε = " + epsilon + ")"
                });

                return highchart;
        }
    }
}
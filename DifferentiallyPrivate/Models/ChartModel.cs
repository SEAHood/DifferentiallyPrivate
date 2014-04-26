using DifferentiallyPrivate.Services;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DifferentiallyPrivate.Models
{
    /// <summary>
    /// ChartModel - Main model for a single chart/query
    /// Holds all information about a single chart or query
    /// -   Highchart
    /// -   All query parameters
    /// Provides methods for invoking the generation of the chart by using PINQAnalyser
    /// </summary>
    public class ChartModel
    {
        //Chart ID
        public int id { get; set; }

        //Chart
        public DotNet.Highcharts.Highcharts highchart;

        //Form fields
        [Required]
        [Display(Name = "Query Type: ")]
        public string queryType_input { get; set; }
        public IEnumerable<SelectListItem> query_types { get; set; }

        [Required]
        [Display(Name = "Distribution: ")]
        public string noiseType_input { get; set; }
        public IEnumerable<SelectListItem> noise_types { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Delta: ")]
        public string delta_input { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Data list: ")]
        public string data_input { get; set; }

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

        //Data fields with true types
        private double[] data;
        private int iterations;
        private double epsilon;
        private int binCount;
        private string queryType;
        private int noiseType;
        private double delta;

        //Actual result for (non-DP) average/median
        public double actualResult { get; set; }

        //Stopwatch for time taken
        public Stopwatch stopwatch { get; set; }

        //Empty constructor - not used, but required
        public ChartModel() { }

        //Constructor - setup chart initially
        public ChartModel(int _id)
        {
            //Set noise and query type arrays
            noise_types = new[] {
                new SelectListItem { Value = "laplace", Text = "Laplace" },
                new SelectListItem { Value = "gaussian", Text = "Gaussian" }
            };
            query_types = new[] {
                new SelectListItem { Value = "avg", Text = "Average" },
                new SelectListItem { Value = "med", Text = "Median" }
            };

            //Set chart ID
            id = _id;

            //Defaults
            iterations_input = "100";
            epsilon_input = "1";
            binCount_input = "10";
            queryType_input = "avg";
            data_input = "10,20,30,40,50,60,70,80,90,100";
            noiseType_input = "laplace";
            delta_input = "0.0001";
        }

        //Initialise chart
        public void InitChart()
        {
            string chartName = "chart" + id.ToString();
            highchart = new DotNet.Highcharts.Highcharts(chartName)
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = ChartTypes.Column,
                        Width = 800,
                        Height = 350
                    });

            stopwatch = new Stopwatch();
        }

        //Validates the chart
        //Returns true if valid, false otherwise
        public bool IsValid()
        {
            try
            {
                //Data validation
                data_input = data_input.Replace(" ", "");

                //Extract double[] array from comma-delimited string
                string[] tokenisedData = data_input.Split(',');
                data = new double[tokenisedData.Count()];

                for (int i = 0; i < tokenisedData.Count(); i++)
                {
                    data[i] = Double.Parse(tokenisedData[i]);
                }
                data = data.OrderBy(x => x).ToArray();

                //Iterations validation
                iterations = Int32.Parse(iterations_input);
                if (iterations < 0 || iterations > 100000)
                    return false;

                //Epsilon validation
                epsilon = Double.Parse(epsilon_input);
                if (epsilon < 0 || epsilon > Double.MaxValue)
                    return false;

                //Delta validation
                delta = Double.Parse(delta_input);
                if (delta < 0 || delta > 1)
                    return false;

                //Bins validation
                binCount = Int32.Parse(binCount_input);
                if (binCount < 0 || binCount > 250)
                    return false;

                //Query Type validation
                queryType = queryType_input;

                //Noise Type validation
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

        //Builds chart
        //Calls PINQAnalyser to perform DP with parameters
        //Returns resulting Highchart
        public Highcharts FillChart()
        {
            //Start timer
            stopwatch.Start();

            //Create PINQAnalyser object and results object[][]
            PINQAnalyser PINQA = new PINQAnalyser() { iData = data, 
                                                        iterations = iterations,
                                                        epsilon = epsilon,
                                                        binCount = binCount,
                                                        noiseType = noiseType,
                                                        delta = delta};
            object[][] results = null;

            //Get DP and Non-DP results
            if (queryType == "avg") //Average analysis
            {
                //DP
                results = PINQA.DoAverageAnalysis();

                //Non-DP
                actualResult = data.Average();
            }
            else if (queryType == "med")
            {
                //DP
                results = PINQA.DoMedianAnalysis();

                //Non-DP
                int count = data.Count();
                var orderedData = data.OrderBy(x => x);
                double median = orderedData.ElementAt(count / 2) + orderedData.ElementAt((count - 1) / 2);
                median /= 2;
                actualResult = median;
            }
                
            //Extract axes from results
            object[] xAxis = results[0]; //Value ranges
            object[] yAxis = results[1]; //Occurences

            //Set up X axis
            highchart.SetXAxis(new DotNet.Highcharts.Options.XAxis
            {
                Categories = xAxis.OfType<string>().Select((o) => (string)o).ToArray(),
                Labels = new XAxisLabels { Enabled = false } //Hide labels to stop congestion
            })
            .SetSeries(new DotNet.Highcharts.Options.Series
            {
                Data = new DotNet.Highcharts.Helpers.Data(yAxis)
            });

            //Set up credits and title, remove legend
            highchart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "DifferentiallyPrivate.com" });
            highchart.SetLegend(new Legend { Enabled = false });
            highchart.SetTitle(new Title()
            {
                Text = "PINQ " + queryType + "s (" + iterations + " iterations; " +
                                            binCount + " bins; " +
                                            "ε = " + epsilon + ")"
            });

            //Stop timer
            stopwatch.Stop();

            return highchart;
        }
    }
}
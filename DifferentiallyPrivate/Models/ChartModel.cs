using DifferentiallyPrivate.Services;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DifferentiallyPrivate.Models
{
    public class ChartModel
    {
        public DotNet.Highcharts.Highcharts chart;

        /*public string queryType { get; set; }
        public double[] data { get; set; }

        [Required]
        [DataType(DataType.Custom)]
        [Display(Name = "Iterations")]
        public int iterations { get; set; }

        [Required]
        [DataType(DataType.Custom)]
        [Display(Name = "Epsilon")]
        public double epsilon { get; set; }

        [Required]
        [DataType(DataType.Custom)]
        [Display(Name = "BinCount")]
        public int binCount { get; set; }*/

        public ChartModel(ChartTypes ct)
        {
            chart = new DotNet.Highcharts.Highcharts("chart")
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = ct,
                        Width = 500,
                        Height = 300
                    });
        }

        public DotNet.Highcharts.Highcharts FillChart(string queryType, double[] data, int iterations, double epsilon, int binCount)
        {
            data = data.OrderBy(x => x).ToArray();

            PINQAnalyser PINQA = new PINQAnalyser() { iData = data, iterations = iterations, epsilon = epsilon, binCount = binCount };


            object[][] averages;

            if (queryType == "Average")
                averages = PINQA.DoAverageAnalysis();
            else if (queryType == "Median")
                averages = PINQA.DoMedianAnalysis();
            else
                averages = PINQA.DoAverageAnalysis();

            object[] xAxis = averages[0];
            object[] yAxis = averages[1];

            for (int i = 0; i < xAxis.Count() - 1; i++ )
            {
                xAxis[i] = Math.Round(Double.Parse(xAxis[i].ToString()), 2).ToString();
            }

            chart.SetXAxis(new DotNet.Highcharts.Options.XAxis
                    {
                        Categories = xAxis.OfType<string>().Select((o) => (string)o).ToArray()
                    })
                    .SetSeries(new DotNet.Highcharts.Options.Series
                    {
                        Data = new DotNet.Highcharts.Helpers.Data(yAxis)
                    });
            chart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "Simple Chart" });

            chart.SetTitle(new Title()
            {
                Text = "PINQ " + queryType + "s (" + iterations + " iterations; " +
                                            binCount + " bins; " +
                                            "ε = " + epsilon + ")"
            });
  
            
            return chart;
        }

    }
}
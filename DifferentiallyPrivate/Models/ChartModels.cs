using DifferentiallyPrivate.Services;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DifferentiallyPrivate.Models
{
    public class ChartModels
    {
        public DotNet.Highcharts.Highcharts chart;


        public ChartModels(ChartTypes ct)
        {
            chart = new DotNet.Highcharts.Highcharts("chart")
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = ct,
                        Width = 500,
                        Height = 300
                    });
        }

        public DotNet.Highcharts.Highcharts FillChart(double[] data, int iterations, double epsilon, int binCount)
        {
            data = data.OrderBy(x => x).ToArray();

            PINQAnalyser PINQA = new PINQAnalyser() { iData = data, iterations = iterations, epsilon = epsilon, binCount = binCount };

            object[][] averages = PINQA.DoAverageAnalysis();
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
            chart.SetTitle(new Title() { Text = "PINQ Averages (" + iterations + " iterations; " + 
                                                                    binCount + " bins; " + 
                                                                    "ε = " + epsilon + ")" });
            
  
            
            return chart;
        }

    }
}
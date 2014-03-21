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
        //public DotNet.Highcharts.Highcharts chart;
        //NOT MUCH OF A MODEL SO FAR
        public DotNet.Highcharts.Highcharts Begin(double[] data, int iterations, double epsilon, int binCount)
        {
            data = data.OrderBy(x => x).ToArray();

            PINQAnalyser PINQA = new PINQAnalyser() { iData = data, iterations = iterations, epsilon = epsilon, binCount = binCount };

            var averages = PINQA.DoAverageAnalysis();
          
            //double chartSizeX = high - low; //Size of chart
            var low = data[0];
            var high = data[data.Count() - 1];
            double chartSizeX = high - low;
            double groupSize = chartSizeX / (double)binCount;

            object[] yAxis = new object[Int32.Parse(binCount.ToString()) + 1];
            string[] xAxis = new string[Int32.Parse(binCount.ToString()) + 1];

            double hiJ = 0;

            for (double i = low, j = 0; i < high; i += groupSize, j++)
            {
                var groupList = averages.Where(x => x < i + groupSize && x > i);
                int valuesFound = groupList.Count();
                //chart1.Series[0].Points.AddXY(i, valuesFoundEps1);                    //valuesFound => Y axis for X = i
                //PINQChart.Series[0].Points.AddXY(i, valuesFoundEps10);

                if (hiJ < j)
                    hiJ = j;

                xAxis[(Int32)j] = i.ToString();
                yAxis[(Int32)j] = valuesFound;
            }

            //var a = 1;

            //CREATE CHART
            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                    .InitChart(new Chart
                    {
                        DefaultSeriesType = ChartTypes.Column,
                        Height = 350
                    })
                    .SetXAxis(new DotNet.Highcharts.Options.XAxis
                    {
                        Categories = xAxis
                    })
                    .SetSeries(new DotNet.Highcharts.Options.Series
                    {                        
                        Data = new DotNet.Highcharts.Helpers.Data(yAxis)
                    });
            chart.SetCredits(new DotNet.Highcharts.Options.Credits() { Text = "Simple Chart" });
            
            return chart;
        }

        private void PopulateChart(double[] data)
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
        }
    }
}
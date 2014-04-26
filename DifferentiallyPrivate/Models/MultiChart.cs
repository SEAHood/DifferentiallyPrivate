using DotNet.Highcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DifferentiallyPrivate.Models
{
    /// <summary>
    /// MultiChart - holds lists of both ChartModel and HomeChartModel
    /// </summary>
    public class MultiChart
    {
        public List<ChartModel> allCharts { get; set; }
        public List<HomeChartModel> allHomeCharts { get; set; }

        public MultiChart()
        {
            allCharts = new List<ChartModel>();
            allHomeCharts = new List<HomeChartModel>();
        }
    }
}
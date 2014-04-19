using DotNet.Highcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DifferentiallyPrivate.Models
{
    public class MultiChart
    {
        public List<ChartModel> allCharts { get; set; }
        public List<HomeChartModel> allHomeCharts { get; set; }

        public MultiChart()
        {
            allCharts = new List<ChartModel>();
            allHomeCharts = new List<HomeChartModel>();
        }

        public void UpdateIDs()
        {
            for (int i = 0; i < allCharts.Count; i++)
            {
                allCharts[i].id = i;
            }

            for (int i = 0; i < allHomeCharts.Count; i++)
            {
                allHomeCharts[i].id = i;
            }
        }
    }
}
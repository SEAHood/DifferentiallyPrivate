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

        public MultiChart()
        {
            allCharts = new List<ChartModel>();
        }
    }
}
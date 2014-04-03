using DifferentiallyPrivate.Services;
using DotNet.Highcharts;
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
        public int id { get; set; }
        public DotNet.Highcharts.Highcharts highchart;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Query Type: ")]
        public string queryType_input { get; set; }

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

        private double[] data;
        private int iterations;
        private double epsilon;
        private int binCount;
        private string queryType;

        public ChartModel()
        {

        }

        public ChartModel(int _id)
        {
            id = _id;

            //Defaults
            iterations_input = "1000";
            epsilon_input = "1";
            binCount_input = "100";
            queryType_input = "avg";
            data_input = "1,2,3,4,5,6,7,8,9,10";
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

        public bool IsValid()
        {
            try
            {
                //Data
                data_input = data_input.Replace(" ", "");

                string[] tokenisedData = data_input.Split(',');
                data = new double[tokenisedData.Count()];

                for (int i = 0; i < tokenisedData.Count(); i++)
                {
                    data[i] = Double.Parse(tokenisedData[i]);
                }
                data = data.OrderBy(x => x).ToArray();

                //Iterations
                iterations = Int32.Parse(iterations_input);

                //Epsilon
                epsilon = Double.Parse(epsilon_input);

                //Bins
                binCount = Int32.Parse(binCount_input);

                //Query Type
                queryType = queryType_input;
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Highcharts FillChart()
        {
                PINQAnalyser PINQA = new PINQAnalyser() { iData = data, iterations = iterations, epsilon = epsilon, binCount = binCount };

                object[][] results = null;

                if (queryType == "avg")
                    results = PINQA.DoAverageAnalysis();
                else if (queryType == "med")
                    results = PINQA.DoMedianAnalysis();

                object[] xAxis = results[0];
                object[] yAxis = results[1];

                for (int i = 0; i < xAxis.Count() - 1; i++)
                {
                    xAxis[i] = Math.Round(Double.Parse(xAxis[i].ToString()), 2).ToString();
                }

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PINQAlternate;

namespace DifferentiallyPrivate.Services
{
    public class PINQAnalyser
    {
        public double[] iData { get; set; }
        public int iterations { get; set; }
        public double epsilon { get; set; }
        public int binCount { get; set; }
        
        public double[] DoAverageAnalysis()
        {
            double low = iData[0];                              //-------
            double high = iData[iData.Count() - 1];             //MAKE MORE GENERAL - NOT JUST DOUBLE
            int[] histoCount = new int[binCount];
            //SET UP CHART LOWS/HIGHS/INTERVAL
            double[] averages = new double[iterations];

            for (int i = 0; i < iterations; i++)
            {
                var agentNEW = new PINQAlternate.PINQAgentBudget(epsilon);
                var dataNEW = new PINQAlternate.PINQueryable<double>(iData.AsQueryable(), agentNEW);
                var pinqAvgNew = dataNEW.NoisyAverage(epsilon, x => (double)x);
                averages[i] = pinqAvgNew;
            }

            /*double chartSizeX = high - low; //Size of chart
            double groupSize = chartSizeX / (double)binCount;


            for (double i = low, j = 0; i < high; i += groupSize, j++)
            {
                var groupList = averages.Where(x => x < i + groupSize && x > i);
                int valuesFound = groupList.Count();
                //chart1.Series[0].Points.AddXY(i, valuesFoundEps1);                    //valuesFound => Y axis for X = i
                //PINQChart.Series[0].Points.AddXY(i, valuesFoundEps10);
            }*/
            //MOVED TO MODEL
            return averages;
        }
    }
}
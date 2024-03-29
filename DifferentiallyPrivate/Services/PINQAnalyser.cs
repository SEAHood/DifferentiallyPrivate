﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PINQAlternate;

namespace DifferentiallyPrivate.Services
{
    public class PINQAnalyser
    {
        public double[] iData { get; set; }
        public double[][] iDataClustered { get; set; }
        public int iterations { get; set; }
        public double epsilon { get; set; }
        public int binCount { get; set; }
        public int noiseType { get; set; } 
        public double delta { get; set; } 

        //RETURNS: object[<value>][<#calculated>]
        public object[][] DoAverageAnalysis()
        {
            double low = 0;
            double high = 100;
            int[] histoCount = new int[binCount];
            double[] averages = new double[iterations];

            for (int i = 0; i < iterations; i++)
            {
                var agentNEW = new PINQAlternate.PINQAgentBudget(epsilon);
                var dataNEW = new PINQAlternate.PINQueryable<double>(iData.AsQueryable(), agentNEW);
                var pinqAvgNew = dataNEW.NoisyAverage(epsilon, x => (double)x, noiseType, delta);
                averages[i] = pinqAvgNew;
            }

            double chartSizeX = high - low;
            decimal groupSize = (decimal)chartSizeX / (decimal)binCount;

            object[] yAxis = new object[Int32.Parse(binCount.ToString())];
            string[] xAxis = new string[Int32.Parse(binCount.ToString())];


            for (decimal i = (decimal)low, j = 0; i < (decimal)high && j < binCount; i += groupSize, j++)
            {
                var groupList = averages.Where(x => x <= (double)(i + groupSize) && x >= (double)i);
                int valuesFound = groupList.Count();

                xAxis[Int32.Parse(j.ToString())] = i.ToString() + " - " + (i + groupSize).ToString();
                yAxis[Int32.Parse(j.ToString())] = valuesFound;
            }

            return new object[][] { xAxis, yAxis };
        }

        //RETURNS: object[<value>][<#calculated>]
        public object[][] DoMedianAnalysis()
        {
            double low = iData[0];
            double high = iData[iData.Count() - 1];
            int[] histoCount = new int[binCount];
            double[] medians = new double[iterations];

            for (int i = 0; i < iterations; i++)
            {
                var agentNEW = new PINQAlternate.PINQAgentBudget(epsilon);
                var dataNEW = new PINQAlternate.PINQueryable<double>(iData.AsQueryable(), agentNEW);
                var pinqAvgNew = dataNEW.NoisyMedian(epsilon, x => (double)x);
                medians[i] = pinqAvgNew;
            }

            double chartSizeX = high - low;
            double groupSize = chartSizeX / (double)binCount;

            object[] yAxis = new object[Int32.Parse(binCount.ToString()) + 1];
            string[] xAxis = new string[Int32.Parse(binCount.ToString()) + 1];

            double hiJ = 0;

            for (double i = low, j = 0; i < high; i += groupSize, j++)
            {
                var groupList = medians.Where(x => x < i + groupSize && x > i);
                int valuesFound = groupList.Count();

                if (hiJ < j)
                    hiJ = j;

                xAxis[(Int32)j] = i.ToString();
                yAxis[(Int32)j] = valuesFound;
            }

            return new object[][] { xAxis, yAxis };
        }
    }
}
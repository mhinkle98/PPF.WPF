using System;
using SciChart.Charting.Model.DataSeries;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PPF.WPF.MVVM.Model
{
    public class SpectrometryChart : ChartBase
    {
        private string _chartName;
        
        [Newtonsoft.Json.JsonIgnore]
        private XyDataSeries<double, double> _chartDataSeries;
        
        /// <summary>
        /// double values to serialize our data aside from xydataseries
        /// </summary>
        private Dictionary<double, double> _chartDataSerializable;

        [JsonConstructor]
        public SpectrometryChart()
        {
            if (ChartDataSerializable == null)
            {
                ChartDataSeries = new XyDataSeries<double, double>();
                ChartDataSerializable = new Dictionary<double, double>();
                return;
            }

            ChartDataSeries = new XyDataSeries<double, double>();
            
            for (int i = 0; i < ChartDataSerializable.Count; i++)
            {
                var val = ChartDataSerializable.ElementAt(i);
                ChartDataSeries.XValues[i] = val.Key;
                ChartDataSeries.YValues[i] = val.Value;
            }
        }
        /*public SpectrometryChart(string name = "NewChart") : base(name)
        {
        }

        public SpectrometryChart(Dictionary<double, double> serializedChart, string name = "NewChart") : base(
            serializedChart, name)
        {
        }*/
        /*{
            base();
            if (ChartDataSerializable == null)
            {
                ChartDataSeries = new XyDataSeries<double, double>();
                ChartDataSerializable = new Dictionary<double, double>();
                return;
            }

            ChartDataSeries = new XyDataSeries<double, double>();
            
            foreach (var val in ChartDataSerializable)
            {
                ChartDataSeries.Append(val.Key, val.Value);
            }
        }*/
        public override Type ViewType { get; }
    }
}

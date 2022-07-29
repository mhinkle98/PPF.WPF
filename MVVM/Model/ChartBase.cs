using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Data.Numerics;

namespace PPF.WPF.MVVM.Model
{
    public abstract class ChartBase : BaseRenderableSeriesViewModel
    {
        private string _chartName;

        public string ChartName
        {
            get => _chartName; 
            set 
            { 
                _chartName = value; 
                OnPropertyChanged("ChartName");
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        private XyDataSeries<double, double> _chartDataSeries;

        [Newtonsoft.Json.JsonIgnore]
        public XyDataSeries<double, double> ChartDataSeries
        {
            get => _chartDataSeries;
            set
            {
                _chartDataSeries = value; 
                OnPropertyChanged("ChartDataSeries");
            }
        }

        private Dictionary<double, double> _chartDataSerializable;

        public Dictionary<double, double> ChartDataSerializable
        {
            get => _chartDataSerializable;
            set
            {
                _chartDataSerializable = value;
                OnPropertyChanged("ChartDataSerializable");
            }
        }

        /*protected ChartBase(string name = "NewChart")
        {
            ChartName = name;
            if (ChartDataSerializable == null)
            {
                ChartDataSerializable = new Dictionary<double, double>();
            }

            if (ChartDataSeries == null)
            {
                ChartDataSeries = new XyDataSeries<double, double>();
            }
            
            for (int i = 0; i < ChartDataSerializable.Count; i++)
            {
                var val = ChartDataSerializable.ElementAt(i);
                ChartDataSeries.XValues[i] = val.Key;
                ChartDataSeries.YValues[i] = val.Value;
            }
        }

        protected ChartBase(Dictionary<double, double> serializedChart, string name = "NewChart")
        {
            ChartName = name;
            ChartDataSerializable = serializedChart;
            ChartDataSeries = new XyDataSeries<double, double>();
            foreach (var val in ChartDataSerializable)
            {
                ChartDataSeries.Append(val.Key, val.Value);
            }
        }*/
    }
}

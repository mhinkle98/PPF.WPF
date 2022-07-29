using System;
using SciChart.Charting.Model.DataSeries;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace PPF.WPF.MVVM.Model
{
    
    public class InterferometryChart : ChartBase
    {
        public enum ChartType
        {
            TemperatureDependent,
            WavelengthDependent,
            TimeDependent
        }

        private string _chartName;
        
        [Newtonsoft.Json.JsonIgnore]
        private XyDataSeries<double, double> _chartDataSeries;
        
        /// <summary>
        /// double values to serialize our data aside from xydataseries
        /// </summary>
        private Dictionary<double, double> _chartDataSerializable;
        
        public string XLabel { get; set; }
        public string YLabel { get; set; }

        public ChartType chartType;

        [JsonConstructor]
        public InterferometryChart()
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
        /*public InterferometryChart(string name = "NewChart", ChartType type = ChartType.WavelengthDependent) :
            base(name)
        {
            chartType = type;
        }

        public InterferometryChart(Dictionary<double, double> serializedChart, string name = "NewChart",
            ChartType type = ChartType.WavelengthDependent) : base(serializedChart, name)
        {
            chartType = type;
        }*/
        /*{
            //InterferometryChartSurface = new SciChartSurface();
            ChartDataSeries = new XyDataSeries<double, double>();
            ChartDataSerializable = new Dictionary<double, double>();
            //ChartDataSeriesList = new ObservableCollection<XyDataSeries<double, double>>();
            chartType = type;
            XLabel = "Wavelength (nm)";
            YLabel = "Phase offset";
        }*/
        public override Type ViewType { get; }
    }
}

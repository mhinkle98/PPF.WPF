using PPF.WPF.MVVM.Model;
using SciChart.Charting.Model.DataSeries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PPF.WPF.MVVM.View;

namespace PPF.WPF.MVVM.ViewModel
{
    public class InterferometryViewModel : ChartViewModel
    {
        public override string DisplayName { get; set; }

        private ObservableCollection<InterferometryChart> charts;
        public new ObservableCollection<InterferometryChart> Charts
        {
            get => charts;
            set
            {
                charts = value;
                OnPropertyChanged("Charts");
            }
        }

        private InterferometryChart selected;
        public new InterferometryChart Selected
        {
            get => selected;
            set
            {
                selected = value;
                Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
                OnPropertyChanged("Selected");
            }
        }

        public override void PopulateCharts()
        {
            if (Charts == null)
            {
                var chart = new InterferometryChart
                {
                    ChartName = "Chart1",
                    XLabel = "Temperature (C)"
                };

                Charts = new ObservableCollection<InterferometryChart>() { chart };
            }
            Selected = Charts[0];

            Selected.ChartDataSeries.AcceptsUnsortedData = true;
        }

        public InterferometryViewModel()
        {
            DisplayName = "Interferometry";
            PopulateCharts();
        }

        public override void DataPull(ChartBase _chart, IEnumerable<string> lines)
        {
            var chart = (InterferometryChart)_chart;
            var xtitle = "";
            void AddData(IReadOnlyList<string> line)
            {
                if (line.Count <= 1) return;
                var xVal = Convert.ToDouble(line[0]);
                var yVal = Convert.ToDouble(line[1]);
                chart.ChartDataSeries.XValues.Add(xVal);
                chart.ChartDataSeries.YValues.Add(yVal);
                try
                {
                    chart.ChartDataSerializable.Add(xVal, yVal);
                }
                catch (ArgumentException)
                {
                    chart.ChartDataSerializable[xVal] = yVal;
                }
            }
            if (lines.First().Split('\t', '\n').Length < 9)
            {
                // temperature dependent likely
                chart.chartType = InterferometryChart.ChartType.TemperatureDependent;
                chart.XLabel = "Temperature (C)";
                xtitle = "Temperature (C)";
                chart.YLabel = "Phase Offset";

                foreach (var line in lines)
                {
                    var templine = line.Split('\t', '\n');
                    AddData(templine);
                }
            }

            else
            {
                var times = new List<double>();
                var phase = new List<double>();
                var wavelengths = new List<double>();

                foreach (var line in lines)
                {
                    var templine = line.Split('\t', '\n');
                    if (templine.Length > 1)
                    {
                        times.Add(Convert.ToDouble(templine[0]));
                        phase.Add(Convert.ToDouble(templine[1]));
                        wavelengths.Add(Convert.ToDouble(templine[6]));
                    }
                }

                if (wavelengths.Distinct().ToArray().Length > 1)
                {
                    // wavelength dependent
                    chart.chartType = InterferometryChart.ChartType.WavelengthDependent;
                    chart.XLabel = "Wavelength (nm)";
                    xtitle = "Wavelength (nm)";
                    chart.YLabel = "Phase offset";

                    var currentWavelength = wavelengths[0];

                    double tempSum = 0;
                    var count = 0;

                    for (int i = 0; i < wavelengths.Count; i++)
                    {
                        if (wavelengths[i] == currentWavelength)
                        {
                            count += 1;
                            tempSum += phase[i];
                        }
                        else
                        {
                            var average = tempSum / count;
                            chart.ChartDataSeries.XValues.Add(currentWavelength);
                            chart.ChartDataSeries.YValues.Add(average);
                            chart.ChartDataSerializable.Add(currentWavelength, average);
                            tempSum = 0;
                            count = 0;
                            currentWavelength = wavelengths[i];
                        }
                    }
                }
                else
                {
                    // time dependent
                    chart.chartType = InterferometryChart.ChartType.TimeDependent;
                    chart.XLabel = "Timestep";
                    xtitle = "Timestep";
                    chart.YLabel = "Phase offset";

                    for (int i = 0; i < times.Count; i++)
                    {
                        chart.ChartDataSeries.XValues.Add(times[i]);
                        chart.ChartDataSeries.YValues.Add(phase[i]);
                        try
                        {
                            chart.ChartDataSerializable.Add(times[i], phase[i]);
                        }
                        catch (ArgumentException) { }
                    }
                }
            }
            Selected.ChartDataSeries.ParentSurface.XAxis.AxisTitle = xtitle;
        }
        
        public override void OpenDataToNewGraph()
        {
            var lines = GetFileLines();
            if (lines == null) return;
            var chart = new InterferometryChart
            {
                ChartDataSeries =
                {
                    AcceptsUnsortedData = true
                }
            };
            
            DataPull(chart, lines);
            
            chart.ChartName = "Chart" + (Charts.Count + 1);

            var temp = Charts;
            temp.Add(chart);
            Charts = temp;

            if (Charts.LastOrDefault() == null) return;
            Selected = Charts.LastOrDefault();
            Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        public override void OpenDataToCurrentGraph()
        {
            var lines = GetFileLines();
            if (lines == null) return;
            var temp = Selected;
            
            temp.ChartDataSeries.AcceptsUnsortedData = true;
            DataPull(temp, lines);

            
            Selected = temp;

            Selected.ChartDataSeries.AcceptsUnsortedData = true;
            Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private ICommand _openCommand;
        private ICommand _openCommand2;
        private ICommand _changeChartCommand;
        private ICommand _renameChartCommand;
        private ICommand _deleteChartCommand;

        public ICommand OpenDataNewGraphCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(
                        param => OpenDataToNewGraph());
                }

                return _openCommand;
            }
        }

        public ICommand OpenDataCurrentGraphCommand
        {
            get
            {
                return _openCommand2 ?? (_openCommand2 = new RelayCommand(
                    param => OpenDataToCurrentGraph()));
            }
        }

        public ICommand ChangeChartCommand
        {
            get
            {
                return _changeChartCommand ?? (_changeChartCommand = new RelayCommand(
                    p => ChangeChart((InterferometryChart)p),
                    p => p is InterferometryChart));
            }
        }

        public ICommand RenameChartCommand
        {
            get { return _renameChartCommand ?? (_renameChartCommand = new RelayCommand(p => ChangeChartName())); }
        }

        public ICommand DeleteChartCommand
        {
            get
            {
                return _deleteChartCommand ?? (_deleteChartCommand = new RelayCommand(
                    p => DeleteChart()));
            }
        }

        private void DeleteChart()
        {
            if (Charts.Count == 1)
            {
                var chart = new InterferometryChart
                {
                    ChartName = "Chart1"
                };
                Charts = new ObservableCollection<InterferometryChart>() { chart };
                Selected = chart;
                return;
            }
            Charts.Remove(Selected);
            if (Charts.Count == 0)
            {
                PopulateCharts();
            }
            else
            {
                Selected = Charts[Charts.Count - 1];
                Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
            }
        }

        private void ChangeChart(InterferometryChart chart)
        {
            var sel = Charts.FirstOrDefault(c => c == chart);
            var xtitle = "";
            Selected = sel;
            if (Selected == null) return;
            Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
            switch (Selected.chartType)
            {
                case InterferometryChart.ChartType.TemperatureDependent:
                    xtitle = "Temperature (C)";
                    break;
                case InterferometryChart.ChartType.WavelengthDependent:
                    xtitle = "Wavelength (nm)";
                    break;
                case InterferometryChart.ChartType.TimeDependent:
                    xtitle = "Timestep";
                    break;
                default:
                    xtitle = "Wavelength (nm)";
                    break;
            }

            Selected.ChartDataSeries.ParentSurface.XAxis.AxisTitle = xtitle;
        }

        private delegate void Rename(string name);

        private void ChangeChartName()
        {
            var newName = ShowRenameDialog();
            if (newName != null) 
                Selected.ChartName = newName;
        }

        private string ShowRenameDialog()
        {
            var rw = new RenameWindow(Selected.ChartName);
            rw.ShowDialog();

            return rw.ChartName;
        }
    }
}

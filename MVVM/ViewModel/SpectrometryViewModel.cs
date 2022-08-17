using SciChart.Charting.Model.DataSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PPF.WPF.MVVM.Model;
using System.Collections.ObjectModel;
using PPF.WPF.MVVM.View;

namespace PPF.WPF.MVVM.ViewModel
{
    public class SpectrometryViewModel : ChartViewModel
    {
        public sealed override string DisplayName { get; set; }

        private ObservableCollection<SpectrometryChart> _charts;
        public ObservableCollection<SpectrometryChart> Charts { get => _charts;
            // ReSharper disable once MemberCanBePrivate.Global
            set { _charts = value; OnPropertyChanged("Charts"); }
        }

        private SpectrometryChart _selected;
        // ReSharper disable once MemberCanBePrivate.Global
        public SpectrometryChart Selected
        {
            get => _selected ?? (_selected = new SpectrometryChart());
            set
            {
                _selected = value;
                Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
                OnPropertyChanged("Selected");
            }
        }

        public sealed override void PopulateCharts()
        {
            if (Charts == null)
            {
                var chart = new SpectrometryChart
                {
                    ChartName = "Chart1"
                };
                Charts = new ObservableCollection<SpectrometryChart>() { chart };
            }
            
            Selected = Charts[0];
            Selected.ChartDataSeries.AcceptsUnsortedData = true;
        }

        
        public SpectrometryViewModel()
        {
            PopulateCharts();
            DisplayName = "Spectrometry";
            //Trace.WriteLine(Charts.Count.ToString());
            //Instance = this;
        }

        public override void DataPull(ChartBase chart, IEnumerable<string> lines)
        {
            chart = chart as SpectrometryChart;
            if (chart == null)
            {
                throw new NullReferenceException();
            }
            
            foreach (var line in lines)
            {
                var tempLine = line.Split('\t', '\n');
                if (tempLine.Length <= 1) continue;
                if (!(Convert.ToDouble(tempLine[1]) < 100) || !(Convert.ToDouble(tempLine[1]) > 0)) continue;
                var xVal = Convert.ToDouble(tempLine[0]);
                var yVal = Convert.ToDouble(tempLine[1]);
                chart.ChartDataSeries.XValues.Add(xVal);
                chart.ChartDataSeries.YValues.Add(yVal);
                try
                {
                    chart.ChartDataSerializable.Add(xVal, yVal);
                }
                catch (ArgumentException) { }
            }
        }

        public override void OpenDataToNewGraph()
        {
            var lines = GetFileLines();
            if (lines == null) return;
            var chart = new SpectrometryChart
            {
                ChartDataSeries =
                {
                    AcceptsUnsortedData = true
                }
            };

            DataPull(chart, lines);
            
            chart.ChartName = chart.ChartName = "Chart" + (Charts.Count + 1);

            var temp = Charts;
            temp.Add(chart);
            Charts = temp;
            
            Selected = Charts.LastOrDefault();
            Selected?.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
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
                return _openCommand ?? (_openCommand = new RelayCommand(
                    param => this.OpenDataToNewGraph()));
            }
        }

        public ICommand OpenDataCurrentGraphCommand
        {
            get
            {
                return _openCommand2 ?? (_openCommand2 = new RelayCommand(
                    param => this.OpenDataToCurrentGraph()));
            }
        }

        public ICommand ChangeChartCommand
        {
            get
            {
                return _changeChartCommand ?? (_changeChartCommand = new RelayCommand(
                    p => ChangeChart((SpectrometryChart)p),
                    p => p is SpectrometryChart));
            }
        }

        public ICommand RenameChartCommand
        {
            get { return _renameChartCommand ?? (_renameChartCommand = new RelayCommand(param => ChangeChartName())); }
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
                var chart = new SpectrometryChart
                {
                    ChartName = "Chart1"
                };
                Charts = new ObservableCollection<SpectrometryChart>() { chart };
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

        private void ChangeChart(SpectrometryChart chart)
        {
            var sel = Charts.FirstOrDefault(c => c == chart);
            Selected = sel;
            Selected?.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

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

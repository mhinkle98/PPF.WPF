using Microsoft.Win32;
using SciChart.Charting.Model.DataSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using SciChart.Charting.Visuals;
using System.Windows.Controls;
using PPF.WPF.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;
using Newtonsoft.Json;
using SciChart.Charting.Model.ChartSeries;

namespace PPF.WPF.MVVM.ViewModel
{
    public class SpectrometryViewModel : ChartViewModel
    {
        public override string DisplayName { get; set; }

        private ObservableCollection<SpectrometryChart> charts;
        public new ObservableCollection<SpectrometryChart> Charts { get => charts;
            set { charts = value; OnPropertyChanged("Charts"); }
        }

        private SpectrometryChart selected;
        public new SpectrometryChart Selected
        {
            get
            {
                if (selected == null)
                {
                    selected = new SpectrometryChart();
                }
                return selected;
            }
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
                var chart = new SpectrometryChart();
                chart.ChartName = "Chart1";

                Charts = new ObservableCollection<SpectrometryChart>() { chart };
                
                //Instance = this;
            }
            
            Selected = Charts[0];

            Selected.ChartDataSeries.AcceptsUnsortedData = true;
        }

        
        public SpectrometryViewModel(bool isNew = true)
        {
            PopulateCharts();
            DisplayName = "Spectrometry";
            //Trace.WriteLine(Charts.Count.ToString());
            //Instance = this;
        }

        public override void DataPull(ChartBase _chart, IEnumerable<string> lines)
        {
            var chart = (SpectrometryChart)_chart;
            foreach (var line in lines)
            {
                var templine = line.Split('\t', '\n');
                if (templine.Length <= 1) continue;
                if (!(Convert.ToDouble(templine[1]) < 100) || !(Convert.ToDouble(templine[1]) > 0)) continue;
                var xVal = Convert.ToDouble(templine[0]);
                var yVal = Convert.ToDouble(templine[1]);
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
                        param => this.OpenDataToNewGraph());
                }

                return _openCommand;
            }
        }

        public ICommand OpenDataCurrentGraphCommand
        {
            get
            {
                if (_openCommand2 == null)
                {
                    _openCommand2 = new RelayCommand(
                        param => this.OpenDataToCurrentGraph());
                }
                return _openCommand2;
            }
        }

        public ICommand ChangeChartCommand
        {
            get
            {
                if (_changeChartCommand == null)
                {
                    _changeChartCommand = new RelayCommand(
                        p => ChangeChart((SpectrometryChart)p),
                        p => p is SpectrometryChart);
                }
                return _changeChartCommand;
            }
        }

        public ICommand RenameChartCommand
        {
            get
            {
                if (_renameChartCommand == null)
                {
                    _renameChartCommand = null;
                }
                return _renameChartCommand;
            }
        }

        public ICommand DeleteChartCommand
        {
            get
            {
                if (_deleteChartCommand == null)
                {
                    _deleteChartCommand = new RelayCommand(
                        p => DeleteChart());
                }
                return _deleteChartCommand;
            }
        }

        private void DeleteChart()
        {
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
            Selected.ChartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private delegate void Rename(string name);

        private void ChangeChartName(string name)
        {
            Selected.ChartName = name;
        }

        private static string ShowDialog()
        {
            Window prompt = new Window()
            {
                Width = 500,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            Label textLabel = new Label() { Margin = new Thickness(50), Content = "Rename chart" };
            TextBox textBox = new TextBox() { Margin = new Thickness(20), Width = 400 };
            Button confirmation = new Button() { Content = "OK", Margin = new Thickness(350) };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            var grid = prompt.Content as Grid;
            grid.Children.Add(textLabel);
            grid.Children.Add(textBox);
            grid.Children.Add(confirmation);

            return prompt.ShowDialog() == (textBox.Text.Length > 0) ? textBox.Text : "";
        }
    }

    public static class Prompt
    {
        public static string ShowDialog()
        {
            Window prompt = new Window()
            {
                Width = 500,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            Label textLabel = new Label() { Margin = new Thickness(50), Content = "Rename chart" };
            TextBox textBox = new TextBox() { Margin = new Thickness(20), Width = 400 };
            Button confirmation = new Button() { Content = "OK",Margin = new Thickness(350) };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            var grid = prompt.Content as Grid;
            grid.Children.Add(textLabel);
            grid.Children.Add(textBox);
            grid.Children.Add(confirmation);

            return prompt.ShowDialog() == (textBox.Text.Length > 0) ? textBox.Text : "";
        }
    }
}

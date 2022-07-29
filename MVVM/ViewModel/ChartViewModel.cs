using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPF.WPF.MVVM.Model;
using SciChart.Charting.Model.DataSeries;

namespace PPF.WPF.MVVM.ViewModel
{
    public abstract class ChartViewModel : ObservableObject, IProcessViewModel
    {
        public abstract string DisplayName { get; set; }

        private ObservableCollection<IChart> charts;
        /*public ObservableCollection<IChart> Charts
        {
            get
            {
                if (charts == null)
                {
                    charts = new ObservableCollection<IChart>();
                }

                return charts;
            }
            set
            {
                charts = value;
                OnPropertyChanged("Charts");
            }
        }*/

        private IChart selected;

        public abstract void PopulateCharts();

        protected IEnumerable<string> GetFileLines()
        {
            var ofDialog = new OpenFileDialog
            {
                Title = "Open " + DisplayName + " Text File",
                Filter = "All files|*.*|Text files|*.txt"
            };
            var result = ofDialog.ShowDialog();

            return ofDialog.FileName.Length == 0 ? null : File.ReadLines(ofDialog.FileName).Skip(1);
        }

        /// <summary>
        /// Specific chart action for model type
        /// </summary>
        /// <param name="_chart">Chart to build</param>
        /// <param name="lines">Lines of data</param>
        public abstract void DataPull(ChartBase _chart, IEnumerable<string> lines);

        public abstract void OpenDataToNewGraph();

        public abstract void OpenDataToCurrentGraph();
    }
}

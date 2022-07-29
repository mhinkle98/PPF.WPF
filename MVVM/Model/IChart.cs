using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.Charting.Model.ChartSeries;

namespace PPF.WPF.MVVM.Model
{
    public interface IChart : IRenderableSeriesViewModel
    {
        string ChartName { get; set; }
    }
}

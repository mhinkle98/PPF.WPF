using System.Linq;

namespace PPF.WPF.MVVM.Model
{
    public static class DataCleaner
    {
        /*public static T CleanData<T>()
        {
            return T
        }*/
/*
        /// <summary>
        /// Cleaning for interferometry values
        /// </summary>
        /// <param name="chart">Chart data to clean</param>
        /// <param name="offset">How high of a jump is too much?</param>
        /// <returns></returns>
        public static InterferometryChart CleanInterferometryChart(InterferometryChart chart, int offset)
        {
            var ys = chart.ChartDataSeries.YValues;
            var range = chart.ChartDataSeries.YRange;
            var expected = (ys[0] + ys[1] + ys[2]) / 3;
            
            for (int i = 0; i < ys.Count - 1; i++)
            {
                if ((ys[i + 1] - expected) >= offset)
                {
                    
                }
            }
        }
        */
    }
    
}
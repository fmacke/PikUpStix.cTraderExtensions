using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Indicator
{
    //public class VolumeROCr : IIndicator
    //{
    //    public static List<double> CalculateVolumeROC(List<HistoricalData> timeseries, int period)
    //    {
    //        List<double> rocValues = new List<double>();

    //        for (int i = 0; i < timeseries.Count; i++)
    //        {
    //            if (i < period)
    //            {
    //                rocValues.Add(double.NaN); // Not enough data
    //            }
    //            else
    //            {
    //                double currentVolume = timeseries[i].Volume;
    //                double previousVolume = timeseries[i - period].Volume;
    //                double roc = ((currentVolume - previousVolume) / previousVolume) * 100;
    //                rocValues.Add(roc);
    //            }
    //        }
    //        return rocValues;
    //    }
    //}
}

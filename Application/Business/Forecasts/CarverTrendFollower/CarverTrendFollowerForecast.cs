using Application.BackTest;
using Application.Business.Forecasts;
using Application.Business.Forecasts.CarverTrendFollower;
using Domain.Entities;

namespace Application.Business.Forecasts.CarverTrendFollower
{
    public class CarverTrendFollowerForecast : IForecastHandler
    {
        /// <summary>
        /// This is Robert Carver's trend following strategy, plain and simple.
        /// </summary>
        /// <param name="historicalDataSets"></param>
        /// <param name="cursorDate"></param>
        /// <returns></returns>
        public List<IForecastValue> GetForecasts(IEnumerable<List<HistoricalData>> historicalDataSets, DateTime cursorDate, Logger logger, double askingPrice, double biddingPrice,
           List<Test_Parameters> testParameters)
        {
            var forecasts = new List<IForecastValue>();
            foreach (var forecast in from historicalDataSet in historicalDataSets
                                     where historicalDataSet.Count > 0
                                     select new EwmacForecastValue(cursorDate, historicalDataSet, askingPrice, biddingPrice, testParameters))
            {
                forecast.CalculateForecast();
                forecasts.Add(forecast);
            }
            return forecasts;
        }
    }
}
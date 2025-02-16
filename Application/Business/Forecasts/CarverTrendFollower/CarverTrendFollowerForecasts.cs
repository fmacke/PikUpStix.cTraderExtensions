using Application.BackTest;
using Application.Business.Market;
using Domain.Entities;

namespace Application.Business.Forecasts.CarverTrendFollower
{
    public static class CarverTrendFollowerForecasts //: IForecastHandler
    {
        /// <summary>
        /// This is Robert Carver's trend following strategy, plain and simple.
        /// </summary>
        public static List<IForecastValue> GetForecasts(IEnumerable<IMarketInfo> marketInfos, Logger logger, List<Test_Parameter> testParameters)
        {
            var forecasts = new List<IForecastValue>();
            foreach (var forecast in from marketInfo in marketInfos
                select new EwmacForecastValue(marketInfo, testParameters))
                {
                    forecast.CalculateForecast();
                    forecasts.Add(forecast);
                }
            return forecasts;
        }
    }
}
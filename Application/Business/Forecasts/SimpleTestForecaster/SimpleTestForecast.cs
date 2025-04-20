using Application.Business.Forecasts;
using Application.Business.Forecasts.SimpleTestForecaster;
using Application.Business.Market;
using Domain.Entities;

namespace PikUpStix.Trading.Forecast.SimpleTestForecaster
{
    public class SimpleTestForecast 
    {
        public List<IForecastValue> GetForecasts(List<IMarketInfo> marketInfos,
           List<Test_Parameter> testParameters)
        {
            var forecasts = new List<IForecastValue>();
            foreach (var forecast in from marketInfo in marketInfos
                                     select new SimpleTestForecastValue(marketInfo))
            {
                forecast.CalculateForecast();
                forecasts.Add(forecast);
            }
            return forecasts;
        }
    }
}

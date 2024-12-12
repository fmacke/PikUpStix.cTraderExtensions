using Application.BackTest;
using Application.Business.Forecasts;
using Application.Business.Forecasts.SimpleTestForecaster;
using Domain.Entities;

namespace PikUpStix.Trading.Forecast.SimpleTestForecaster
{
    public class SimpleTestForecast : IForecastHandler
    {
        public List<IForecastValue> GetForecasts(IEnumerable<List<HistoricalData>> historicalDataSets, DateTime cursorDate, Logger logger, 
           double askingPrice, double biddingPrice, List<Test_Parameter> testParameters)
        {
            var forecasts = new List<IForecastValue>();
            foreach (var forecast in from historicalDataSet in historicalDataSets
                                     where historicalDataSet.Count > 0
                                     select new SimpleTestForecastValue(cursorDate, historicalDataSet, askingPrice, biddingPrice))
            {
                forecast.CalculateForecast();
                forecasts.Add(forecast);
            }
            return forecasts;
        }
    }
}

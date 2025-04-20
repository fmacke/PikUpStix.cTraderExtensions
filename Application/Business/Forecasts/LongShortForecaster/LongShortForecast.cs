using Application.Business.Market;
using Domain.Entities;


namespace Application.Business.Forecasts.LongShortForecaster
{
    public class LongShortForecast //: IForecastHandler
    {
        public List<IForecastValue> GetForecasts(List<IMarketInfo> historicalDataSets,// Logger logger,
            List<Test_Parameter> testParameters)
        {
            var forecasts = new List<IForecastValue>();
            foreach (var forecast in from historicalDataSet in historicalDataSets
                                     select new LongShortForecastValue(historicalDataSet))
            {
                forecast.CalculateForecast();
                forecasts.Add(forecast);
            }
            return forecasts;
        }

        public List<IForecastValue> GetForecasts(IEnumerable<List<HistoricalData>> historicalDataSets, DateTime cursorDate, double askingPrice, double biddingPrice, List<Domain.Entities.Test_Parameter> testParameters)
        {
            throw new NotImplementedException();
        }
    }
}

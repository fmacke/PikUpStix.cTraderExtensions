using Domain.Entities;

namespace PikUpStix.Trading.Forecast
{
    public interface IForecastHandler
    {
        List<IForecastValue> GetForecasts(IEnumerable<List<HistoricalData>> historicalDataSets, DateTime cursorDate, double askingPrice, double biddingPrice, List<Test_Parameters> testParameters);
    }
}
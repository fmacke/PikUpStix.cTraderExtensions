using Application.BackTest;
using Domain.Entities;

namespace Application.Business.Forecasts
{
    public interface IForecastHandler
    {
        List<IForecastValue> GetForecasts(IEnumerable<List<HistoricalData>> historicalDataSets, DateTime cursorDate, Logger logger, double askingPrice, double biddingPrice, List<Test_Parameters> testParameters);
    }
}
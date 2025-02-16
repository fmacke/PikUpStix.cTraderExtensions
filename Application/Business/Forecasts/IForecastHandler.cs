using Application.BackTest;
using Domain.Entities;

namespace Application.Business.Forecasts
{
    public interface IForecastHandler
    {
        List<IForecastValue> GetForecasts(List<IMarketInfo> historicalDataSets, Logger logger, List<Test_Parameter> testParameters);
    }
}
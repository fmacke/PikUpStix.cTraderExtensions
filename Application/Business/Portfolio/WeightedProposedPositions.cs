using Application.Business.Forecasts;
using Domain.Entities;

namespace Application.Business.Portfolio
{
    public class WeightedProposedPositions : List<PositionValue>
    {
        public WeightedProposedPositions(List<IForecastValue> instrumentForecasts, decimal stopLossPercent, decimal exchangeRate, decimal targetVolatility,
            List<List<HistoricalData>> historicalDataSets, decimal availableTradingCapital)
        {
            foreach (IForecastValue forecast in instrumentForecasts)
            {
                var list = new List<HistoricalData>();
                foreach (var historicalDataSet in historicalDataSets)
                {
                    // TODO - BEWARE - CURRENTLY NO WAY TO ESTABLISH WHAT INSTRUMENT THE DIFFERENT HISTORICALDATASETS ARE FROM
                    //if (historicalDataSet.Any(x => x.InstrumentId == forecast.Instrument.Id))
                    //{
                        list = historicalDataSet;
                    //}
                }
                var pos = new PositionValue(forecast, stopLossPercent, exchangeRate, targetVolatility, list, availableTradingCapital);
                Add(pos);
            }
        }
    }
}
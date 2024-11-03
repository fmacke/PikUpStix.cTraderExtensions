using Domain.Entities;
using PikUpStix.Trading.Forecast;

namespace Application.Portfolio
{
    public class WeightedProposedPositions : List<PositionValue>
    {//flagged
        public WeightedProposedPositions(List<IForecastValue> instrumentForecasts, decimal stopLossPercent, decimal exchangeRate, decimal targetVolatility,
            List<List<HistoricalData>> historicalDataSets, decimal availableTradingCapital)
        {
            foreach (IForecastValue forecast in instrumentForecasts)
            {
                var list = new List<HistoricalData>();

                foreach (var historicalDataSet in historicalDataSets)
                {
                    if (historicalDataSet.Any(x => x.InstrumentId == forecast.Instrument.InstrumentId))
                    {
                        list = historicalDataSet;
                    }
                }
                Add(new PositionValue(forecast, stopLossPercent, exchangeRate, targetVolatility, list, availableTradingCapital));
            }
        }
    }
}
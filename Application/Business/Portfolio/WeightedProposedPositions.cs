using Application.Business.Forecasts;
using Application.Business.Market;

namespace Application.Business.Portfolio
{
    public class WeightedProposedPositions : List<PositionValue>
    {
        public WeightedProposedPositions(List<IForecastValue> instrumentForecasts, 
            double stopLossPercent, double exchangeRate, double targetVolatility,
            List<IMarketInfo> marketInfos)
        {
            foreach (IForecastValue forecast in instrumentForecasts)
            {
                foreach (var marketInfo in marketInfos)
                {
                    if (forecast.SymbolName == marketInfo.SymbolName)
                    {
                        var pos = new PositionValue(forecast, stopLossPercent, exchangeRate, 
                            targetVolatility, marketInfo.Bars, marketInfo.CurrentCapital, marketInfo.SymbolName);
                        Add(pos);
                    }
                }
            }
        }
    }
}
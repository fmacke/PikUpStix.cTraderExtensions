using Domain.Entities;

namespace PikUpStix.Trading.Forecast
{
    public interface IStopLossHandler
    {
        List<TestTrade> CloseStoppedOutPositions(List<TestTrade> existingPositions, DateTime cursorDate, List<List<HistoricalData>> historicalDataSets, decimal exchangeRate, decimal startingTradingCapital);
    }
}
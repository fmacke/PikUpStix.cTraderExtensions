using Domain.Entities;

namespace PikUpStix.Trading.Forecast
{
    public interface IStopLossHandler
    {
        List<Position> CloseStoppedOutPositions(List<Position> existingPositions, DateTime cursorDate, List<List<HistoricalData>> historicalDataSets, decimal exchangeRate, decimal startingTradingCapital);
    }
}
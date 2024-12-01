using Domain.Entities;

namespace PikUpStix.Trading.Forecast
{
    public interface IStopLossHandler
    {
        List<Test_Trades> CloseStoppedOutPositions(List<Test_Trades> existingPositions, DateTime cursorDate, List<List<HistoricalData>> historicalDataSets, decimal exchangeRate, decimal startingTradingCapital);
    }
}
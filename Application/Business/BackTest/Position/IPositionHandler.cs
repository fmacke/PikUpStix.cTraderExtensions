using Application.Business.Portfolio;
using Domain.Entities;
using PikUpStix.Trading.Forecast;

namespace Application.Business.BackTest.Position
{
    public interface IPositionHandler
    {
        List<TestTrade> GetUpdatedPositions(TradingSystemParams paramsa, int testId, List<TestTrade> existingPositions, decimal currentMargin, DateTime cursorDate, WeightedProposedPositions weightedPositions,
            List<List<HistoricalData>> historicalDataSets, IStopLossCreator stopLossCreator);
    }
}
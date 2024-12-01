using Application.Business.Portfolio;
using Domain.Entities;
using PikUpStix.Trading.Forecast;

namespace Application.Business.BackTest.Position
{
    public interface IPositionHandler
    {
        List<Test_Trades> GetUpdatedPositions(TradingSystemParams paramsa, int testId, List<Test_Trades> existingPositions, decimal currentMargin, DateTime cursorDate, WeightedProposedPositions weightedPositions,
            List<List<HistoricalData>> historicalDataSets, IStopLossCreator stopLossCreator);
    }
}
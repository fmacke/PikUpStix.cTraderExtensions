using Application.Business.Portfolio;
using Domain.Entities;
using PikUpStix.Trading.Forecast;

namespace Application.Business.BackTest.Position
{
    public interface IPositionHandler
    {
        List<Domain.Entities.Position> GetUpdatedPositions(TradingSystemParams paramsa, int testId, List<Domain.Entities.Position> existingPositions, double currentMargin, DateTime cursorDate, WeightedProposedPositions weightedPositions,
            List<List<HistoricalData>> historicalDataSets, IStopLossCreator stopLossCreator);
    }
}
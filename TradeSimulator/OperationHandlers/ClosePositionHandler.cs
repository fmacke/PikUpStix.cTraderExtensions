using Application.Business.Calculations;
using Domain.Entities;
using Domain.Enums;

namespace TradeSimulator.Business
{
    internal class ClosePositionHandler
    {
        private List<Position> openPositions;
        private List<Position> closedTrades;
        public ClosePositionHandler(ref List<Position> openPositions, ref List<Position> closedTrades)
        {
            this.openPositions = openPositions;
            this.closedTrades = closedTrades;
        }
        public void ClosePosition(Position position, double closePrice, DateTime closedAt, double contractUnit, double exchangeRate)
        {
            position.ClosePrice = closePrice;
            position.Status = PositionStatus.CLOSED;
            position.ClosedAt = closedAt;
            position.Margin = Margin.Calculate(contractUnit, exchangeRate, position, closePrice, position.Volume);
            closedTrades.Add(position);
            openPositions.Remove(position);
            return;
        }
    }
}
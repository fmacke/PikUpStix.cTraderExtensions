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
        public void ClosePosition(Position position, double? closePrice, DateTime? closedAt)
        {
            if (closePrice.HasValue && closedAt.HasValue)
            {
                position.ClosePrice = closePrice;
                position.Status = PositionStatus.CLOSED;
                position.ClosedAt = closedAt;
                closedTrades.Add(position);
                openPositions.Remove(position);
                return;
            }
            throw new Exception("Close price is required to close a position");
        }
    }
}
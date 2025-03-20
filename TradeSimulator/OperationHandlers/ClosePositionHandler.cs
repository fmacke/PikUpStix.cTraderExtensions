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
        public void ClosePosition(Position position, double? closeAt)
        {
            if (closeAt.HasValue)
            {
                position.ClosePrice = closeAt;
                position.Status = PositionStatus.CLOSED;
                position.ClosedAt = DateTime.Now;
                closedTrades.Add(position);
                openPositions.Remove(position);
                return;
            }
            throw new Exception("Close price is required to close a position");
        }
    }
}
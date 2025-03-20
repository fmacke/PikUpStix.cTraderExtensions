using Domain.Entities;
using Domain.Enums;

namespace TradeSimulator.Business
{
    internal class StopLossHandler
    {
        private double openPrice;
        private List<Position> openPositions;
        private List<Position> closePositions;

        public StopLossHandler(double openPrice, ref List<Position> openPositions, ref List<Position> closedTrades)
        {
            this.openPrice = openPrice;
            this.openPositions = openPositions;
        }
        public void CloseOutStops()
        {
            foreach (var position in openPositions)
            {
                if (position.StopLoss.HasValue && StopLossHit(position, openPrice))
                {
                    new ClosePositionHandler(ref openPositions, ref closePositions).ClosePosition(position, position.StopLoss);
                }
            }
        }
        private bool StopLossHit(Position position, double currentPrice)
        {
            if (position.PositionType == PositionType.BUY && currentPrice <= position.StopLoss)
                return true;
            if (position.PositionType == PositionType.SELL && currentPrice >= position.StopLoss)
                return true;
            return false;
        }
    }
}
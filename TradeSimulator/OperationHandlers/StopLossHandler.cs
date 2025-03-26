using Domain.Entities;
using Domain.Enums;

namespace TradeSimulator.Business
{
    internal class StopLossHandler
    {
        private double openPrice;
        private List<Position> openPositions;
        private List<Position> closePositions;
        private DateTime? cursorDate;
        private double exchangeRate;
        private double contractUnit;

        public StopLossHandler(DateTime? cursorDate, double openPrice, ref List<Position> openPositions, ref List<Position> closedTrades, double exchangeRate, double contractUnit)
        {
            this.openPrice = openPrice;
            this.openPositions = openPositions;
            this.closePositions = closedTrades;
            this.cursorDate = cursorDate;
            this.exchangeRate = exchangeRate;
            this.contractUnit = contractUnit;
        }
        public void CloseOutStops()
        {
            var positionsToClose = openPositions.FindAll(p => p.StopLoss.HasValue && StopLossHit(p, openPrice));
            foreach (var position in positionsToClose)
                    new ClosePositionHandler(ref openPositions, ref closePositions).ClosePosition(position, Convert.ToDouble(position.StopLoss), Convert.ToDateTime(cursorDate), contractUnit, exchangeRate);
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
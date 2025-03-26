using Application.Business.Market;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
    public class StopLossHandler
    {
        private double openPrice;
        private List<Position> openPositions;
        private List<Position> closePositions;
        private DateTime? cursorDate;

        private List<IMarketInfo> marketInfo;

        public StopLossHandler(DateTime? cursorDate, double openPrice, ref List<Position> openPositions, ref List<Position> closedTrades, List<IMarketInfo> marketInfo)
        {
            this.openPrice = openPrice;
            this.openPositions = openPositions;
            closePositions = closedTrades;
            this.cursorDate = cursorDate;
            this.marketInfo = marketInfo;
        }
        public void CloseOutStops()
        {
            var positionsToClose = openPositions.FindAll(p => p.StopLoss.HasValue && StopLossHit(p, openPrice));
            foreach (var position in positionsToClose)
            {
                var exchangeRate = marketInfo.Find(m => m.SymbolName == positionsToClose[0].SymbolName).ExchangeRate;
                var contractUnit = marketInfo.Find(m => m.SymbolName == positionsToClose[0].SymbolName).ContractUnit;
                new ClosePositionHandler(ref openPositions, ref closePositions).ClosePosition(position, Convert.ToDouble(position.StopLoss), Convert.ToDateTime(cursorDate), contractUnit, exchangeRate);
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
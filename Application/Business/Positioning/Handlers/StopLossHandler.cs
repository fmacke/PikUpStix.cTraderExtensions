using Application.Business.Market;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
    public class StopLossHandler
    { 
        private List<Position> positions;
        private DateTime cursorDate;
        private List<IMarketInfo> marketInfos;

        public StopLossHandler(DateTime cursorDate, ref List<Position> positions, List<IMarketInfo> marketInfos)
        {
            this.positions = positions;;
            this.cursorDate = cursorDate;
            this.marketInfos = marketInfos;
        }   
        public void CloseOutStops()
        {
            foreach (var marketInfo in marketInfos)
            {
                var positionsToClose = positions
                    .Where(p => p.Status == PositionStatus.OPEN
                        && p.StopLoss.HasValue
                        && StopLossHit(p, marketInfo)
                        && p.SymbolName == marketInfo.SymbolName
                        && p.ClosedAt == null);

                var closeHandler = new ClosePositionHandler(ref positions); 

                foreach (var position in positionsToClose)
                {
                    position.Comment = $"Stop Loss hit at {marketInfo.Ask} on {cursorDate}";
                    closeHandler.ClosePosition(
                        position,
                        position.StopLoss.GetValueOrDefault(),
                        Convert.ToDateTime(cursorDate),
                        marketInfo.LotSize,
                        marketInfo.PipSize,
                        marketInfo.ExchangeRate);
                }
            }
        }
        private bool StopLossHit(Position position, IMarketInfo marketInfo)
        {
            var maxPriceExcursion = GetMaxPriceExcursion(position, marketInfo);
            if (position.PositionType == PositionType.BUY && maxPriceExcursion <= position.StopLoss)
                return true;
            if (position.PositionType == PositionType.SELL && maxPriceExcursion >= position.StopLoss)
                return true;
            return false;
        }
        private double? GetMaxPriceExcursion(Position position, IMarketInfo marketInfo)
        {
            if (position.PositionType == PositionType.BUY)                
                return marketInfo.Ask < marketInfo.LastBar.LowPrice ? marketInfo.Ask : marketInfo.LastBar.LowPrice;  // FOR BUY POSITIONS
            return marketInfo.Ask > marketInfo.LastBar.HighPrice ? marketInfo.Ask : marketInfo.LastBar.HighPrice;  // FOR SELL POSITIONS

        }
    }
}
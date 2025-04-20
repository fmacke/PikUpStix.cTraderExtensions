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
                    closeHandler.ClosePosition(
                        position,
                        position.StopLoss.GetValueOrDefault(),
                        Convert.ToDateTime(cursorDate),
                        marketInfo.ContractUnit,
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
            if (position.Created < marketInfo.CursorDate) //  Position created at some point during the last bar - tricky to know if the stop loss was hit without looking at more refined timeframes/tick data
                return marketInfo.CurrentBar.OpenPrice;
            if (position.PositionType == PositionType.BUY)                
                return marketInfo.CurrentBar.OpenPrice < marketInfo.LastBar.LowPrice ? marketInfo.CurrentBar.OpenPrice : marketInfo.LastBar.LowPrice;  // FOR BUY POSITIONS
            return marketInfo.CurrentBar.OpenPrice < marketInfo.LastBar.LowPrice ? marketInfo.CurrentBar.OpenPrice : marketInfo.LastBar.LowPrice;  // FOR SELL POSITIONS

        }
    }
    public class TakeProfitHandler
    {
        private List<Position> positions;
        private DateTime cursorDate;
        private List<IMarketInfo> marketInfos;

        public TakeProfitHandler(DateTime cursorDate, ref List<Position> positions, List<IMarketInfo> marketInfos)
        {
            this.positions = positions; ;
            this.cursorDate = cursorDate;
            this.marketInfos = marketInfos;
        }
        public void CloseOutTakeProfits()
        {
            foreach (var marketInfo in marketInfos)
            {
                var positionsToClose = positions
                    .Where(p => p.Status == PositionStatus.OPEN
                        && p.TakeProfit.HasValue
                        && TakeProfitHit(p, marketInfo)
                        && p.SymbolName == marketInfo.SymbolName
                        && p.ClosedAt == null);

                var closeHandler = new ClosePositionHandler(ref positions);

                foreach (var position in positionsToClose)
                {
                    closeHandler.ClosePosition(
                        position,
                        position.TakeProfit.GetValueOrDefault(),
                        Convert.ToDateTime(cursorDate),
                        marketInfo.ContractUnit,
                        marketInfo.ExchangeRate);
                }
            }
        }
        private bool TakeProfitHit(Position position, IMarketInfo marketInfo)
        {
            var maxPriceExcursion = GetMaxPriceExcursion(position, marketInfo);
            if (position.PositionType == PositionType.BUY && maxPriceExcursion >= position.TakeProfit)
                return true;
            if (position.PositionType == PositionType.SELL && maxPriceExcursion <= position.StopLoss)
                return true;
            return false;
        }
        private double? GetMaxPriceExcursion(Position position, IMarketInfo marketInfo)
        {
            if (position.Created < marketInfo.CursorDate) //  Position created at some point during the last bar - tricky to know if the stop loss was hit without looking at more refined timeframes/tick data
                return marketInfo.CurrentBar.OpenPrice;
            if (position.PositionType == PositionType.BUY)
                return marketInfo.CurrentBar.OpenPrice < marketInfo.LastBar.HighPrice ? marketInfo.CurrentBar.HighPrice : marketInfo.LastBar.OpenPrice;  // FOR BUY POSITIONS
            //SELL
            return marketInfo.CurrentBar.OpenPrice < marketInfo.LastBar.LowPrice ? marketInfo.CurrentBar.LowPrice : marketInfo.LastBar.OpenPrice;  // FOR SELL POSITIONS

        }
    }
}
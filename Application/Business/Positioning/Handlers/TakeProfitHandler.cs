﻿using Application.Business.Market;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
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
                    position.Comment = "Take Profit hit at " + cursorDate.ToString("yyyy-MM-dd HH:mm:ss");
                    closeHandler.ClosePosition(
                        position,
                        position.TakeProfit.GetValueOrDefault(),
                        Convert.ToDateTime(cursorDate),
                        marketInfo.LotSize,
                        marketInfo.PipSize,
                        marketInfo.ExchangeRate);
                }
            }
        }
        private bool TakeProfitHit(Position position, IMarketInfo marketInfo)
        {
            var maxPriceExcursion = GetMaxPriceExcursion(position, marketInfo);
            if (position.PositionType == PositionType.BUY && maxPriceExcursion >= position.TakeProfit)
                return true;
            if (position.PositionType == PositionType.SELL && maxPriceExcursion <= position.TakeProfit)
                return true;
            return false;
        }
        private double? GetMaxPriceExcursion(Position position, IMarketInfo marketInfo)
        {
            if (position.PositionType == PositionType.BUY)
                return marketInfo.Ask > marketInfo.LastBar.HighPrice ? marketInfo.Ask : marketInfo.LastBar.HighPrice;  // FOR BUY POSITIONS
            return marketInfo.Ask < marketInfo.LastBar.LowPrice? marketInfo.Ask : marketInfo.LastBar.LowPrice;   // FOR SELL POSITIONS
        }
    }
}
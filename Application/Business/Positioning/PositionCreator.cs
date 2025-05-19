using Application.Business.Market;
using Domain.Enums;
using Domain.Entities;
using Application.Business.Calculations;

namespace Application.Business.Positioning
{
    public static class PositionCreator
    {
        public static Position CreatePosition(PositionType positionType, double forecast, double maximumRiskPercentage, 
            double stopLossAmount, double? takeProfitAmount, IMarketInfo marketInfo, DateTime? expiration)
        {
            double stopLoss = 0;
            if (stopLossAmount > 0)
                stopLoss = positionType == PositionType.BUY ? marketInfo.Ask - stopLossAmount : marketInfo.Ask + stopLossAmount;
            var stopLossPrice = marketInfo.Ask - stopLoss;
            var positionSize = new PositionSizer(forecast,
                                                maximumRiskPercentage,
                                                marketInfo.CurrentCapital,
                                                marketInfo.LotSize,
                                                marketInfo.PipSize,
                                                stopLossPrice,
                                                marketInfo.Ask).Calculate();
            
            var position = new Position()
            {
                SymbolName = marketInfo.SymbolName,
                PositionType = positionType,
                EntryPrice = marketInfo.Ask,
                StopLoss = stopLoss,
                
                InstrumentId = marketInfo.InstrumentId,
                Volume = positionSize,
                Created = marketInfo.CursorDate
            };
            if(takeProfitAmount.HasValue)
                position.TakeProfit = positionType == PositionType.BUY ? marketInfo.Ask + takeProfitAmount : marketInfo.Ask - takeProfitAmount;
            if (expiration.HasValue)
                position.ExpirationDate = expiration.Value;
            return position;
        }

    }
}

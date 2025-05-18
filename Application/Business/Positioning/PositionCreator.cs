using Application.Business.Market;
using Domain.Enums;
using Domain.Entities;
using Application.Business.Calculations;

namespace Application.Business.Positioning
{
    public static class PositionCreator
    {
        public static Position CreatePosition(PositionType positionType, double forecast, double maximumRiskPercentage, 
            double stopLossInPips, double takeProfitInPips, IMarketInfo marketInfo, DateTime? expiration)
        {
            var stopLoss = marketInfo.Ask - (marketInfo.Ask * stopLossInPips); // Example stop loss calculation
            var stopLossPrice = marketInfo.Ask - stopLoss;
            var takeProfit = marketInfo.Ask + (marketInfo.Ask * takeProfitInPips); // Example stop loss calculation                    
            var positionSize = new PositionSizer(forecast,
                                                maximumRiskPercentage,
                                                marketInfo.CurrentCapital,
                                                marketInfo.PipSize,
                                                marketInfo.LotSize,
                                                stopLossPrice,
                                                marketInfo.Ask).Calculate();
            
            var position = new Position()
            {
                SymbolName = marketInfo.SymbolName,
                PositionType = positionType,
                EntryPrice = marketInfo.Ask,
                StopLoss = stopLoss,
                TakeProfit = takeProfit,
                InstrumentId = marketInfo.InstrumentId,
                Volume = positionSize,
                Created = marketInfo.CursorDate
            };
            if (expiration.HasValue)
            {
                position.ExpirationDate = expiration.Value;
            }
            return position;
        }

    }
}

using Application.Business.Market;
using Domain.Enums;
using Domain.Entities;
using Application.Business.Calculations;

namespace Application.Business.Positioning
{
    public static class PositionCreator
    {
        public static Position CreatePosition(PositionType positionType, double forecast, double maximumRiskPercentage, 
            double? stopLossAmount, double? takeProfitAmount, IMarketInfo marketInfo, DateTime? expiration)
        {
            double positionSize = 0.0;
            double stopLossPrice = 0;
            positionSize = GetPositionSize(positionType, forecast, maximumRiskPercentage, stopLossAmount, marketInfo, ref stopLossPrice);
            var position = new Position()
            {
                SymbolName = marketInfo.SymbolName,
                PositionType = positionType,
                EntryPrice = marketInfo.Ask,
                InstrumentId = marketInfo.InstrumentId,
                Volume = positionSize,
                Created = marketInfo.CursorDate
            };
            if (stopLossAmount.HasValue)
                position.StopLoss = stopLossPrice;
            if (takeProfitAmount.HasValue)
                position.TakeProfit = positionType == PositionType.BUY ? marketInfo.Ask + takeProfitAmount : marketInfo.Ask - takeProfitAmount;
            if (expiration.HasValue)
                position.ExpirationDate = expiration.Value;
            return position;
        }

        private static double GetPositionSize(PositionType positionType, double forecast, double maximumRiskPercentage, double? stopLossAmount, IMarketInfo marketInfo, ref double stopLossPrice)
        {
            double positionSize;
            if (stopLossAmount.HasValue)
            {
                // Get position size based on stop loss amount
                if (stopLossAmount > 0)
                    stopLossPrice = positionType == PositionType.BUY ? marketInfo.Ask - Convert.ToDouble(stopLossAmount) : marketInfo.Ask + Convert.ToDouble(stopLossAmount);
                positionSize = new StopLossBasedPositionSizer(forecast,
                                                    maximumRiskPercentage,
                                                    marketInfo.CurrentCapital,
                                                    marketInfo.LotSize,
                                                    marketInfo.PipSize,
                                                    stopLossPrice,
                                                    marketInfo.Ask).Calculate();
            }
            else
                // Get position size based on capital allocation
                positionSize = new CapitalBasedPositionSizer(forecast,
                                                    maximumRiskPercentage,
                                                    marketInfo.CurrentCapital,
                                                    marketInfo.LotSize,
                                                    marketInfo.PipSize).Calculate();
            return positionSize;
        }
    }
}

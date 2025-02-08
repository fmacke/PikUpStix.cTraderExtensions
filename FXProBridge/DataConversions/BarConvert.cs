using Application.Business;
using cAlgo.API;
using Domain.Entities;
using Domain.Enums;

namespace FXProBridge.DataConversions
{
    public static class PendingOrderConvert
    {
        public static  Domain.Enums.TradeType ConvertTradeType(cAlgo.API.TradeType tradeType)
        {
            switch (tradeType)
            {
                case cAlgo.API.TradeType.Buy:
                    return Domain.Enums.TradeType.Buy;
                case cAlgo.API.TradeType.Sell:
                    return Domain.Enums.TradeType.Sell;
                default:
                    return Domain.Enums.TradeType.Buy;
            }
        }
        public static List<Application.Business.PendingOrder> ConvertOrders(cAlgo.API.PendingOrders pendingOrders)
        {
            var orders = new List<Application.Business.PendingOrder>();
            foreach (var order in pendingOrders)
            {
                orders.Add(new Application.Business.PendingOrder()
                {
                    TradeType = ConvertTradeType(order.TradeType),
                    VolumeInUnits = order.VolumeInUnits,
                    Id = order.Id,
                    //OrderType = ConvertTradeType(order.OrderType),
                    TargetPrice = order.TargetPrice,
                    ExpirationTime = order.ExpirationTime,
                    StopLoss = order.StopLoss,
                    StopLossPips = order.StopLossPips,
                    TakeProfit = order.TakeProfit,
                    TakeProfitPips = order.TakeProfitPips,
                    Label = order.Label,
                    Comment = order.Comment,
                    Quantity = order.Quantity,
                    HasTrailingStop = order.HasTrailingStop,
                    StopLimitRangePips = order.StopLimitRangePips,
                    SymbolName = order.SymbolName,
                    CurrentPrice = order.CurrentPrice,
                    DistancePips = order.DistancePips,
                    Channel = order.Channel,
                    SubmittedTime = order.SubmittedTime,
                    LastUpdateTime = order.LastUpdateTime,
                    //ProtectionType = order.ProtectionType
                });
            }
            return orders;
        }
    }
    public static class BarConvert
    {
        public static List<HistoricalData> ConvertBars(Bars historicalData, Instrument instrument)
        {
            var histoData = new List<HistoricalData>();
            return MapBars(historicalData, instrument.Id);
        }
        public static List<HistoricalData> ConvertBars(Bars historicalData)
        {
            return MapBars(historicalData, 1);  /// TODO: 1 is a placeholder for instrumentId
        }

        private static List<HistoricalData> MapBars(Bars historicalData, int instrumentId)
        {
            var histoData = new List<HistoricalData>();
            for (var x = historicalData.ClosePrices.Count - 1; x >= 0; x--)
            {
                histoData.Add(new HistoricalData()
                {
                    ClosePrice = historicalData.ClosePrices[x],
                    OpenPrice = historicalData.OpenPrices[x],
                    HighPrice = historicalData.HighPrices[x],
                    LowPrice = historicalData.LowPrices[x],
                    //Instrument = instrument,
                    Date = historicalData.OpenTimes[x],
                    //InstrumentId = instrumentId
                });
            }
            return histoData;
        }

    }
}

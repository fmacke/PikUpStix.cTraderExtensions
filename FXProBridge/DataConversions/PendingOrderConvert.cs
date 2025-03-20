namespace FXProBridge.DataConversions
{
    public static class PendingOrderConvert
    {
        public static  Domain.Enums.PositionType ConvertTradeType(cAlgo.API.TradeType tradeType)
        {
            switch (tradeType)
            {
                case cAlgo.API.TradeType.Buy:
                    return Domain.Enums.PositionType.BUY;
                case cAlgo.API.TradeType.Sell:
                    return Domain.Enums.PositionType.SELL;
                default:
                    return Domain.Enums.PositionType.BUY;
            }
        }
        public static List<Application.Business.PendingOrder> ConvertOrders(cAlgo.API.PendingOrders pendingOrders)
        {
            var orders = new List<Application.Business.PendingOrder>();
            foreach (var order in pendingOrders)
            {
                orders.Add(new Application.Business.PendingOrder()
                {
                    VolumeInUnits = order.VolumeInUnits,
                    Id = order.Id,
                    OrderType = ConvertTradeType(order.TradeType),
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
}

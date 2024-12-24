namespace FXProBridge.DataConversions
{
    public static class PositionConvert
    {
        public static Application.Business.Positions ConvertPosition(cAlgo.API.Positions positions)
        {
            var convertedPositions = new Application.Business.Positions();
            foreach (var position in positions)
            {
                convertedPositions.Add(new Application.Business.Position()
                {
                    SymbolName = position.SymbolName,
                    Volume = Convert.ToDecimal(position.VolumeInUnits),
                    StopLoss = Convert.ToDecimal(position.StopLoss),
                    TakeProfit = Convert.ToDecimal(position.TakeProfit),
                    EntryPrice = Convert.ToDecimal(position.EntryPrice),
                    Id = position.Id,
                    TradeType = ConvertTradeType(position.TradeType),
                });
            }
            return convertedPositions;
        }

        private static Domain.Enums.TradeType ConvertTradeType(cAlgo.API.TradeType tradeType)
        {
            if (tradeType == cAlgo.API.TradeType.Buy)
                return Domain.Enums.TradeType.Buy;
            return Domain.Enums.TradeType.Sell;
        }
    }
}

using Domain.Entities;
namespace FXProBridge.DataConversions
{
    public static class PositionConvert
    {
        public static new List<Position> ConvertPosition(cAlgo.API.Positions positions)
        {
            var convertedPositions = new List<Position>();
            foreach (var position in positions)
            {
                convertedPositions.Add(new Position()
                {
                    SymbolName = position.SymbolName,
                    Volume = position.VolumeInUnits,
                    StopLoss = position.StopLoss,
                    TakeProfit = position.TakeProfit,
                    EntryPrice = position.EntryPrice,
                    Id = position.Id,
                    PositionType = ConvertTradeType(position.TradeType),
                });
            }
            return convertedPositions;
        }

        private static Domain.Enums.PositionType ConvertTradeType(cAlgo.API.TradeType tradeType)
        {
            if (tradeType == cAlgo.API.TradeType.Buy)
                return Domain.Enums.PositionType.BUY;
            return Domain.Enums.PositionType.SELL;
        }
    }
}

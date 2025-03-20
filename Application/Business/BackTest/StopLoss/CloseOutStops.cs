using Domain.Entities;
using Domain.Enums;
using PikUpStix.Trading.Forecast;

namespace Application.Business.BackTest.StopLoss
{
    public class CloseOutStops : IStopLossHandler
    {
        public List<Domain.Entities.Position> TradingPositions { get; private set; }

        public List<Domain.Entities.Position> CloseStoppedOutPositions(List<Domain.Entities.Position> existingPositions, DateTime cursorDate, List<List<HistoricalData>> historicalDataSets, decimal exchangeRate, decimal startingTradingCapital)
        {
            TradingPositions = existingPositions;
            foreach (Domain.Entities.Position existingPosition in TradingPositions.Where(x => x.Status == PositionStatus.OPEN))
            {
                if (existingPosition.StopLoss <= 0) continue;
                var history = new RecentHistory(historicalDataSets, existingPosition.InstrumentId, cursorDate);
                //if (!history.HasValidData) continue;
                //var previousDayLow = history.MostRecentHistoricalTick.LowPrice;
                //var previousDayHigh = history.MostRecentHistoricalTick.HighPrice;

                //if (BuyStopLossHit(existingPosition, previousDayLow) || SellStopLossHit(existingPosition, previousDayHigh))
                //{
                //    existingPosition.ClosePrice = existingPosition.StopLoss;
                //    existingPosition.ClosedAt = cursorDate;
                //    existingPosition.Status = PositionStatus.HISTORICALTRADE.ToString();
                //    existingPosition.Comment = "STOP LOSS HIT";
                //    existingPosition.Margin =
                //        Margin.Calculate(existingPosition.Instrument.ContractUnit, exchangeRate,
                //            existingPosition, existingPosition.StopLoss, existingPosition.Volume);
                //    existingPosition.CapitalAtClose = TradingPositions.Sum(x => x.Margin) + startingTradingCapital;
                //}
            }
            return TradingPositions;
        }

        //private bool BuyStopLossHit(TestTrade existingPosition, decimal? previousDayLow)
        //{
        //    return existingPosition.Direction == PositionType.BUY.ToString() && previousDayLow <= existingPosition.StopLoss;
        //}

        //private bool SellStopLossHit(TestTrade existingPosition, decimal? previousDayHigh)
        //{
        //    return existingPosition.Direction == PositionType.SELL.ToString() && previousDayHigh >= existingPosition.StopLoss;
        //}
    }
}

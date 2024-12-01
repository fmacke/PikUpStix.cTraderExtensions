using Application.Business.Portfolio;
using Domain.Entities;
using Domain.Enums;
using PikUpStix.Trading.Forecast;

namespace Application.Business.BackTest.Position
{
    public class AdjustPositionWithMinimumForecastRequiredForPositionReversals : AdjustPositions
    {
        private decimal MaxMinForecast;
        public AdjustPositionWithMinimumForecastRequiredForPositionReversals(decimal maxMinForecast)
        {
            MaxMinForecast = maxMinForecast;
        }

        public override void OpenTrade(DateTime cursorDate, PositionValue weightedProposedPosition, decimal volume, decimal stopLoss)
        {
            if (weightedProposedPosition.ForecastValue.Forecast >= MaxMinForecast || weightedProposedPosition.ForecastValue.Forecast <= MaxMinForecast * -1)
                base.OpenTrade(cursorDate, weightedProposedPosition, volume, stopLoss);
        }
    }
    public class AdjustPositions : IPositionHandler
    {
        private TradingSystemParams Parameters;
        private int TestId;
        private decimal CurrentMargin;
        private IStopLossCreator StopLossCreator;
        private List<Test_Trades> Trades;

        public List<Test_Trades> GetUpdatedPositions(TradingSystemParams paramsa, int testId, List<Test_Trades> existingPositions, decimal currentMargin, DateTime cursorDate, WeightedProposedPositions weightedPositions,
            List<List<HistoricalData>> historicalDataSets, IStopLossCreator stopLossHandler)
        {
            StopLossCreator = stopLossHandler;
            Parameters = paramsa;
            TestId = testId;
            Trades = existingPositions;
            CurrentMargin = currentMargin;
            foreach (PositionValue proposedPosition in weightedPositions)
                AdjustInstrumentPositions(cursorDate, proposedPosition);
            return Trades;
        }

        public List<Test_Trades> GetAdjustedTrades()
        {
            return Trades;
        }

        private void AdjustInstrumentPositions(DateTime cursorDate, PositionValue proposedPosition)
        {
            var proposedPositionSize = proposedPosition.ProposedWeightedPosition;
            var currentPositionSize = Trades.Where(x => x.Status == PositionStatus.POSITION.ToString() && x.InstrumentId == proposedPosition.Instrument.Id).Sum(y => y.Volume);

            //var positionAdjuster = new PositionCalculator(proposedPositionSize, Trades.Where(x => x.Status == PositionStatus.POSITION.ToString() && x.InstrumentId == proposedPosition.Instrument.InstrumentId).ToList());
            //var newProposedPositions = positionAdjuster.RevisedPositions;


            // Close Trades
            foreach (var position in Trades.Where(x => x.Status == PositionStatus.POSITION.ToString() && x.InstrumentId == proposedPosition.Instrument.Id))
            {  //if (!positionAdjuster.RevisedPositions.Any(x => x == position))
                CloseTrade(position, cursorDate, proposedPosition, Convert.ToDecimal(proposedPosition.AskingPrice));
            }


            Trades = StopLossCreator.CalculateStops(Trades, Parameters.StopLossPercent, proposedPosition.AvailableTradingCapital,
                proposedPositionSize, Parameters.ExchangeRate, proposedPosition);
            // Open Trades
            //if (positionAdjuster.RequiredAdjustmentToVolume != 0)
            if (proposedPositionSize != 0)
                OpenTrade(cursorDate, proposedPosition, proposedPositionSize, StopLossCreator.NewPositionStopLoss);
        }

        public virtual void OpenTrade(DateTime cursorDate, PositionValue weightedProposedPosition, decimal volume, decimal stopLoss)
        {
            string direction = volume < 0
                ? PositionType.SELL.ToString()
                : PositionType.BUY.ToString();

            var position = new Test_Trades
            {
                TestId = TestId,
                InstrumentId = weightedProposedPosition.Instrument.Id,
                Created = cursorDate,
                Direction = direction.Trim(),
                EntryPrice = Convert.ToDecimal(weightedProposedPosition.AskingPrice),
                Status = PositionStatus.POSITION.ToString(),
                Volume = volume,
                Instrument = weightedProposedPosition.Instrument,
                TakeProfit = 0,
                StopLoss = stopLoss,
                Commission = 0,
                Comment = "None",
                ClosePrice = 0,
                TrailingStop = 0,
                Margin = 0.0M,
                InstrumentWeight = "None",
                ForecastAtClose = 0.0M,
                ForecastAtEntry = weightedProposedPosition.ForecastValue.Forecast,
                CapitalAtClose = 0.0M,
                CapitalAtEntry = CurrentMargin
            };
            Trades.Add(position);
        }

        private void CloseTrade(Test_Trades trade, DateTime cursorDate, PositionValue weightedProposedPosition, decimal currentPrice)
        {
            foreach (
                Test_Trades existingPosition in
                    Trades.Where(x => x == trade && x.Status == PositionStatus.POSITION.ToString()))
            {
                existingPosition.ClosePrice = currentPrice;
                existingPosition.ClosedAt = cursorDate;
                existingPosition.ForecastAtClose = weightedProposedPosition.ForecastValue.Forecast;

                existingPosition.Status = PositionStatus.HISTORICALTRADE.ToString();
                existingPosition.Comment = "Position closed due to volume change.  From existing volume of " + trade.Volume + " to new volume of " + weightedProposedPosition.ProposedWeightedPosition;
                existingPosition.Margin =
                    Reports.Margin.Calculate(existingPosition.Instrument.ContractUnit, Parameters.ExchangeRate, trade,
                        currentPrice, trade.Volume);
                CurrentMargin += existingPosition.Margin;
                existingPosition.CapitalAtClose = CurrentMargin + existingPosition.Margin;
            }
        }
    }
}

using Application.Business.BackTest.Position;
using Application.Business.Calculations;
using Application.Business.Portfolio;
using Domain.Entities;
using Domain.Enums;
using PikUpStix.Trading.Forecast;

namespace Application.Business.BackTest.Positioning
{
    public class AdjustPositionWithMinimumForecastRequiredForPositionReversals : AdjustPositions
    {
        private double MaxMinForecast;
        public AdjustPositionWithMinimumForecastRequiredForPositionReversals(double maxMinForecast)
        {
            MaxMinForecast = maxMinForecast;
        }

        public override void OpenTrade(DateTime cursorDate, PositionValue weightedProposedPosition, double volume, double stopLoss)
        {
            if (weightedProposedPosition.ForecastValue.Forecast >= MaxMinForecast || weightedProposedPosition.ForecastValue.Forecast <= MaxMinForecast * -1)
                base.OpenTrade(cursorDate, weightedProposedPosition, volume, stopLoss);
        }
    }
    public class AdjustPositions : IPositionHandler
    {
        private TradingSystemParams Parameters;
        private int TestId;
        private double CurrentMargin;
        private IStopLossCreator StopLossCreator;
        private List<Domain.Entities.Position> Trades;

        public List<Domain.Entities.Position> GetUpdatedPositions(TradingSystemParams paramsa, int testId, List<Domain.Entities.Position> existingPositions, double currentMargin, DateTime cursorDate, WeightedProposedPositions weightedPositions,
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

        public List<Domain.Entities.Position> GetAdjustedTrades()
        {
            return Trades;
        }

        private void AdjustInstrumentPositions(DateTime cursorDate, PositionValue proposedPosition)
        {
            var proposedPositionSize = proposedPosition.ProposedWeightedPosition;
            var currentPositionSize = Trades.Where(x => x.Status == PositionStatus.OPEN && x.InstrumentId == proposedPosition.Instrument.Id).Sum(y => y.Volume);

            //var positionAdjuster = new PositionCalculator(proposedPositionSize, Trades.Where(x => x.Status == PositionStatus.POSITION.ToString() && x.InstrumentId == proposedPosition.Instrument.InstrumentId).ToList());
            //var newProposedPositions = positionAdjuster.RevisedPositions;


            // Close Trades
            foreach (var position in Trades.Where(x => x.Status == PositionStatus.OPEN && x.InstrumentId == proposedPosition.Instrument.Id))
            {  //if (!positionAdjuster.RevisedPositions.Any(x => x == position))
                CloseTrade(position, cursorDate, proposedPosition, proposedPosition.AskingPrice);
            }


            Trades = StopLossCreator.CalculateStops(Trades, Parameters.StopLossPercent, proposedPosition.AvailableTradingCapital,
                proposedPositionSize, Parameters.ExchangeRate, proposedPosition);
            // Open Trades
            //if (positionAdjuster.RequiredAdjustmentToVolume != 0)
            if (proposedPositionSize != 0)
                OpenTrade(cursorDate, proposedPosition, proposedPositionSize, StopLossCreator.NewPositionStopLoss);
        }

        public virtual void OpenTrade(DateTime cursorDate, PositionValue weightedProposedPosition, double volume, double stopLoss)
        {
            PositionType direction = volume < 0
                ? PositionType.SELL
                : PositionType.BUY;

            var position = new Domain.Entities.Position
            {
                TestId = TestId,
                InstrumentId = weightedProposedPosition.Instrument.Id,
                Created = cursorDate,
                PositionType = direction,
                EntryPrice = weightedProposedPosition.AskingPrice,
                Status = PositionStatus.OPEN,
                Volume = volume,
                TakeProfit = 0,
                StopLoss = stopLoss,
                Commission = 0,
                Comment = "None",
                ClosePrice = 0,
                TrailingStop = 0,
                Margin = 0.0
            };
            Trades.Add(position);
        }

        private void CloseTrade(Domain.Entities.Position trade, DateTime cursorDate, PositionValue weightedProposedPosition, double currentPrice)
        {
            foreach (
                Domain.Entities.Position existingPosition in
                    Trades.Where(x => x == trade && x.Status == PositionStatus.OPEN))
            {
                existingPosition.ClosePrice = currentPrice;
                existingPosition.ClosedAt = cursorDate;
               
                existingPosition.Status = PositionStatus.CLOSED;
                existingPosition.Comment = "Position closed due to volume change.  From existing volume of " + trade.Volume + " to new volume of " + weightedProposedPosition.ProposedWeightedPosition;
                
                existingPosition.Margin =
                    Margin.Calculate(0.0001, Parameters.ExchangeRate, trade,
                        currentPrice, trade.Volume);
                CurrentMargin += existingPosition.Margin;
            }
        }
    }
}

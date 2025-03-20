using Application.Business;
using Application.Business.Portfolio;
using Domain.Entities;
using Domain.Enums;

namespace PikUpStix.Trading.Forecast
{
    public interface IStopLossCreator
    {
        double NewPositionStopLoss { get; set; }
        List<Domain.Entities.Position> CalculateStops(List<Domain.Entities.Position> trades, double stopLossPercentage, double currentMargin, double volume, double exchangeRate, PositionValue weightedProposedPosition);
    }

    public class SimpleStopLossCreator : IStopLossCreator
    {
        // Works out stop loss based on single percentage input and does not consider other position that may be on the same instrument
        public double NewPositionStopLoss { get; set; }

        public List<Domain.Entities.Position> CalculateStops(List<Domain.Entities.Position> trades, double stopLossPercentage, double currentMargin, 
            double volume, double exchangeRate, PositionValue weightedProposedPosition)
        {
            var stopLoss = new StopLossCalculator(currentMargin,
                stopLossPercentage,
                weightedProposedPosition.Instrument.ContractUnit,
                volume,
                exchangeRate,
                volume < 0 ? PositionType.SELL : PositionType.BUY,
                weightedProposedPosition.AskingPrice,
                weightedProposedPosition.BiddingPrice,
                weightedProposedPosition.Instrument.MinimumPriceFluctuation);
            NewPositionStopLoss = stopLoss.StopLossInCurrency();
            return trades;
        }
    }
    public class AggregatedStopLossCreator : IStopLossCreator
    {
        // Works out stop loss for all positions on a given instrument to create overall risk based on StopLossPercent
        public double NewPositionStopLoss { get; set; }

        public List<Domain.Entities.Position> CalculateStops(List<Domain.Entities.Position> trades, double stopLossPercentage, double currentMargin,
            double volume, double exchangeRate, PositionValue weightedProposedPosition)
        {
            var totalVolume = volume;
            if(trades.Any(x => x.InstrumentId == weightedProposedPosition.Instrument.Id))
                totalVolume = volume + trades.Where(x => x.InstrumentId == weightedProposedPosition.Instrument.Id && x.Status == PositionStatus.OPEN).Sum(x => x.Volume);
            var stopLoss = new StopLossCalculator(currentMargin,
                stopLossPercentage,
                weightedProposedPosition.Instrument.ContractUnit,
                volume,
                exchangeRate,
                totalVolume < 0 ? PositionType.SELL : PositionType.BUY,
                weightedProposedPosition.AskingPrice,
                weightedProposedPosition.BiddingPrice,
                weightedProposedPosition.Instrument.MinimumPriceFluctuation);
            NewPositionStopLoss = stopLoss.StopLossInCurrency();
            foreach (var trade in trades.Where(x => x.InstrumentId == weightedProposedPosition.Instrument.Id && x.Status == PositionStatus.OPEN))
                trade.StopLoss = NewPositionStopLoss;
            return trades;
        }
    }
}
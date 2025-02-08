using Domain.Entities;
using Domain.Enums;

namespace PikUpStix.Trading.Forecast
{
    public class PositionCalculator
    {        
        public List<TestTrade> RevisedPositions { get; set; }
        public double RequiredAdjustmentToVolume { get; private set; }
        public double FinalPosition { get; private set; }
        public double ExistingPosition { get; private set; }

        public PositionCalculator(double proposedPosition, List<TestTrade> currentPositions)
        {
            RevisedPositions = new List<TestTrade>();
            RequiredAdjustmentToVolume = 0;
            FinalPosition = 0;
            ExistingPosition = 0;

            ExistingPosition = currentPositions.Sum(x => x.Volume);

            RevisedPositions = currentPositions;
            RemoveTradesInOppositeDirection(proposedPosition);
            RemoveTradesTooBig(proposedPosition);
            if (RevisedPositions.Any(x => x.Volume == proposedPosition))
                CloseAllExcept(proposedPosition);
            RemoveAllButAggregatedTrades(proposedPosition);
            CalculateRequiredAdjustmentToVolume(proposedPosition);
            FinalPosition = RevisedPositions.Sum(x => x.Volume) + RequiredAdjustmentToVolume;
        }

        private void CalculateRequiredAdjustmentToVolume(double proposedPosition)
        {
            var existingVolume = RevisedPositions.Sum(x => x.Volume);
            //if (PositionIsReversal(proposedPosition))
            //    existingVolume = RevisedPositions.Sum(x => x.Volume) + ExistingPosition;
            if (IsASell(proposedPosition))
                RequiredAdjustmentToVolume = proposedPosition - existingVolume;
            if (IsABuy(proposedPosition))
                RequiredAdjustmentToVolume = proposedPosition - existingVolume;
        }

        private void RemoveTradesTooBig(double proposedPosition)
        {
            var removals = new List<TestTrade>();
            foreach (var position in RevisedPositions.Where(x => x.Volume * x.Volume > proposedPosition * proposedPosition))
                removals.Add(position);
            foreach (var remove in removals)
                RevisedPositions.Remove(remove);
        }

        private void RemoveAllButAggregatedTrades(double proposedPosition)
        {
            // Note this is pretty simple.  Just orders by volume ascending and should use matching combos instead
            var aggregatedPositionSum = 0.0;
            var removals = new List<TestTrade>();
            foreach (var position in RevisedPositions.OrderBy(x => x.Volume))
            {
                if (Math.Pow(aggregatedPositionSum + position.Volume, 2) <= Math.Pow(proposedPosition,2))
                    aggregatedPositionSum += position.Volume;
                else
                    removals.Add(position);
            }
            foreach (var remove in removals)
                RevisedPositions.Remove(remove);
            
        }
       
        private void RemoveTradesInOppositeDirection(double proposedPosition)
        {
            if (IsASell(proposedPosition))
                RemovePositions(PositionType.BUY);
            if (IsABuy(proposedPosition))
                RemovePositions(PositionType.SELL);
        }

        private void RemovePositions(PositionType positionTypeToRemove)
        {
            var removals = new List<TestTrade>();
            foreach (var position in RevisedPositions.Where(x => x.Direction == positionTypeToRemove.ToString()))
            {
                removals.Add(position);
            }
            foreach (var remove in removals)
                RevisedPositions.Remove(remove);
        }

        private void CloseAllExcept(double proposedPosition)
        {
            var singleAlreadyMatched = false;
            var removals = new List<TestTrade>();
            foreach (var position in RevisedPositions)
            {
                if (position.Volume != proposedPosition)
                    removals.Add(position);
                else
                {
                    if (singleAlreadyMatched)
                        removals.Add(position);
                    else // don't remove match, set match flag to true
                        singleAlreadyMatched = true;
                }
            }
            foreach (var remove in removals)
                RevisedPositions.Remove(remove);
        }

        private bool IsASell(double proposedPosition)
        {
            return proposedPosition < 0;
        }
        private bool IsABuy(double proposedPosition)
        {
            return proposedPosition > 0;
        }
    }
}

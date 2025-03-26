using Domain.Entities;

namespace Application.Business.Positioning.Handlers
{
    internal class ModifyPositionHandler
    {
        private List<Position> openPositions;

        public ModifyPositionHandler(ref List<Position> openPositions)
        {
            this.openPositions = openPositions;
        }

        internal void ModifyPosition(Position position, double? adjustStopLossTo, double? adjustTakeProfitTo)
        {
            if (position == null)
                return;          
            var positionToModify = openPositions.Find(p => p.Equals(position));
            if (positionToModify != null)
            {
                if (adjustStopLossTo.HasValue)
                    position.StopLoss = adjustStopLossTo;
                if (adjustTakeProfitTo.HasValue)
                    position.TakeProfit = adjustTakeProfitTo;
            }
        }
    }
}
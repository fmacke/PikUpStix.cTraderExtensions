using Domain.Entities;

namespace TradeSimulator.Business
{
    internal class OpenPositionHandler
    {
        private List<Position> openPositions;

        public OpenPositionHandler(ref List<Position> openPositions)
        {
            this.openPositions = openPositions;
        }

        internal void OpenPosition(Position position)
        {
            openPositions.Add(position);
        }
    }
}
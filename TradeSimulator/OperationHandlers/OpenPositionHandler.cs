using Application.Business.Positioning;
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

        internal void OpenPosition(OpenInstruction positionInstruction)
        {
            openPositions.Add(positionInstruction.Position);
        }
    }
}
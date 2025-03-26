using Application.Business.Positioning.Instructions;
using Domain.Entities;

namespace Application.Business.Positioning.Handlers
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
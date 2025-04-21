using Application.Business.Calculations;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
    internal class ClosePositionHandler
    {
        private Dictionary<Position, Position> _positionLookup;
        public ClosePositionHandler(ref List<Position> positions)
        {
            _positionLookup = positions.ToDictionary(p => p);
        }
        public void ClosePosition(Position position, double closePrice, DateTime closedAt, double contractUnit, double exchangeRate)
        {
            if (_positionLookup.TryGetValue(position, out var pos))
            {
                pos.ClosedAt = closedAt;
                pos.Status = PositionStatus.CLOSED;
                pos.ClosePrice = closePrice;
                pos.Margin = new Margin(contractUnit, exchangeRate, pos, closePrice, pos.Volume).Calculate();
            }
        }
    }
}
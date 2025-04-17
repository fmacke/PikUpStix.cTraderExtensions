using Application.Business.Calculations;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
    internal class ClosePositionHandler
    {
        private List<Position> _positions;
        public ClosePositionHandler(ref List<Position> positions)
        {
            this._positions = positions;
        }
        public void ClosePosition(Position position, double closePrice, DateTime closedAt, double contractUnit, double exchangeRate)
        {
            if (position.ClosedAt == null)
            {
                _positions.Find(p => p.Equals(position)).ClosedAt = closedAt; ;
                _positions.Find(p => p.Equals(position)).Status = PositionStatus.CLOSED;
                _positions.Find(p => p.Equals(position)).ClosePrice = closePrice;
                _positions.Find(p => p.Equals(position)).Margin = Margin.Calculate(contractUnit, exchangeRate, position, closePrice, position.Volume);
            }
            else
            {
                throw new Exception("Position already closed");
            }
        }
    }
}
using Domain.Entities;

namespace Application.Business
{
    public class Positions : List<Position>
    {
        public Positions()
        {
        }

        public Positions(List<Position> positions)
        {
            foreach (var position in positions)
            {
                Add(new Position());
            }
        }

        public Position GetPosition(int id)
        {
            return this.First(x => x.Id == id);
        }

        public void AddPosition(Position position)
        {
            Add(position);
        }

        public void RemovePosition(Position position)
        {
            Remove(position);
        }

        public void RemovePosition(int tradeId)
        {
            Remove(GetPosition(tradeId));
        }

        public void UpdatePosition(Position position)
        {
            var pos = GetPosition(position.Id);
            pos = position;
        }

        public void UpdatePosition(int tradeId, Position position)
        {
            var pos = GetPosition(tradeId);
            pos = position;
        }

        public bool PositionExists(int id)
        {
            return this.Any(x => x.Id == id);
        }

        public bool PositionExists(Position position)
        {
            return this.Any(x => x.Id == position.Id);
        }
    }
}
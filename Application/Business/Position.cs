using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

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
    [NotMapped]
    public class Position 
    {
        public int Id { get; set; }
        public string SymbolName {get; set;}
        public double? StopLoss { get; set; } = 0;
        public double? TakeProfit { get; set; } = 0;
        public double Volume { get; set; } = 0;
        public PositionStatus Status { get; set; } = PositionStatus.POSITION;
        public TradeType TradeType { get; set; } 
        public double EntryPrice { get; set; } = 0;
        public double ClosePrice { get; set; } = 0;
        public double Margin { get; set; } = 0;
        public double TrailingStop { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpirationDate { get; set; }
        

        public bool PositionHasStopLoss()
        {
            return StopLoss > 0;
        }

        public bool PositionHasProfitTarget()
        {
            return TakeProfit > 0;
        }

        public bool PositionIsOpen()
        {
            return Status.Equals(PositionStatus.POSITION.ToString());
        }
    }
}
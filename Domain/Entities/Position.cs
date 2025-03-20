using Domain.Abstractions;
using Domain.Enums;
namespace Domain.Entities
{
    public partial class Position : BaseEntity
    {
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public double Volume { get; set; } = 0;
        public PositionType PositionType { get; set; }
        public double EntryPrice { get; set; }
        public double? TakeProfit { get; set; }
        public double? StopLoss { get; set; }
        public double Commission { get; set; } = 0;
        public DateTime Created { get; set; }
        public string Comment { get; set; }
        public double? ClosePrice { get; set; }
        public double? TrailingStop { get; set; }
        public double Margin { get; set; } = 0;
        public PositionStatus Status { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string SymbolName { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}

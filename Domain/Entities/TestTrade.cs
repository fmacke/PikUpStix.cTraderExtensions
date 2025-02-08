using Domain.Abstractions;
namespace Domain.Entities
{
    public partial class TestTrade : BaseEntity
    {
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public double Volume { get; set; }
        public string Direction { get; set; }
        public double EntryPrice { get; set; }
        public double TakeProfit { get; set; }
        public double StopLoss { get; set; }
        public double Commission { get; set; }
        public DateTime Created { get; set; }
        public string Comment { get; set; }
        public double ClosePrice { get; set; }
        public int TrailingStop { get; set; }
        public double Margin { get; set; }
        public string InstrumentWeight { get; set; }
        public string Status { get; set; }
        public DateTime? ClosedAt { get; set; }
        public double? CapitalAtEntry { get; set; }
        public double? CapitalAtClose { get; set; }
        public double? ForecastAtEntry { get; set; }
        public double? ForecastAtClose { get; set; }
    }
}

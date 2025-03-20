using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Features.TestTrades.Queries.GetAllCached
{
    public class GetAllPositionsCachedResponse
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public decimal Volume { get; set; }
        public string Direction { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal StopLoss { get; set; }
        public decimal Commission { get; set; }
        public DateTime Created { get; set; }
        public string Comment { get; set; }
        public decimal ClosePrice { get; set; }
        public int TrailingStop { get; set; }
        public decimal Margin { get; set; }
        public string InstrumentWeight { get; set; }
        public string Status { get; set; }
        public DateTime? ClosedAt { get; set; }
        public decimal? CapitalAtEntry { get; set; }
        public decimal? CapitalAtClose { get; set; }
        public decimal? ForecastAtEntry { get; set; }
        public decimal? ForecastAtClose { get; set; }
    }
}
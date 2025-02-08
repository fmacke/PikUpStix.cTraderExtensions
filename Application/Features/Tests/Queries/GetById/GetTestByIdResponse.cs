using Domain.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Tests.Queries.GetById
{
    public class GetTestByIdResponse
    {
        public int Id { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime FromDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime ToDate { get; set; }
        [Column(TypeName = "money")]
        public double StartingCapital { get; set; }
        [Column(TypeName = "money")]
        public double EndingCapital { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TestRunAt { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TestEndAt { get; set; }
        public double? MaxAdverseExcursion { get; set; }
        public double? SharpeRatio { get; set; }
        [Column(TypeName = "money")]
        public double NetProfit { get; set; }
        [Column(TypeName = "money")]
        public double Commission { get; set; }
        public double MaxEquityDrawdown { get; set; }
        public double MaxBalanceDrawdown { get; set; }
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int MaxConsecutiveWinningTrades { get; set; }
        [Column(TypeName = "money")]
        public double LargestWinningTrade { get; set; }
        public int LosingTrades { get; set; }
        public int MaxConsecutiveLosingTrades { get; set; }
        [Column(TypeName = "money")]
        public double LargestLosingTrades { get; set; }
        [Column(TypeName = "money")]
        public double AverageTrade { get; set; }
        public double SortinoRatio { get; set; }
        [Column(TypeName = "money")]
        public double GrossProfit { get; set; }
        [Column(TypeName = "money")]
        public double GrossLoss { get; set; }
        [Column(TypeName = "money")]
        public double NetShortProfit { get; set; }
        [Column(TypeName = "money")]
        public double NetLongProfit { get; set; }
        [Column(TypeName = "money")]
        public double GrossShortProfit { get; set; }
        [Column(TypeName = "money")]
        public double GrossLongProfit { get; set; }
        public double ProfitFactor { get; set; }
        public double ProfitFactorLongTrades { get; set; }
        public double ProfitFactorShortTrades { get; set; }
        [Column(TypeName = "money")]
        public double NetShortLoss { get; set; }
        [Column(TypeName = "money")]
        public double NetLongLoss { get; set; }
        [Column(TypeName = "money")]
        public double GrossShortLoss { get; set; }
        [Column(TypeName = "money")]
        public double GrossLongLoss { get; set; }
        public double ProfitableTradesRatio { get; set; }
        public double LosingTradesRatio { get; set; }
        public double ProfitableLongTradesRatio { get; set; }
        public double ProfitableShortTradesRatio { get; set; }
        [Column(TypeName = "money")]
        public double AverageWin { get; set; }
        [Column(TypeName = "money")]
        public double AverageWinLong { get; set; }
        [Column(TypeName = "money")]
        public double AverageWinShort { get; set; }
        [Column(TypeName = "money")]
        public double AverageLoss { get; set; }
        [Column(TypeName = "money")]
        public double AverageLossLong { get; set; }
        [Column(TypeName = "money")]
        public double AverageLossShort { get; set; }
    }
    
}

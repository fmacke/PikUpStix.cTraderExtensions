using Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    public partial class Test : BaseEntity
    {
        //public Test()
        //{
        //    TestTrades 
        //    Test_Parameters = new HashSet<Test_Parameter>();
        //}
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double StartingCapital { get; set; }
        public double EndingCapital { get; set; }
        public string Description { get; set; }
        public DateTime? TestRunAt { get; set; }
        public DateTime? TestEndAt { get; set; }
        public double? MaxAdverseExcursion { get; set; }
        public double? SharpeRatio { get; set; }
        public double NetProfit { get; set; }
        public double Commission { get; set; }
        public double MaxEquityDrawdown { get; set; }
        public double MaxBalanceDrawdown { get; set; }
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int MaxConsecutiveWinningTrades { get; set; }
        public double LargestWinningTrade { get; set; }
        public int LosingTrades { get; set; }
        public int MaxConsecutiveLosingTrades { get; set; }
        public double LargestLosingTrades { get; set; }
        public double AverageTrade { get; set; }
        public double SortinoRatio { get; set; }
        public double GrossProfit { get; set; }
        public double GrossLoss { get; set; }
        public double NetShortProfit { get; set; }
        public double NetLongProfit { get; set; }
        public double GrossShortProfit { get; set; }
        public double GrossLongProfit { get; set; }
        public double ProfitFactor { get; set; }
        public double ProfitFactorLongTrades { get; set; }
        public double ProfitFactorShortTrades { get; set; }
        public double NetShortLoss { get; set; }
        public double NetLongLoss { get; set; }
        public double GrossShortLoss { get; set; }
        public double GrossLongLoss { get; set; }
        public double ProfitableTradesRatio { get; set; }
        public double LosingTradesRatio { get; set; }
        public double ProfitableLongTradesRatio { get; set; }
        public double ProfitableShortTradesRatio { get; set; }
        public double AverageWin { get; set; }
        public double AverageWinLong { get; set; }
        public double AverageWinShort { get; set; }
        public double AverageLoss { get; set; }
        public double AverageLossLong { get; set; }
        public double AverageLossShort { get; set; }
        public virtual ICollection<Position> Positions { get; set; } = new HashSet<Position>();
        public virtual ICollection<Test_Parameter> Test_Parameters { get; set; } = new HashSet<Test_Parameter>();
    }
}

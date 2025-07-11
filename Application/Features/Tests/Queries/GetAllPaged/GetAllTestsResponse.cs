﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Tests.Queries.GetAllPaged
{
    public class GetAllTestsResponse
    {
        public int Id { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime FromDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime ToDate { get; set; }
        [Column(TypeName = "money")]
        public decimal StartingCapital { get; set; }
        [Column(TypeName = "money")]
        public decimal EndingCapital { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TestRunAt { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TestEndAt { get; set; }
        public double? MaxAdverseExcursion { get; set; }
        public double? SharpeRatio { get; set; }
        [Column(TypeName = "money")]
        public decimal NetProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal Commission { get; set; }
        public double MaxEquityDrawdown { get; set; }
        public double MaxBalanceDrawdown { get; set; }
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int MaxConsecutiveWinningTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal LargestWinningTrade { get; set; }
        public int LosingTrades { get; set; }
        public int MaxConsecutiveLosingTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal LargestLosingTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageTrade { get; set; }
        public double SortinoRatio { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal NetShortProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal NetLongProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossShortProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossLongProfit { get; set; }
        public double ProfitFactor { get; set; }
        public double ProfitFactorLongTrades { get; set; }
        public double ProfitFactorShortTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal NetShortLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal NetLongLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossShortLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossLongLoss { get; set; }
        public double ProfitableTradesRatio { get; set; }
        public double LosingTradesRatio { get; set; }
        public double ProfitableLongTradesRatio { get; set; }
        public double ProfitableShortTradesRatio { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageWin { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageWinLong { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageWinShort { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageLossLong { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageLossShort { get; set; }
    }
}
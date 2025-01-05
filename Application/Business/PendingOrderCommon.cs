using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business
{
    public class PendingOrderCommon
    {
        public TradeType TradeType { get; set; }
        public double VolumeInUnits { get; set; }
        public int Id { get; set; }
        public TradeType OrderType { get; set; }
        public double TargetPrice { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public double? StopLoss { get; set; }
        public double? StopLossPips { get; set; }
        public double? TakeProfit { get; set; }
        public double? TakeProfitPips { get; set; }
        public string Label { get; set; }
        public string Comment { get; set; }
        public double Quantity { get; set; }
        public bool HasTrailingStop { get; set; }
        public double? StopLimitRangePips { get; set; }
        public string SymbolName { get; set; }
        public double CurrentPrice { get; set; }
        public double DistancePips { get; set; }
        public string Channel { get; set; }
        public DateTime SubmittedTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        //public ProtectionType ProtectionType { get; set; }
    }
}

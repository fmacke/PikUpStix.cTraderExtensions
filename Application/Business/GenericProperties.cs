using Application.Business.Risk;
using Domain.Entities;
namespace Application.Business
{
    public class GenericProperties : IGenericProperties
    {
        public DateTime CursorDate { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public Positions Positions { get; set; }
        public List<PendingOrder> Orders { get; set; }
        public List<HistoricalData> Bars { get; set; }
        public IRiskManager RiskManager { get; set; }
        public string SymbolName { get; set; }
        public double AccountBalance { get; set; }
        public double PipSize { get; set; }
    }
}

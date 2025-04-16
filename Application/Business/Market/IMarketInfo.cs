using Application.Business.Indicator.Signal;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Market
{
    public interface IMarketInfo
    {
        DateTime CursorDate { get; set; }
        double Ask { get; set; }        
        double Bid { get; set; }        
        string SymbolName { get; }
        string Currency { get; }
        double CurrentCapital { get; set; }
        public HistoricalData CurrentBar { get;  set; }
        public HistoricalData LastBar { get;  set; }
        double ContractUnit { get; }
        double ExchangeRate { get; }
        List<HistoricalData> Bars { get; set; }
        List<Position> Positions { get; set; }
        ConfirmingSignals Signals { get; }
        TimeFrame TimeFrame { get; }
    }
}
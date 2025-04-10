using Application.Business.Indicator.Signal;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Market
{
    public interface IMarketInfo
    {
        DateTime CursorDate { get; }
        double Ask { get; }        
        double Bid { get; }        
        string SymbolName { get; }
        string Currency { get; }
        double AccountBalance { get; }
        double ContractUnit { get; }
        double ExchangeRate { get; }
        List<HistoricalData> Bars { get; }
        List<Position> Positions { get; }
        ConfirmingSignals Signals { get; }
        TimeFrame TimeFrame { get; }
    }
}
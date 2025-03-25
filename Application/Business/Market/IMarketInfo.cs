using Application.Business.Indicator;
using Application.Business.Indicator.Signal;
using Domain.Entities;

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
        double PipSize { get; }
        double Balance { get; }
        double Maximumrisk { get; }
        List<HistoricalData> Bars { get; }
        List<Position> Positions { get; }
        ConfirmingSignals Signals { get; }
        List<IIndicator> Indicators { get; }
    }
}
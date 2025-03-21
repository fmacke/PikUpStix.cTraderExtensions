using Domain.Entities;

namespace Application.Business.Market
{
    public interface IMarketInfo
    {
        double Ask { get; }
        List<HistoricalData> Bars { get; }
        double Bid { get; }
        DateTime CursorDate { get; }
        List<PendingOrder> Orders { get; }
        List<Position> Positions { get; }
        string SymbolName { get; }
        string Currency { get; }
        double AccountBalance { get; }
        double PipSize { get; }
    }
}
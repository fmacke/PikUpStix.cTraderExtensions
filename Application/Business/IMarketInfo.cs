using Domain.Entities;

namespace Application.Business
{
    public interface IMarketInfo
    {
        double Ask { get; }
        List<HistoricalData> Bars { get; }
        double Bid { get;  }
        DateTime CursorDate { get;  }
        List<PendingOrder> Orders { get;}
        Positions Positions { get;  }
        string SymbolName { get;  }
        string Currency { get; }
        double AccountBalance { get;  }
        double PipSize { get; }
    }
}
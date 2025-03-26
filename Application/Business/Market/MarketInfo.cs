using Application.Business.Indicator;
using Application.Business.Indicator.Signal;
using Domain.Entities;
namespace Application.Business.Market
{
    public class MarketInfo : IMarketInfo
    {
        public DateTime CursorDate { get; private set; }
        public double Bid { get; private set; }
        public double Ask { get; private set; }
        public List<Position> Positions { get; private set; }
        public List<HistoricalData> Bars { get; private set; }
        public string SymbolName { get; private set; }
        public double AccountBalance { get; private set; }
        public string Currency { get; private set; }
        public double ExchangeRate { get; private set; }
        public double ContractUnit { get; private set; }

        public double Balance => throw new NotImplementedException();

        public double Maximumrisk => throw new NotImplementedException();

        public ConfirmingSignals Signals => throw new NotImplementedException();

        public List<IIndicator> Indicators => throw new NotImplementedException();

        public MarketInfo(DateTime cursorDate, double bid, double ask, List<Position> positions,
            List<HistoricalData> bars, string symbolName, string currency, double accountBalance,
            double contractUnit, double exchangeRate)
        {
            CursorDate = cursorDate;
            Bid = bid;
            Ask = ask;
            Positions = positions;
            Bars = bars;
            SymbolName = symbolName;
            Currency = currency;
            AccountBalance = accountBalance;
            ExchangeRate = exchangeRate;
            ContractUnit = contractUnit;
        }
    }
}

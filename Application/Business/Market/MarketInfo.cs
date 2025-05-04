using Application.Business.Indicator.Signal;
using Domain.Entities;
using Domain.Enums;
namespace Application.Business.Market
{
    public class MarketInfo : IMarketInfo
    {
        public DateTime CursorDate { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public List<Position> Positions { get; set; }
        public List<HistoricalData> Bars { get; set; }
        public string SymbolName { get; private set; }
        public double CurrentCapital { get; set; }
        public string Currency { get; private set; }
        public double ExchangeRate { get; private set; }
        public double PipSize { get; private set; }
        public double LotSize { get; private set; }
        public ConfirmingSignals Signals { get; private set; }
        public TimeFrame TickTimeFrame { get; private set; }
        public TimeFrame BarTimeFrame { get; private set; }
        public HistoricalData CurrentBar { get; set; }
        public HistoricalData LastBar { get; set; }

        public MarketInfo(DateTime cursorDate, double bid, double ask, List<Position> positions,
            List<HistoricalData> bars, string symbolName, string currency, double accountBalance,
            double pipSize, double lotSize, double exchangeRate, ConfirmingSignals signals, TimeFrame timeFrame)
        {
            CursorDate = cursorDate;
            Bid = bid;
            Ask = ask;
            Positions = positions;
            Bars = bars;
            SymbolName = symbolName;
            Currency = currency;
            CurrentCapital = accountBalance;
            ExchangeRate = exchangeRate;
            PipSize = pipSize;
            LotSize = lotSize;
            Signals = signals;
            //TickTimeFrame = timeFrame;
            BarTimeFrame = timeFrame;
            if (bars != null && bars.Count > 0)
            {
                CurrentBar = bars.LastOrDefault();
                LastBar = bars[bars.Count - 2];
            }
            else
            {
                CurrentBar = new HistoricalData();
                LastBar = new HistoricalData();
            }
        }
    }
}

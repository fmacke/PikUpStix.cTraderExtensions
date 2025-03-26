using Application.Business.Market;
using Application.Business.Reports;
using Application.Business.Utilities;
using Application.Interfaces;
using Domain.Entities;
using System.Diagnostics;
using TradeSimulator.Business;

namespace TradeSimulator
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> OpenPositions = new List<Position>();
        List<Position> ClosedTrades = new List<Position>();

        public IStrategy Strategy { get; private set; }

        public TradeSimulate(List<HistoricalData> bars, IStrategy strategy, double initialCapital) : base(initialCapital, bars) 
        { 
            Strategy = strategy;
        }

        protected internal override void OnBar()
        {
            // remove from OpenPositions and add to ClosedTrades
            new StopLossHandler(CurrentBar.Date, CurrentBar.OpenPrice, ref OpenPositions, ref ClosedTrades).CloseOutStops();
            var positionInstructions = Strategy.Run(GetMarketInfo());
            // remove/updates OpenPositions and adds to ClosedTrades where necessary
            new PositionHandler(positionInstructions, ref OpenPositions, ref ClosedTrades).ExecuteInstructions();
        }        
        protected internal override void OnStart()
        {
            foreach(var position in ClosedTrades)
            {
                Debug.WriteLine($"Closed Trade: {position.SymbolName} {position.PositionType} {position.Volume} {position.StopLoss}");
            }
        }
        protected internal override void OnStop()
        {
            Debug.WriteLine("OnStop");
            var report = new TradeStatistics(ClosedTrades, InitialCapital, 20);
            Debug.WriteLine(ClassToString.FormatProperties(report));
        }
        private List<IMarketInfo> GetMarketInfo()
        {
            return new List<IMarketInfo>() { new MarketInfo(Convert.ToDateTime(CurrentBar.Date), CurrentBar.OpenPrice, CurrentBar.OpenPrice, OpenPositions, CurrentBars, "EURUSD", "GBPUSD", 10000, 0.0001) };
        }
    }
}

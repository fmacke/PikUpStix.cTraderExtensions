using Application.Business.Market;
using Application.Business.Reports;
using Application.Business.Utilities;
using Application.Business.Positioning.Handlers;
using Application.Interfaces;
using Domain.Entities;
using System.Diagnostics;

namespace Application.Business.Simulate
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
            var marketInfo = GetMarketInfo();
            new StopLossHandler(CurrentBar.Date, CurrentBar.OpenPrice, ref OpenPositions, ref ClosedTrades, marketInfo).CloseOutStops();            
            var positionInstructions = Strategy.Run(marketInfo);
            new PositionHandler(positionInstructions, ref OpenPositions, ref ClosedTrades, marketInfo).ExecuteInstructions();
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
            return new List<IMarketInfo>() { 
                new MarketInfo(Convert.ToDateTime(CurrentBar.Date), 
                CurrentBar.OpenPrice, CurrentBar.OpenPrice, 
                OpenPositions, CurrentBars, 
                "EURUSD", "GBPUSD", 10000, 1, 1) };
        }
    }
}

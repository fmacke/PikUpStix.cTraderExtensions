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
        List<IMarketInfo> MarketInfo = new List<IMarketInfo>();

        public IStrategy Strategy { get; private set; }

        public TradeSimulate(IMarketInfo runData, IStrategy strategy, double initialCapital) : base(initialCapital, runData.Bars) 
        {
            MarketInfo.Add(runData);
            Strategy = strategy;
        }
        protected internal override void OnBar()
        {
            var currentMarketInfo = new MarketInfo(Convert.ToDateTime(CurrentBar.Date), CurrentBar.OpenPrice, CurrentBar.OpenPrice, OpenPositions, CurrentBars, 
                MarketInfo.First().SymbolName, MarketInfo.First().Currency, MarketInfo.First().AccountBalance, MarketInfo.First().ContractUnit,
                MarketInfo.First().ExchangeRate, MarketInfo.First().Signals, MarketInfo.First().TimeFrame);
            new StopLossHandler(CurrentBar.Date, CurrentBar.OpenPrice, ref OpenPositions, ref ClosedTrades, MarketInfo).CloseOutStops();  
            var listed = new List<IMarketInfo>();
            listed.Add(currentMarketInfo);
            var positionInstructions = Strategy.Run(listed);
            new PositionHandler(positionInstructions, ref OpenPositions, ref ClosedTrades, MarketInfo).ExecuteInstructions();
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
    }
}

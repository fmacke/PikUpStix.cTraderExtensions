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
        //List<Position> ClosedTrades = new List<Position>();

        public IStrategy Strategy { get; private set; }

        public TradeSimulate(IMarketInfo runData, IStrategy strategy, double initialCapital) : base(initialCapital, runData) 
        {
            Strategy = strategy;
        }
        protected internal override void OnBar()
        {
            List<IMarketInfo> marketInfos = new List<IMarketInfo>();
            marketInfos.Add(CurrentMarketInfo);  // this works for now for testing purposes, where a strategy only deals with a single market instrument.
            new StopLossHandler(CurrentMarketInfo.CursorDate, ref OpenPositions, marketInfos).CloseOutStops();            
            var positionInstructions = Strategy.Run(marketInfos);
            new PositionHandler(positionInstructions, ref OpenPositions, marketInfos).ExecuteInstructions();
        }        
        protected internal override void OnStart()
        {
                Debug.WriteLine($"TradeSimulate OnStart()");
        }
        protected internal override void OnStop()
        {
            Debug.WriteLine("OnStop");
            var report = new TradeStatistics(OpenPositions, InitialCapital, 20);
            Debug.WriteLine(ClassToString.FormatProperties(report));
        }        
    }
}

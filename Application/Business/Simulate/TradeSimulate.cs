using Application.Business.Market;
using Application.Business.Reports;
using Application.Business.Positioning.Handlers;
using Application.Interfaces;
using Domain.Entities;
using System.Diagnostics;
using Application.Common.Utilities;
using Application.Business.Positioning.Instructions;

namespace Application.Business.Simulate
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> OpenPositions = new List<Position>();
        List<IPositionInstruction> PositionInstructions;
        public IStrategy Strategy { get; private set; }

        public TradeSimulate(IMarketInfo runData, IStrategy strategy, double initialCapital) : base(initialCapital, runData) 
        {
            Strategy = strategy;
        }
        protected internal override void OnBar()
        {
           
            List<IMarketInfo> marketInfos = new List<IMarketInfo>();
            marketInfos.Add(CurrentMarketInfo);  // this works for now for testing purposes, where a strategy only deals with a single market instrument.
            TimeSpan ts1 = MethodTimer.MeasureExecutionTime(() => 
                new StopLossHandler(CurrentMarketInfo.CursorDate, ref OpenPositions, marketInfos).CloseOutStops());
            TimeSpan ts2 = MethodTimer.MeasureExecutionTime(() =>
                RunStrat(marketInfos));
            TimeSpan ts3 = MethodTimer.MeasureExecutionTime(() =>
                new PositionHandler(PositionInstructions, ref OpenPositions, marketInfos).ExecuteInstructions());
            Console.WriteLine($"StopLossHandler Time: {ts1.TotalMilliseconds} ms");
            Console.WriteLine($"RunStrat Time: {ts2.TotalMilliseconds} ms");
            Console.WriteLine($"PositionHandler Time: {ts3.TotalMilliseconds} ms");
        }

        private void RunStrat(List<IMarketInfo> marketInfos)
        {
            PositionInstructions = Strategy.Run(marketInfos);
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

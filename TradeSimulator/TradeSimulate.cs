using Domain.Entities;
using Robots.Interfaces;
using TradeSimulator.Business;

namespace TradeSimulator
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> OpenPositions = new List<Position>();
        List<Position> ClosedTrades = new List<Position>();
        List<HistoricalData> Bars { get; set; }

        public IStrategy Strategy { get; private set; }

        public TradeSimulate(IStrategy strategy, List<HistoricalData> bars) : base(bars)
        {
            Strategy = strategy;
        }
        protected internal override void OnBar()
        {
            new StopLossHandler(CurrentBar.OpenPrice, ref OpenPositions, ref ClosedTrades).CloseOutStops();
            //new StrategyRunner(Strategy, CurrentBar, Bars).Run();
            new PositionHandler(Strategy.PositionInstructions, ref OpenPositions, ref ClosedTrades).ExecuteInstructions();
        } 

        protected internal override void OnStart()
        {
            Console.WriteLine("OnStart");
        }
        protected internal override void OnStop()
        {
            Console.WriteLine("OnStop");
        }
    }
}

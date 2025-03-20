using Domain.Entities;
using Robots.Interfaces;
using TradeSimulator.Business;

namespace TradeSimulator
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> OpenPositions = new List<Position>();
        List<Position> ClosedTrades = new List<Position>();

        public IStrategy Strategy { get; private set; }

        public TradeSimulate(List<HistoricalData> bars) : base(bars)
        {
        }
        protected internal override void OnBar()
        {
            ManagePositions(Strategy);
        }
        protected internal override void OnStart()
        {
            Console.WriteLine("OnStart");
        }
        protected internal override void OnStop()
        {
            Console.WriteLine("OnStop");
        }
        public void ManagePositions(IStrategy x)
        {
            var positionHandler = new PositionHandler(x.PositionInstructions, ref OpenPositions, ref ClosedTrades);
            positionHandler.ExecuteInstructions();            
        }
    }
}

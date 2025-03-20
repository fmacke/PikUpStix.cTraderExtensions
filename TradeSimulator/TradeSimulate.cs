using Application.Business;
using Application.Business.Extensions;
using Application.Business.Market;
using Domain.Entities;
using Robots.Interfaces;
using Robots.Strategies.CarverTrendFollower;
using TradeSimulator.Business;
using TradeSimulator.StrategySetup;

namespace TradeSimulator
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> OpenPositions = new List<Position>();
        public List<Position> ClosedTrades = new List<Position>();
        IGetStrategyParameters GetParameters;
        public IStrategy Strategy { get; private set; }

        public TradeSimulate(List<HistoricalData> bars) : base(bars)
        {
            GetParameters = new GetCarverTrendFollowerStrategyParameters();
        }
        protected internal override void OnBar()
        {
            new StopLossHandler(CurrentBar.OpenPrice, ref OpenPositions, ref ClosedTrades).CloseOutStops();
            Strategy = new CarverTrendFollowerStrategy(
                new List<IMarketInfo>()
                {
                    new MarketInfo(Convert.ToDateTime(CurrentBar.Date), CurrentBar.OpenPrice, CurrentBar.OpenPrice,
                        new Positions(OpenPositions), new List<PendingOrder>(), Bars, "EURUSD", "GBPUSD", 10000, 0.0001)
                },
                GetParameters.Run());
            new PositionHandler(Strategy.GetPositionInstructions(), ref OpenPositions, ref ClosedTrades).ExecuteInstructions();
        }       

        protected internal override void OnStart()
        {
            foreach(var position in ClosedTrades)
            {
                Console.WriteLine($"Closed Trade: {position.SymbolName} {position.PositionType} {position.Volume} {position.StopLoss}");
            }
        }
        protected internal override void OnStop()
        {
            Console.WriteLine("OnStop");
        }
    }
}

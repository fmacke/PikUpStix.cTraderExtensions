using Application.Business;
using Application.Business.Extensions;
using Application.Business.Market;
using Application.Business.Strategy;
using Domain.Entities;
using Robots.Strategies.CarverTrendFollower;
using System.Diagnostics;
using TradeSimulator.Business;
using TradeSimulator.StrategySetup;

namespace TradeSimulator
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> OpenPositions = new List<Position>();
        List<Position> ClosedTrades = new List<Position>();
        IGetStrategyParameters GetParameters;
        public IStrategy Strategy { get; private set; }

        public TradeSimulate(List<HistoricalData> bars) : base(bars)
        {
            GetParameters = new GetCarverTrendFollowerStrategyParameters();
        }
        protected internal override void OnBar()
        {
            var parames = GetParameters.Run();
            new StopLossHandler(CurrentBar.Date, CurrentBar.OpenPrice, ref OpenPositions, ref ClosedTrades).CloseOutStops();
            Strategy = new CarverTrendFollowerStrategy(
                new List<IMarketInfo>()
                {
                    new MarketInfo(Convert.ToDateTime(CurrentBar.Date), CurrentBar.OpenPrice, CurrentBar.OpenPrice,
                        OpenPositions, new List<PendingOrder>(), CurrentBars, "EURUSD", "GBPUSD", 10000, 0.0001)
                }, parames
                );
            new PositionHandler(Strategy.GetPositionInstructions(), ref OpenPositions, ref ClosedTrades).ExecuteInstructions();
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
        }
    }
}

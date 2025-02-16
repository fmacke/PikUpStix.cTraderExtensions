using Application.Business;
using cAlgo.API;
using cAlgo.API.Internals;
using FXProBridge.Capture;
using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.CarverTrendFollower;

namespace FXProBridge.Robots
{
    public class CarverTrendFollowerRobot : PositionManager
    {
        [Parameter("Max Stop Loss", Group = "Risk Management", DefaultValue = 1, MinValue = 1, Step = 1)]
        public double MaxStopLoss { get; set; }
        [Parameter("Target Velocity", Group = "Performance", DefaultValue = 0.25, MinValue = 0.01, Step = 0.01)]
        public double TargetVelocity { get; set; }
        [Parameter("Minimum Opening Forecast", Group = "Forecasting", DefaultValue = 0.5, MinValue = 0.01, Step = 0.01)]
        public double MinimumOpeningForecast { get; set; }
        [Parameter("Trail Stop At Pips", Group = "Risk Management", DefaultValue = 100, MinValue = 1, Step = 0.01)]
        public double TrailStopAtPips { get; set; }
        [Parameter("Trail Stop Size In Pips", Group = "Risk Management", DefaultValue = 80, MinValue = 0, Step = 0.01)]
        public double TrailStopSizeInPips { get; set; }
        [Parameter("Take Profit In Pips", Group = "Risk Management", DefaultValue = 0, MinValue = 0, Step = 0.01)]
        public double TakeProfitInPips { get; set; }
        [Parameter("Short Scalar", Group = "Scalars", DefaultValue = 0.4, MinValue = 0.1, Step = 0.01)]
        public double ShortScalar { get; set; }
        [Parameter("Medium Scalar", Group = "Scalars", DefaultValue = 0.2, MinValue = 0.1, Step = 0.01)]
        public double MediumScalar { get; set; }
        [Parameter("Long Scalar", Group = "Scalars", DefaultValue = 0.4, MinValue = 0.1, Step = 0.01)]
        public double LongScalar { get; set; }

        protected override void OnStart()
        {
            TestParams = RobotProperties.GetRobotProperties((CarverTrendFollowerRobot)this);
            base.OnStart();
        }

        protected override void OnBar()
        {
            var changeInstructions = new CarverTrendFollowerStrategy(
                new List<IMarketInfo> {
                    new MarketInfo(Bars.OpenTimes.LastValue,
                    Symbol.Bid,
                    Symbol.Ask,
                    PositionConvert.ConvertPosition(Positions),
                    PendingOrderConvert.ConvertOrders(PendingOrders),
                    BarConvert.ConvertBars(Bars),
                    SymbolName,
                    SymbolName,
                    Account.Equity,
                    Symbol.PipSize) },
                ResultsCapture.TestParams);
            ManagePositions(changeInstructions);
            base.OnBar();
        }
    }
}


















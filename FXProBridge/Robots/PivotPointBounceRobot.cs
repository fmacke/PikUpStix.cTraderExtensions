using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.PivotPointBounce;
using cAlgo.API;
using cAlgo.API.Internals;
using Indicators;
using Application.Business.Indicator;

namespace FXProBridge.Robots
{
    public class PivotPointBounceRobot : PositionManager
    {
        [Parameter("Take Profit at Pivot", DefaultValue = "true")]
        public bool TakeProfitAtPivot { get; set; }

        private PivotPointIndicator _pivotPointIndicator;
        protected override void OnStart()
        {
            _pivotPointIndicator = Indicators.GetIndicator<PivotPointIndicator>();
            base.OnStart();
        }
        protected override void OnBar()
        {
            var testParams = ResultsCapture.TestParams;
            var currentTime = Bars.OpenTimes.LastValue;
            if (currentTime.Hour == 8)
                RunStrategy(currentTime);
            base.OnBar();
        }
        private void RunStrategy(DateTime cursorDate)
        {
            var pivotPoints = new PivotPoints(cursorDate,
                _pivotPointIndicator.Pivot.LastValue, 
                _pivotPointIndicator.Support1.LastValue, 
                _pivotPointIndicator.Resistance1.LastValue, 
                _pivotPointIndicator.Support2.LastValue, 
                _pivotPointIndicator.Resistance2.LastValue);
            if (pivotPoints == null)
                return; 
            var positions = PositionConvert.ConvertPosition(Positions);
            var bars = BarConvert.ConvertBars(Bars);
            var orders = PendingOrderConvert.ConvertOrders(PendingOrders);
            var changeInstructions = new PivotPointStrategy(SymbolName, TakeProfitAtPivot, pivotPoints, Symbol.Bid, Symbol.Ask, positions, orders, bars);
            ManagePositions(changeInstructions);
        }

    }
}

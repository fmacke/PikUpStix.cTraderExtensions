using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.PivotPointBounce;
using cAlgo.API;
using cAlgo.API.Internals;
using Indicators;
using Application.Business.Indicator;
using cAlgo.API.Indicators;

namespace FXProBridge.Robots
{
    public class PivotPointBounceRobot : PositionManager
    {
        [Parameter("Take Profit at Pivot", DefaultValue = "true")]
        public bool TakeProfitAtPivot { get; set; }

        [Parameter("UseAdxMinFilter", DefaultValue = "false", Group = "ADX Filter")]
        public bool UseAdxMinimumFilter { get; set; }
        [Parameter("Adx Minimum Threshold to open position", DefaultValue = 20, Step = 1, MaxValue = 30, MinValue = 15, Group = "ADX Filter")]
        public int AdxMinimumThreshold { get; set; }

        [Parameter("Adx Maximum Threshold to open position", DefaultValue = 5, Step = 1, MaxValue = 20, MinValue = 0, Group = "ADX Filter")]
        public int AdxMaximumThreshold { get; set; }
        [Parameter("UseAdxMinFilter", DefaultValue = "false", Group = "ADX Filter")]
        public bool UseAdxMaximumFilter { get; set; }

        private PivotPointIndicator _pivotPointIndicator;
        private AverageDirectionalMovementIndexRating _adxIndicator;
        private RelativeStrengthIndex _rsiIndicator;

        protected override void OnStart()
        {
            _pivotPointIndicator = Indicators.GetIndicator<PivotPointIndicator>();
            _adxIndicator = Indicators.AverageDirectionalMovementIndexRating(14);
            _rsiIndicator = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14);
            base.OnStart();
        }
        protected override void OnBar()
        {
            var testParams = ResultsCapture.TestParams;
            var currentTime = Bars.OpenTimes.LastValue;
            if (currentTime.Hour >= 8 && currentTime.Hour < 13)
                RunStrategy(currentTime);
            base.OnBar();
        }
        private void RunStrategy(DateTime cursorDate)
        {
            var rsi = _rsiIndicator.Result.LastValue;
            var pivotPoints = new PivotPoints(cursorDate,
                _pivotPointIndicator.Pivot.LastValue, 
                _pivotPointIndicator.Support1.LastValue, 
                _pivotPointIndicator.Resistance1.LastValue, 
                _pivotPointIndicator.Support2.LastValue, 
                _pivotPointIndicator.Resistance2.LastValue);
            if (pivotPoints == null)
                return;
            var adxValues = new AdxScores(_adxIndicator.ADX.LastValue, _adxIndicator.ADXR.LastValue, _adxIndicator.DIMinus.LastValue, _adxIndicator.DIPlus.LastValue);
           

            var positions = PositionConvert.ConvertPosition(Positions);
            var bars = BarConvert.ConvertBars(Bars);
            var orders = PendingOrderConvert.ConvertOrders(PendingOrders);
            
            var changeInstructions = new PivotPointStrategy(cursorDate, SymbolName, TakeProfitAtPivot, pivotPoints, 
                UseAdxMinimumFilter, AdxMinimumThreshold, UseAdxMaximumFilter, AdxMaximumThreshold, adxValues, 
                Symbol.Bid, Symbol.Ask, positions, orders, bars, rsi);
            ManagePositions(changeInstructions);
        }

    }
}

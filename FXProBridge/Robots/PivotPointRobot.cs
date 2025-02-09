using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.PivotPointBounce;
using cAlgo.API;
using Indicators;
using Application.Business.Indicator;
using cAlgo.API.Indicators;
using Application.Business;
using FXProBridge.Capture;

namespace FXProBridge.Robots
{
    public class PivotPointRobot : PositionManager
    {
        [Parameter("ForecastThreshold", DefaultValue = 1, Step = 0.1, MaxValue = 1, MinValue = 0, Group = "Strategy")]
        public double ForecastThreshold { get; set; }
        [Parameter("Confirming Forecast Threshold", DefaultValue = 0.5, Step = 0.1, MaxValue = 1, MinValue = 0, Group = "Strategy")]
        public double ConfirmingForecastThreshold { get; set; }
        [Parameter("Take Profit at Pivot", DefaultValue = "true", Group = "Strategy")]
        public bool TakeProfitAtPivot { get; set; }
        [Parameter("RiskPerTrade", DefaultValue = 2, Step = 0.5, MaxValue = 20, MinValue = 0.5, Group = "Strategy")]
        public double RiskPerTrade { get; set; }


        [Parameter("Enable RSI", DefaultValue = "true", Group = "Confirming Signals")]
        public bool EnableRSI { get; set; }
        [Parameter("UseAdx", DefaultValue = "true", Group = "Confirming Signals")]
        public bool UseAdx { get; set; }
        [Parameter("Adx Low Threshold", DefaultValue = 20, Step = 2, MaxValue = 24, MinValue = 0, Group = "Confirming Signals")]
        public int AdxLowThreshold { get; set; }
        [Parameter("Adx High Threshold", DefaultValue = 25, Step = 2, MaxValue = 40, MinValue = 24, Group = "Confirming Signals")]
        public int AdxHighThreshold { get; set; }


        private PivotPointIndicator _pivotPointIndicator;
        private AverageDirectionalMovementIndexRating _adxIndicator;
        private RelativeStrengthIndex _rsiIndicator;

        protected override void OnStart()
        {
            TestParams = RobotProperties.GetRobotProperties(this);
            _pivotPointIndicator = Indicators.GetIndicator<PivotPointIndicator>();
            if (UseAdx)
            {
                if (AdxLowThreshold > AdxHighThreshold)
                    throw new System.Exception("AdxLowThreshold must be less than AdxHighThreshold");
                _adxIndicator = Indicators.AverageDirectionalMovementIndexRating(14);
            }
            if (EnableRSI)
                _rsiIndicator = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14);
            base.OnStart();
        }
        protected override void OnBar()
        {
            var testParams = ResultsCapture.TestParams;
            var currentTime = Bars.OpenTimes.LastValue;
            if (currentTime.Hour >= 3 && currentTime.Hour < 14)
                RunStrategy(currentTime);
            base.OnBar();
        }
        private void RunStrategy(DateTime cursorDate)
        {
            var signals = new List<ISignal>();
            if (UseAdx)
            {
                signals.Add(new AdxScores(_adxIndicator.ADX.LastValue, _adxIndicator.ADXR.LastValue,
                _adxIndicator.DIMinus.LastValue, _adxIndicator.DIPlus.LastValue, SymbolName, AdxLowThreshold, AdxHighThreshold));
            }
            if (EnableRSI)
            {
                signals.Add(new RSI(_rsiIndicator.Result.LastValue, SymbolName));
            }
            var confirmingSignals = new ConfirmingSignals(signals);
            var props  = new CurrentMarketInfo();
            props.Positions = PositionConvert.ConvertPosition(Positions);
            props.Orders = PendingOrderConvert.ConvertOrders(PendingOrders);
            props.Bars = BarConvert.ConvertBars(Bars);
            props.SymbolName = SymbolName;
            props.AccountBalance = Account.Balance;            
            props.CursorDate = cursorDate;
            props.Ask = Symbol.Ask;
            props.Bid = Symbol.Bid;
            props.PipSize = Symbol.PipSize;
            var changeInstructions = new PivotPointConfirmStrategy(props, confirmingSignals, ForecastThreshold, ConfirmingForecastThreshold, 
                new PivotPoints(cursorDate,
                _pivotPointIndicator.Pivot.LastValue,
                _pivotPointIndicator.Support1.LastValue,
                _pivotPointIndicator.Resistance1.LastValue,
                _pivotPointIndicator.Support2.LastValue,
                _pivotPointIndicator.Resistance2.LastValue), RiskPerTrade/100);
            ManagePositions(changeInstructions);
        }

    }
}

using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.PivotPointBounce;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.Indicators;
using Application.Business.Indicator;

namespace FXProBridge.Robots
{
    public class PivotPointBounceRobot : PositionManager
    {
        //private Bars _differentTimeframeSeries;
        private PivotPointIndicator _pivotPointIndicator;
        private PivotPoints _pivotPoints;
        protected override void OnStart()
        {
            _pivotPointIndicator = Indicators.GetIndicator<PivotPointIndicator>();
            //_differentTimeframeSeries = MarketData.GetBars(TimeFrame.Daily);
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
            var yesterday = cursorDate.AddDays(-1);
            //_pivotPointIndicator.Calculate(_pivotPointIndicator.Pivot.Count); 
            var pivotPoints = new PivotPoints(cursorDate, 
                _pivotPointIndicator.Pivot.LastValue, 
                _pivotPointIndicator.Support1.LastValue, 
                _pivotPointIndicator.Resistance1.LastValue, 
                _pivotPointIndicator.Support2.LastValue, 
                _pivotPointIndicator.Resistance2.LastValue);
            //var pivotPoints = _pivotPointIndicator.PivotPoints.FirstOrDefault(x => x.Date.Year == yesterday.Year
            //    && x.Date.Month == yesterday.Month 
            //    && x.Date.Day == yesterday.Day);
            if (pivotPoints == null)
                return; 
            var positions = PositionConvert.ConvertPosition(Positions);
            var bars = BarConvert.ConvertBars(Bars);
            var orders = PendingOrderConvert.ConvertOrders(PendingOrders);
            var changeInstructions = new PivotPointStrategy(SymbolName, pivotPoints, Symbol.Bid, Symbol.Ask, positions, orders);
            ManagePositions(changeInstructions);
        }

    }
}

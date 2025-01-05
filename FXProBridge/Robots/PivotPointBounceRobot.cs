using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.PivotPointBounce;
using cAlgo.API;
using cAlgo.API.Internals;
using DocumentFormat.OpenXml.Bibliography;
using System.Runtime.Intrinsics.X86;
using cAlgo.Indicators;

namespace FXProBridge.Robots
{
    public class PivotPointBounceRobot : PositionManager
    {
        private Bars _differentTimeframeSeries;
        private PivotPointIndicator _pivotPointIndicator;
        protected override void OnStart()
        {
            _pivotPointIndicator = Indicators.GetIndicator<PivotPointIndicator>();
            _differentTimeframeSeries = MarketData.GetBars(TimeFrame.Daily);
            base.OnStart();
        }
        protected override void OnBar()
        {

            var testParams = ResultsCapture.TestParams;
            var currentTime = Bars.OpenTimes.LastValue;
            if (currentTime.Hour == 8)
                RunStrategy();
            base.OnBar();
        }
        private void RunStrategy()
        {
            _pivotPointIndicator.Calculate(_pivotPointIndicator.Pivot.Count - 1);

            var positions = PositionConvert.ConvertPosition(Positions);
            var bars = BarConvert.GetHistoData(Bars);
            var orders = PendingOrderConvert.ConvertOrders(PendingOrders);
            double previousHigh = _differentTimeframeSeries.HighPrices.Last(1);

            //double previousHigh = _differentTimeframeSeries.HighPrices.Last(1);
            double previousLow = _differentTimeframeSeries.LowPrices.Last(1);
            double previousClose = _differentTimeframeSeries.ClosePrices.Last(1);
            var changeInstructions = new PivotPointStrategy(SymbolName, previousHigh,
                previousLow, previousClose, Symbol.Bid, Symbol.Ask, positions, orders);
            ManagePositions(changeInstructions);
        }

    }
}

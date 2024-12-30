using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.PivotPointBounce;

namespace FXProBridge.Robots
{
    public class PivotPointBounceRobot : PositionManager
    {
        protected override void OnBar()
        {
            var testParams = ResultsCapture.TestParams;
            var positions = PositionConvert.ConvertPosition(Positions); 
            var bars = BarConvert.GetHistoData(Bars);
            double previousHigh = Bars.HighPrices.Last(1);
            double previousLow = Bars.LowPrices.Last(1);
            double previousClose = Bars.ClosePrices.Last(1);
            var changeInstructions = new PivotPointStrategy(previousHigh, previousLow, previousClose, 
                Bars.LastBar.Open, Bars.LastBar.Open, positions);
            ManagePositions(changeInstructions);
            base.OnBar();
        }
    }
}

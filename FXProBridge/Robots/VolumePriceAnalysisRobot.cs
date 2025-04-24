using Application.Business.Indicator.Signal;
using Application.Business.Market;
using FXProBridge.Common;
using FXProBridge.DataConversions;

namespace FXProBridge.Robots
{
    public class VolumePriceAnalysisRobot : PositionManager
    {
       protected override void OnBar()
        {
            var bars = BarConvert.ConvertBars(Bars);
            var marketInfo = new MarketInfo(Bars.OpenTimes.LastValue,
                Symbol.Bid,
                Symbol.Ask,
                PositionConvert.ConvertPosition(Positions),
                bars,
                SymbolName,
                SymbolName,
                Account.Equity,
                Symbol.PipSize, 1, new ConfirmingSignals(new List<ISignal>()), Domain.Enums.TimeFrame.H1);

            var strategy = new VolumePriceAnalysis();
            ManagePositions(strategy.Run(new List<IMarketInfo> { marketInfo }));
            base.OnBar();
        }
    }
}

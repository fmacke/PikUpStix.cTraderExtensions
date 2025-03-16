using System.Data;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using FXProBridge.Capture;
using Indicators;

namespace Robots
{
    public abstract class EngulfingTestRobot : RobotTestWrapper
    {
        private double _volumeInUnits;
        // Common settings
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }
        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }
        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }
        [Parameter("Label", DefaultValue = "engulfingTest")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceSeries { get; set; }

        private CandlePatternIdentifiersBase engulfCheckIndicator;
        private MovingAverage fastMa;

        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);
            }
        }

        protected override void OnStart()
        {
            var result = System.Diagnostics.Debugger.Launch();
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);
            engulfCheckIndicator = Indicators.GetIndicator<CandlePatternIdentifiersBase>(true, 150);
            fastMa = Indicators.MovingAverage(SourceSeries, 10, MovingAverageType.Simple);
        }

        protected override void OnBarClosed()
        {
            if (engulfCheckIndicator.IsBullishEngulfingPattern(Bars.Count - 1))
            {
                Print("Bearish Engulfing Pattern Identified");
                ClosePositions(TradeType.Sell);
                var stopLoss = Math.Sqrt(Math.Pow(Bars.HighPrices[0] - Symbol.Bid, 2)) * Math.Pow(10, Symbol.Digits);
                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, stopLoss, TakeProfitInPips);
                Print("Bearish Engulfing Pattern Operations Complete");
            }
            if (engulfCheckIndicator.IsBearishEngulfingPattern(Bars.Count - 1))
            {
                Print("Bullish Engulfing Pattern");
                ClosePositions(TradeType.Buy);
                var stopLosss = Math.Sqrt(Math.Pow(Symbol.Bid - Bars.LowPrices[0], 2)) * Math.Pow(10, Symbol.Digits);
                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, stopLosss, TakeProfitInPips);
                Print("Bullish Engulfing Pattern Operations Complete");
            }
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in BotPositions.Where(t => t.TradeType == tradeType))
            {
                ClosePosition(position);
            }
        }
    }
}
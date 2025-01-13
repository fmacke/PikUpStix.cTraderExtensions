using cAlgo.API;
using cAlgo.API.Internals;

namespace Indicators
{
    /// <summary>
    ///     CandleIds - CandleStick Patterns Identifier
    /// </summary>
    /// <remarks>
    ///     Indicator for identifying engulfing candlestick patterns in financial trading. Each 
    ///     engulfing candle stick is highlighted on the chart.  The class 
    ///     inherits from cAlgo.API.Indicator class
    /// </remarks>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public abstract class CandlePatternIdentifiersBase : Indicator
    {
        [Parameter("Enable", Group = "Highlight Engulfing Pattern", DefaultValue = true)]
        public bool EngulfingPatterns { get; set; }
        [Parameter("Bullish Color", DefaultValue = "Blue")]
        public string BullishColor { get; set; }
        [Parameter("Bearish Color", DefaultValue = "Red")]
        public string BearishColor { get; set; }
        public Dictionary<int, bool> BearishEngulfments { get; set; } = new Dictionary<int, bool>();
        public Dictionary<int, bool> BullishEngulfments { get; set; } = new Dictionary<int, bool>();

        public override void Calculate(int index)
        {
            HighlightEngulfingPatterns(index);
        }

        private void HighlightEngulfingPatterns(int index)
        {
            if (EngulfingPatterns & index > 0)
            {
                if (IsBullishEngulfingPattern(index))
                {
                    Chart.DrawRectangle("bullish" + index, index - 1, Bars.LowPrices[index - 1], index, Bars.HighPrices[index], BullishColor, 3);
                    BullishEngulfments.Add(index, true);
                    BearishEngulfments.Add(index, false);
                }
                if (IsBearishEngulfingPattern(index))
                {
                    Chart.DrawRectangle("bearish" + index, index - 1, Bars.HighPrices[index - 1], index, Bars.LowPrices[index], BearishColor, 3);
                    BullishEngulfments.Add(index, false);
                    BearishEngulfments.Add(index, true);
                }
            }
        }

        public bool IsBullishEngulfingPattern(int i)
        {
            return Bars.OpenPrices[i - 1] > Bars.ClosePrices[i - 1]
                && Bars.OpenPrices[i] < Bars.ClosePrices[i - 1]
                && Bars.ClosePrices[i] > Bars.OpenPrices[i - 1];
        }
        public bool IsBearishEngulfingPattern(int i)
        {
            return Bars.ClosePrices[i - 1] > Bars.OpenPrices[i - 1]
                && Bars.OpenPrices[i] > Bars.ClosePrices[i - 1]
                && Bars.ClosePrices[i] < Bars.OpenPrices[i - 1];
        }
    }
}
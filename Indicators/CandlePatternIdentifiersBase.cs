using cAlgo.API;
using cAlgo.API.Internals;

namespace PikUpStix.cTraderExtenstions.Indicators
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
    public class CandlePatternIdentifiersBase : Indicator
    {
        //[Parameter("Source")]
        //public DataSeries Source { get; set; }
        [Parameter("Enable", Group = "Highlight Engulfing Pattern", DefaultValue = true)]
        public bool EngulfingPatterns { get; set; }
        [Parameter("SignalYAdjust", Group = "Highlight Engulfing Pattern", DefaultValue = 150)]
        public int SignalYAdjust { get; set; }

        //public bool IsBearEngulfingPattern = false;
        //public bool IsBullEngulfingPattern = false;

        public override void Calculate(int index)
        {
            HighlightEngulfingPatterns(index);
        }

        private void HighlightEngulfingPatterns(int value)
        {
            for (int i = Chart.FirstVisibleBarIndex; i <= Chart.LastVisibleBarIndex; i++)
            {
                if (EngulfingPatterns & i > 0)
                {
                    if (IsBullishEngulfingPattern(i))
                    {
                        //IsBullEngulfingPattern = true;
                        HighlightEngulfing("Bullish Engulfing", i, Bars.HighPrices[i] + (Symbol.TickSize * SignalYAdjust), Color.Green, ChartIconType.UpArrow);
                    }
                    if (IsBearishEngulfingPattern(i))
                    {
                        //IsBearEngulfingPattern = true;
                        HighlightEngulfing("Bearish Engulfing", i, Bars.LowPrices[i] - (Symbol.TickSize * SignalYAdjust), Color.Red, ChartIconType.DownArrow);
                    }
                }
            }
        }

        private void HighlightEngulfing(string text, int barIndex, double yAxisPosition, Color color, ChartIconType arrowDirection)
        {
            var textId = string.Format("TextId_{0,1}", barIndex, text);
            Chart.DrawText(textId, text, barIndex, yAxisPosition, color);
            Chart.DrawIcon(string.Format("BE_Id_{0,1}", barIndex, text), arrowDirection, barIndex, yAxisPosition, color);
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
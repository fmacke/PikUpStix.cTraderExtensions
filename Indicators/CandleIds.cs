using cAlgo.API;
using cAlgo.API.Internals;
using System.Drawing;

namespace cAlgo
{
    /// <summary>
    ///     CandleIds - CandleStick Patterns Identifier
    /// </summary>
    /// <remarks>
    ///     Indicator for identifying candlestick patterns in financial trading. The class 
    ///     inherits from cAlgo.API.Indicator class
    /// </remarks>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CandleIds : Indicator
    {
        [Parameter("Highlight Engulfing Pattern", Group = "CandleStick Patterns", DefaultValue = true)]
        public bool EngulfingPatterns { get; set; }

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
                        HighlightEngulfing("Bullish Engulfing", i, Bars.HighPrices[i] + (Symbol.TickSize * 20), Color.Green, ChartIconType.UpArrow);
                    if (IsBearishEngulfingPattern(i))
                        HighlightEngulfing("Bearish Engulfing", i, Bars.ClosePrices[i] - (Symbol.TickSize * 20), Color.Red, ChartIconType.DownArrow);
                }
            }
        }

        private void HighlightEngulfing(string text, int barIndex, double yAxisPosition, Color color, ChartIconType arrowDirection)
        {
            var textId = string.Format("TextId_{0,1}", barIndex, text);
            Chart.DrawText(textId, text, barIndex, yAxisPosition, color);
            Chart.DrawIcon(string.Format("BE_Id_{0,1}", barIndex, text), arrowDirection, barIndex, yAxisPosition, color);
        }

        private bool IsBullishEngulfingPattern(int i)
        {
            return Bars.OpenPrices[i - 1] > Bars.ClosePrices[i - 1]
                && Bars.OpenPrices[i] < Bars.ClosePrices[i - 1]
                && Bars.ClosePrices[i] > Bars.OpenPrices[i - 1];
        }
        private bool IsBearishEngulfingPattern(int i)
        {
            return Bars.ClosePrices[i - 1] > Bars.OpenPrices[i - 1]
               && Bars.OpenPrices[i] > Bars.ClosePrices[i - 1]
               && Bars.ClosePrices[i] < Bars.OpenPrices[i - 1];
        }
    }
}
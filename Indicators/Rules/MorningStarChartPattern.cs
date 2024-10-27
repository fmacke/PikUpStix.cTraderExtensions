using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicators.Rules
{
    public class MorningStarChartPattern
    {
        //public bool IsMorningStar(List<Candlestick> candlesticks)
        //{
        //    if (candlesticks.Count < 3)
        //        return false;

        //    var firstCandle = candlesticks[0];
        //    var secondCandle = candlesticks[1];
        //    var thirdCandle = candlesticks[2];

        //    // Condition 1: First candle is bearish
        //    bool isFirstBearish = firstCandle.Close < firstCandle.Open;

        //    // Condition 2: Second candle is small and gaps down
        //    bool isSecondSmall = Math.Abs(secondCandle.Close - secondCandle.Open) < Math.Abs(firstCandle.Close - firstCandle.Open);
        //    bool isSecondGapDown = secondCandle.Open > firstCandle.Close;

        //    // Condition 3: Third candle is bullish and closes above the midpoint of the first candle
        //    bool isThirdBullish = thirdCandle.Close > thirdCandle.Open;
        //    bool isThirdAboveMidpoint = thirdCandle.Close > (firstCandle.Close + firstCandle.Open) / 2;

        //    // Check if all conditions are met
        //    return isFirstBearish && isSecondSmall && isSecondGapDown && isThirdBullish && isThirdAboveMidpoint;
        //}

    }
}

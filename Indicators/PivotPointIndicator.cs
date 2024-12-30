//using cAlgo.API;
//using cAlgo.API.Indicators;
//using cAlgo.API.Internals;
//using System.Collections.Generic;

//namespace cAlgo.Indicators
//{
//    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
//    public class PivotPointIndicator
//    {
//        [Output("Pivot", LineColor = "Yellow")]
//        public IndicatorDataSeries Pivot { get; set; }

//        [Output("Support1", LineColor = "Green")]
//        public IndicatorDataSeries Support1 { get; set; }

//        [Output("Resistance1", LineColor = "Red")]
//        public IndicatorDataSeries Resistance1 { get; set; }

//        [Output("Support2", LineColor = "Green")]
//        public IndicatorDataSeries Support2 { get; set; }

//        [Output("Resistance2", LineColor = "Red")]
//        public IndicatorDataSeries Resistance2 { get; set; }

//        protected override void Initialize()
//        {
//            // Initialization logic here
//        }

//        public override void Calculate(int index)
//        {
//            double High = Bars.HighPrices.Last(1);
//            double Close = Bars.ClosePrices.Last(1);
//            double Low = Bars.LowPrices.Last(1);
//            double currentBid = Symbol.Bid;
//            double currentAsk = Symbol.Ask;
//            double currentPrice = (currentBid + currentAsk) / 2;

//            double pivot = (High + Low + Close) / 3;
//            double support1 = 2 * pivot - High;
//            double resistance1 = 2 * pivot - Low;
//            double support2 = pivot - (High - Low);
//            double resistance2 = pivot + (High - Low);

//            Pivot[index] = pivot;
//            Support1[index] = support1;
//            Resistance1[index] = resistance1;
//            Support2[index] = support2;
//            Resistance2[index] = resistance2;
//        }
//    }
//}

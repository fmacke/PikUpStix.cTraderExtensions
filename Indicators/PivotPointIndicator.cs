using Application.Business.Indicator;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PivotPointIndicator : Indicator
    {
        [Output("Pivot", LineColor = "Blue")]
        public IndicatorDataSeries Pivot { get; set; }

        [Output("Support1", LineColor = "Green")]
        public IndicatorDataSeries Support1 { get; set; }

        [Output("Resistance1", LineColor = "Red")]
        public IndicatorDataSeries Resistance1 { get; set; }

        [Output("Support2", LineColor = "Green")]
        public IndicatorDataSeries Support2 { get; set; }

        [Output("Resistance2", LineColor = "Red")]
        public IndicatorDataSeries Resistance2 { get; set; }

        private Bars _dailyTimeFrame;

        public PivotPoints PivotPoints { get; private set; }

        protected override void Initialize()
        {
            _dailyTimeFrame = MarketData.GetBars(TimeFrame.Daily);
        }

        public override void Calculate(int index)
        {
            var currentDate = Bars.OpenTimes.LastValue;
            var yesterday = currentDate.AddDays(-1);

            for (int i = 0; i < _dailyTimeFrame.ClosePrices.Count; i++)
            {
                if (yesterday.Year == _dailyTimeFrame.OpenTimes.Last(i).Year
                    && yesterday.Month == _dailyTimeFrame.OpenTimes.Last(i).Month
                    && yesterday.Day == _dailyTimeFrame.OpenTimes.Last(i).Day)
                {
                    PivotPoints = new PivotPoints(_dailyTimeFrame.HighPrices.Last(i), _dailyTimeFrame.LowPrices.Last(i), _dailyTimeFrame.ClosePrices.Last(i));

                    Pivot[index] = PivotPoints.Pivot;
                    Support1[index] = PivotPoints.Support1;
                    Resistance1[index] = PivotPoints.Resistance1;
                    Support2[index] = PivotPoints.Support2;
                    Resistance2[index] = PivotPoints.Resistance2;
                }
            }
        }
    }
}

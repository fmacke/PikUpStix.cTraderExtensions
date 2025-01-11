using Application.Business.Indicator;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PivotPointIndicator : Indicator
    {
        [Output("Pivot", LineColor = "Blue", Thickness = 5)]
        public IndicatorDataSeries Pivot { get; set; }

        [Output("Support1", LineColor = "Green")]
        public IndicatorDataSeries Support1 { get; set; }

        [Output("Resistance1", LineColor = "Red")]
        public IndicatorDataSeries Resistance1 { get; set; }

        [Output("Support2", LineColor = "Green")]
        public IndicatorDataSeries Support2 { get; set; }

        [Output("Resistance2", LineColor = "Red")]
        public IndicatorDataSeries Resistance2 { get; set; }

        public Dictionary<int, DateTime> Dates{ get; set; } = new Dictionary<int, DateTime>();

        private Bars _dailyTimeFrame;

        protected override void Initialize()
        {
            _dailyTimeFrame = MarketData.GetBars(TimeFrame.Daily);
        }
        public override void Calculate(int index)
        {
            var yesterday = Bars.OpenTimes.LastValue.AddDays(-1);
            var found = false;
            var count = 0;
            while (found == false)
            {
                if (count > 4)
                {
                    break;
                }
                found = CalculatePivots(yesterday, index);
                if (found == false)
                {
                    yesterday = yesterday.AddDays(-1);
                    count++;
                }
            }
        }
        private bool CalculatePivots(DateTime forDate, int index)
        {
            var found = false;
            if (index > 1)
            {                
                for (int i = 0; i < _dailyTimeFrame.Count; i++)
                {
                    var currentBar = _dailyTimeFrame.OpenTimes[i];
                    if (IsDateMatch(forDate, currentBar))
                    {
                        var high = _dailyTimeFrame.HighPrices[i];
                        var low = _dailyTimeFrame.LowPrices[i];
                        var close = _dailyTimeFrame.ClosePrices[i];
                        var pivotPoints = new PivotPoints(forDate, high, low, close);
                        Pivot[index] = pivotPoints.Pivot;
                        Support1[index] = pivotPoints.Support1;
                        Resistance1[index] = pivotPoints.Resistance1;
                        Support2[index] = pivotPoints.Support2;
                        Resistance2[index] = pivotPoints.Resistance2;
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        private static bool IsDateMatch(DateTime forDate, DateTime currentBar)
        {
            return currentBar.Year == forDate.Year
                                && currentBar.Month == forDate.Month
                                && currentBar.Day == forDate.Day;
        }
    }
}

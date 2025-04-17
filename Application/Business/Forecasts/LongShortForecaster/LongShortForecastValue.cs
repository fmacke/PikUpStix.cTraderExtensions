using Application.Business.Market;
using Domain.Entities;

namespace Application.Business.Forecasts.LongShortForecaster
{
    public class LongShortForecastValue : ForecastValue
    {
        protected HistoricalData CursorDatePriceData { get; private set; }

        public LongShortForecastValue(IMarketInfo marketInfo)
           : base(marketInfo)
        {
            CursorDatePriceData = marketInfo.Bars[marketInfo.Bars.FindIndex(x => x.Date == marketInfo.CursorDate)];
            AskingPrice = marketInfo.Ask;
            BiddingPrice = marketInfo.Bid;
        }
        public new double CalculateForecast()
        {
            Forecast = 0;
                if (CursorDatePriceData.Date.DayOfWeek.Equals(DayOfWeek.Monday))
                    Forecast = 20;
                if (CursorDatePriceData.Date.DayOfWeek.Equals(DayOfWeek.Friday))
                    Forecast = -20;
            return Forecast;
        }
    }
}

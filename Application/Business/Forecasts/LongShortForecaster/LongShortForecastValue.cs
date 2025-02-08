using Domain.Entities;

namespace Application.Business.Forecasts.LongShortForecaster
{
    public class LongShortForecastValue : ForecastValue
    {
        protected HistoricalData CursorDatePriceData { get; private set; }

        public LongShortForecastValue(DateTime cursorDate, List<HistoricalData> priceData, double askingPrice, double biddingPrice)
           : base(cursorDate, priceData, askingPrice, biddingPrice)
        {
            CursorDatePriceData = priceData[priceData.FindIndex(x => x.Date == cursorDate)];
            AskingPrice = askingPrice;
            BiddingPrice = biddingPrice;
        }
        public new double CalculateForecast()
        {
            Forecast = 0;
            // InstrumentId = CursorDatePriceData.Instrument.InstrumentId;

            if (CursorDatePriceData.Date.HasValue)
            {
                if (Convert.ToDateTime(CursorDatePriceData.Date).DayOfWeek.Equals(DayOfWeek.Monday))
                    Forecast = 20;
                if (Convert.ToDateTime(CursorDatePriceData.Date).DayOfWeek.Equals(DayOfWeek.Friday))
                    Forecast = -20;
            }
            return Forecast;
        }
    }
}

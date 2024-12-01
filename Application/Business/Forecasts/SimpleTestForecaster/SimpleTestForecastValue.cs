using Domain.Entities;

namespace Application.Business.Forecasts.SimpleTestForecaster
{
    public class SimpleTestForecastValue : ForecastValue
    {
        protected HistoricalData CursorDatePriceData { get; private set; }
        protected HistoricalData LastFridayPriceData { get; private set; }
        protected HistoricalData LastMondayPriceData { get; private set; }

        protected DateTime CursorDate { get; private set; }

        public SimpleTestForecastValue(DateTime cursorDate, List<HistoricalData> priceData, double askingPrice, double biddingPrice)
            : base(cursorDate, priceData, askingPrice, biddingPrice)
        {
            CursorDate = cursorDate;

            int index = 0;
            index = priceData.FindIndex(x => x.Date == cursorDate);

            if (cursorDate.DayOfWeek.Equals(DayOfWeek.Monday))
            {
                CursorDatePriceData = priceData[index];
            }
            else
            {
                CursorDatePriceData = priceData[index];
                //CursorDatePriceData = GetLast(DayOfWeek.Monday, index, priceData);
                index = priceData.FindIndex(x => x.Date == GetLast(DayOfWeek.Monday, index, priceData).Date);
            }
            LastFridayPriceData = GetLast(DayOfWeek.Friday, index, priceData);
            LastMondayPriceData = GetLast(DayOfWeek.Monday, index, priceData);
        }

        public new decimal CalculateForecast()
        {
            Forecast = 0;

            //InstrumentId = CursorDatePriceData.InstrumentId;
            //CurrentPrice = Convert.ToDecimal(CursorDatePriceData.OpenPrice);

            try
            {
                if (LastFridayPriceData.ClosePrice > LastMondayPriceData.ClosePrice)
                {
                    Forecast = 20;
                }
                if (LastFridayPriceData.ClosePrice < LastMondayPriceData.ClosePrice)
                {
                    Forecast = -20;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("This error occurred: " + ex.Message);
            }

            return Forecast;
        }

        private HistoricalData GetLast(DayOfWeek day, int index, List<HistoricalData> priceData)
        {
            HistoricalData hd = new HistoricalData();
            for (var x = index - 1; x >= 0; x--)
            {
                if (Convert.ToDateTime(priceData[x].Date).DayOfWeek.Equals(day))
                {
                    return priceData[x];
                }
            }
            return hd;
        }
    }
}

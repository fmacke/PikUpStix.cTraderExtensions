using Application.Business.Market;
using Domain.Entities;

namespace Application.Business.Forecasts.SimpleTestForecaster
{
    public class SimpleTestForecastValue : ForecastValue
    {
        protected HistoricalData CursorDatePriceData { get; private set; }
        protected HistoricalData LastFridayPriceData { get; private set; }
        protected HistoricalData LastMondayPriceData { get; private set; }

        protected DateTime CursorDate { get; private set; }

        public SimpleTestForecastValue(IMarketInfo marketInfo)
            : base(marketInfo)
        {
            CursorDate = marketInfo.CursorDate;

            int index = 0;
            index = marketInfo.Bars.FindIndex(x => x.Date == CursorDate);

            if (CursorDate.DayOfWeek.Equals(DayOfWeek.Monday))
            {
                CursorDatePriceData = marketInfo.Bars[index];
            }
            else
            {
                CursorDatePriceData = marketInfo.Bars[index];
                //CursorDatePriceData = GetLast(DayOfWeek.Monday, index, priceData);
                index = marketInfo.Bars.FindIndex(x => x.Date == GetLast(DayOfWeek.Monday, index, marketInfo.Bars).Date);
            }
            LastFridayPriceData = GetLast(DayOfWeek.Friday, index, marketInfo.Bars);
            LastMondayPriceData = GetLast(DayOfWeek.Monday, index, marketInfo.Bars);
        }

        public new double CalculateForecast()
        {
            Forecast = 0;

            //InstrumentId = CursorDatePriceData.InstrumentId;
            //CurrentPrice = Convert.ToDecimal(CursorDatePriceData.OpenPrice);

            try
            {
                if (LastFridayPriceData.ClosePrice > LastMondayPriceData.ClosePrice)
                    Forecast = 20;
                if (LastFridayPriceData.ClosePrice < LastMondayPriceData.ClosePrice)
                    Forecast = -20;
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

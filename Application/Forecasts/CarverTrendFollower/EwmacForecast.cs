using Domain.Entities;

namespace PikUpStix.Trading.Forecast
{
    public class EwmacForecast
    {
        private readonly ForecastScaling _forecastScaling;

        public EwmacForecast(int slowPeriod, int fastPeriod, List<HistoricalData> priceData)
        {
            SlowPeriod = slowPeriod;
            FastPeriod = fastPeriod;
            HistoricalPrices = priceData;
            if (priceData.Count < slowPeriod)
                throw new Exception(
                    "EwmacForecast:  The slow period parameter exceeds the amount of price data available.");
            _forecastScaling = new ForecastScaling();
        }

        public int SlowPeriod { get; private set; }
        public int FastPeriod { get; private set; }
        public List<HistoricalData> HistoricalPrices { get; private set; }

        public string Forecastname
        {
            get { return GetType() + "_" + SlowPeriod + "_" + FastPeriod; }
        }



    }
}
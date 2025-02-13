using Application.Business.Forecasts.CarverTrendFollower;
using Domain.Entities;

namespace Application.Business.Indicator.Signal
{
    public class EWMAC : ISignal
    {
        public string Instrument { get; set; }
        public string Name { get; set; }
        public double Forecast { get; set; }
        private readonly ForecastScaling _forecastScaling;
        public List<ForecastElement> ForecastData { get; set; }
        public double ShortScalar { get; private set; }
        public double MediumScalar { get; private set; }
        public double LongScalar { get; private set; }
        public List<HistoricalData> PriceData { get; set; } = new List<HistoricalData>();

        public EWMAC(string symbolName, List<HistoricalData> priceData, double askingPrice, double biddingPrice)
        {
            Instrument = symbolName;
            Name = "EWMAC";
            PriceData = priceData;
            _forecastScaling = new ForecastScaling();
            LoadScalars();
            Forecast = CalculateForecast();
        }

        private void LoadScalars()
        {
            ShortScalar = 0.4;
            MediumScalar = 0.2;
            LongScalar = 0.4;
        }

        public new double CalculateForecast()
        {
            var shortForecast = CalculateScaledForecast(16, 64, 36, 0.99).Forecast;
            var mediumForecast = CalculateScaledForecast(32, 128, 36, 0.99).Forecast;
            var longForecast = CalculateScaledForecast(64, 256, 36, 0.99).Forecast;
            var combinedForecast = new SingleInstrumentCombinedForecast(shortForecast, mediumForecast,
            longForecast, ShortScalar, MediumScalar, LongScalar);
            Forecast = combinedForecast.CombinedForecast;
            return combinedForecast.CombinedForecast;
        }

        public ForecastElement CalculateScaledForecast(int fastPeriod, int slowPeriod, int standardDeviationLookBack,
            double forecastScalar)
        {
            // note the forecastScalar variable seems to do virtually nothing and is then used below for a secondary purpose!
            List<ForecastElement> unscaledForecast = GetUnscaledForecast(fastPeriod, slowPeriod,
                standardDeviationLookBack, forecastScalar);
            forecastScalar = _forecastScaling.GetForecastScalar(unscaledForecast); // to get to average forecast of 10
            ForecastElement fc = unscaledForecast.Last();
            if (!double.IsNaN(fc.Forecast) && !double.IsInfinity(fc.Forecast))
                if (fc.Forecast != 0.0)
                    fc.Forecast = _forecastScaling.CapForecast(fc.Forecast * forecastScalar);
            return fc;
        }

        public List<ForecastElement> GetUnscaledForecast(int fastPeriod, int slowPeriod, int standardDeviationLookBack,
            double forecastScalar)
        {
            double FastDecay = 2 / Convert.ToDouble(fastPeriod + 1);
            double SlowDecay = 2 / Convert.ToDouble(slowPeriod + 1);
            double StandardDeviationDecay = 2 / (standardDeviationLookBack + 1);
            ForecastData = new List<ForecastElement>();
            double previousPrice = 0;
            double previousFastEwma = 0;
            double previousSlowEwma = 0;
            double previousVariance = 0;
            foreach (HistoricalData historicalData in PriceData.OrderBy(x => x.Date))
            {
                double currentPrice = Convert.ToDouble(historicalData.ClosePrice);
                var element = new ForecastElement(Convert.ToDateTime(historicalData.Date), 
                    currentPrice, previousPrice, previousFastEwma, FastDecay,
                    SlowDecay, previousSlowEwma, StandardDeviationDecay,
                    previousVariance, forecastScalar);
                previousPrice = element.Price;
                previousFastEwma = element.EwmaFast;
                previousSlowEwma = element.EwmaSlow;
                previousVariance = element.Variance;
                ForecastData.Add(element);
            }
            return ForecastData;
        }
    }
}

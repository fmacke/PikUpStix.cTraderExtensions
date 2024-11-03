using Domain.Entities;
using PikUpStix.Trading.Forecast;

namespace Application.Forecasts.CarverTrendFollower
{
    public class EwmacForecastValue : ForecastValue
    {
        private readonly ForecastScaling _forecastScaling;

        public EwmacForecastValue(DateTime cursorDate, List<HistoricalData> priceData, double askingPrice, double biddingPrice, List<Test_Parameters> testParameters)
            : base(cursorDate, priceData, askingPrice, biddingPrice)
        {
            _forecastScaling = new ForecastScaling();
            LoadScalars(testParameters);

        }

        private void LoadScalars(List<Test_Parameters> testParameters)
        {
            ShortScalar = 0.4M;
            MediumScalar = 0.2M;
            LongScalar = 0.4M;

            foreach (var parameter in testParameters)
            {
                if (parameter.Name.Equals("ShortScalar[Double]"))
                    ShortScalar = Convert.ToDecimal(parameter.Value);
                if (parameter.Name.Equals("LongScalar[Double]"))
                    LongScalar = Convert.ToDecimal(parameter.Value);
                if (parameter.Name.Equals("MediumScalar[Double]"))
                    MediumScalar = Convert.ToDecimal(parameter.Value);
            }
        }

        public List<ForecastElement> ForecastData { get; set; }
        public decimal ShortScalar { get; private set; }
        public decimal MediumScalar { get; private set; }
        public decimal LongScalar { get; private set; }

        public new decimal CalculateForecast()
        {
            ShortForecast = Convert.ToDecimal(CalculateScaledForecast(16, 64, 36, 0.99).Forecast);
            MediumForecast = Convert.ToDecimal(CalculateScaledForecast(32, 128, 36, 0.99).Forecast);
            LongForecast = Convert.ToDecimal(CalculateScaledForecast(64, 256, 36, 0.99).Forecast);
            var combinedForecast = new SingleInstrumentCombinedForecast(ShortForecast, MediumForecast,
                LongForecast, ShortScalar, MediumScalar, LongScalar);
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
                    fc.Forecast = Convert.ToDouble(_forecastScaling.CapForecast(Convert.ToDecimal(fc.Forecast * forecastScalar)));
            return fc;
        }

        public List<ForecastElement> GetUnscaledForecast(int fastPeriod, int slowPeriod, int standardDeviationLookBack,
            double forecastScalar)
        {
            double FastDecay = 2 / Convert.ToDouble(fastPeriod + 1);
            double SlowDecay = 2 / Convert.ToDouble(slowPeriod + 1);
            double StandardDeviationDecay = 2 / Convert.ToDouble(standardDeviationLookBack + 1);
            ForecastData = new List<ForecastElement>();
            double previousPrice = 0;
            double previousFastEwma = 0;
            double previousSlowEwma = 0;
            double previousVariance = 0;
            foreach (HistoricalData historicalData in PriceData.Where(x => x.Date <= DateTime).OrderBy(x => x.Date))
            {
                double currentPrice = Convert.ToDouble(historicalData.ClosePrice);
                var element = new ForecastElement(Convert.ToDateTime(historicalData.Date), currentPrice
                    , previousPrice, previousFastEwma, FastDecay,
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
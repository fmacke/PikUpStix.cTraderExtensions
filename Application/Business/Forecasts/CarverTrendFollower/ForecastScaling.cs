using Application.Business.Forecasts;

namespace Application.Business.Forecasts.CarverTrendFollower
{
    public class ForecastScaling
    {
        public decimal CapForecast(decimal uncappedForecast)
        {
            if (uncappedForecast > 20)
                return 20;
            if (uncappedForecast < -20)
                return -20;
            return uncappedForecast;
        }

        public decimal GetForecastScalar(List<ForecastValue> forecastValues)
        {
            var total = new decimal(0);
            foreach (ForecastValue forecastValue in forecastValues)
                if (forecastValue.Forecast < 0)
                    total += forecastValue.Forecast * -1;
                else
                    total += forecastValue.Forecast;
            decimal averageForecastValue = total / forecastValues.Count;
            decimal forecastScalar = 10 / averageForecastValue;
            return forecastScalar;
        }

        public double GetForecastScalar(List<ForecastElement> unscaledForecasts)
        {
            double tot = Convert.ToDouble(0.000);

            foreach (ForecastElement forecastValue in unscaledForecasts)
                if (!double.IsNaN(forecastValue.Forecast))
                    if (forecastValue.Forecast < 0)
                        tot += forecastValue.Forecast * Convert.ToDouble(-1);
                    else
                        tot += forecastValue.Forecast;
            double averageForecastValue = tot / unscaledForecasts.Count;
            double forecastScalar = 10 / averageForecastValue;
            return forecastScalar;
        }
    }
}
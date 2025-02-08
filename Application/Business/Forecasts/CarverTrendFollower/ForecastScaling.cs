using Application.Business.Forecasts;

namespace Application.Business.Forecasts.CarverTrendFollower
{
    public class ForecastScaling
    {
        public double CapForecast(double uncappedForecast)
        {
            if (uncappedForecast > 20)
                return 20;
            if (uncappedForecast < -20)
                return -20;
            return uncappedForecast;
        }

        public double GetForecastScalar(List<ForecastValue> forecastValues)
        {
            var total = 0.0;
            foreach (ForecastValue forecastValue in forecastValues)
                if (forecastValue.Forecast < 0)
                    total += forecastValue.Forecast * -1;
                else
                    total += forecastValue.Forecast;
            double averageForecastValue = total / forecastValues.Count;
            double forecastScalar = 10 / averageForecastValue;
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
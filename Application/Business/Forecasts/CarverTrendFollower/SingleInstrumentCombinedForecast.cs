namespace Application.Business.Forecasts.CarverTrendFollower
{
    public class SingleInstrumentCombinedForecast
    {
        public SingleInstrumentCombinedForecast(double shortForecast, double mediumForecast, double longForecast, double shortScalar, double mediumScalar, double longScalar)
        {
            LongForecast = longForecast * longScalar;
            ShortForecast = shortForecast * shortScalar;
            MediumForecast = mediumForecast * mediumScalar;
            CombinedForecast = LongForecast + MediumForecast + ShortForecast;
        }

        public double LongForecast { get; private set; }
        public double MediumForecast { get; private set; }
        public double ShortForecast { get; private set; }
        public double CombinedForecast { get; private set; }
    }
}
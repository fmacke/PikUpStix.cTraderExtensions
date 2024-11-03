namespace Application.Forecasts.CarverTrendFollower
{
    public class SingleInstrumentCombinedForecast
    {
        public SingleInstrumentCombinedForecast(decimal shortForecast, decimal mediumForecast, decimal longForecast, decimal shortScalar, decimal mediumScalar, decimal longScalar)
        {
            LongForecast = longForecast * longScalar;
            ShortForecast = shortForecast * shortScalar;
            MediumForecast = mediumForecast * mediumScalar;
            CombinedForecast = LongForecast + MediumForecast + ShortForecast;
        }

        public decimal LongForecast { get; private set; }
        public decimal MediumForecast { get; private set; }
        public decimal ShortForecast { get; private set; }
        public decimal CombinedForecast { get; private set; }
    }
}
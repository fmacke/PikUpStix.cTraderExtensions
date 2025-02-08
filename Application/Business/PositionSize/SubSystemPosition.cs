namespace Application.Business.PositionSize
{
    public class SubSystemPosition
    {
        private const int SquareRootOfTime = 16;

        public SubSystemPosition(double instrumentForecast, double tradingCapital, double targetVolatility,
            InstrumentPositionSize instrumentDetails)
        {
            if (tradingCapital < 0)
            {
                // setting to zero stops weird negative trading happening ie. if you've got no money, don't place a fucking trade!
                tradingCapital = 0;
            }
            InstrumentForecast = instrumentForecast;
            TradingCapital = tradingCapital;
            TargetVolatility = targetVolatility;
            InstrumentDetails = instrumentDetails;
        }

        public double InstrumentForecast { get; private set; }
        public double TradingCapital { get; private set; }
        public double TargetVolatility { get; private set; }
        public InstrumentPositionSize InstrumentDetails { get; private set; }

        public double AnnualisedCashVolatility
        {
            get { return TradingCapital * TargetVolatility; }
        }

        public double DailyCashVolatilityTarget
        {
            get { return AnnualisedCashVolatility / SquareRootOfTime; }
        }

        public double VolatilityScalar
        {
            get { return DailyCashVolatilityTarget / InstrumentDetails.ValueVolatility; }
        }

        public double GetUnscaledPosition()
        {
            if (InstrumentDetails.ValueVolatility <= 0)
                return 0;
            double unscaledPosition = InstrumentForecast * VolatilityScalar / 10;
            return unscaledPosition;
        }
    }
}
namespace Application.PositionSize
{
    public class SubSystemPosition
    {
        private const int SquareRootOfTime = 16;

        public SubSystemPosition(decimal instrumentForecast, decimal tradingCapital, decimal targetVolatility,
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

        public decimal InstrumentForecast { get; private set; }
        public decimal TradingCapital { get; private set; }
        public decimal TargetVolatility { get; private set; }
        public InstrumentPositionSize InstrumentDetails { get; private set; }

        public decimal AnnualisedCashVolatility
        {
            get { return TradingCapital * TargetVolatility; }
        }

        public decimal DailyCashVolatilityTarget
        {
            get { return AnnualisedCashVolatility / SquareRootOfTime; }
        }

        public decimal VolatilityScalar
        {
            get { return DailyCashVolatilityTarget / InstrumentDetails.ValueVolatility; }
        }

        public decimal GetUnscaledPosition()
        {
            if (InstrumentDetails.ValueVolatility <= 0)
                return 0;
            decimal unscaledPosition = InstrumentForecast * VolatilityScalar / 10;
            return unscaledPosition;
        }
    }
}
namespace PikUpStix.Trading.Forecast
{
    public class ForecastElement
    {
        public ForecastElement(DateTime date, double price, double previousPrice, double previousFastEwma,
            double fastDecay,
            double slowDecay, double previousSlowEwma, double standardDeviationDecay, double previousVariance,
            double forecastScalar)
        {
            Date = date;
            Price = price;

            EwmaFast = Price;
            EwmaSlow = Price;
            if (previousPrice > 0)
            {
                Return = Convert.ToDouble(price - previousPrice);
                SquareRet = Math.Pow(Return, 2);
                EwmaFast = (fastDecay*Price) + (previousFastEwma*(1 - fastDecay));
                EwmaSlow = (slowDecay*Price) + (previousSlowEwma*(1 - slowDecay));
            }


            RawCrossover = EwmaFast - EwmaSlow;

            Variance = SquareRet;
            if (previousVariance > 0)
                Variance = (standardDeviationDecay*SquareRet) + ((1 - standardDeviationDecay)*previousVariance);
            StdDeviation = Math.Sqrt(Variance);
            VolatilityAdjustedCrossOver = RawCrossover/StdDeviation;
            Forecast = VolatilityAdjustedCrossOver*forecastScalar;
            UnCapped = Forecast;
            if (!double.IsNaN(Forecast) && !double.IsInfinity(Forecast))
                Forecast = Convert.ToDouble(new ForecastScaling().CapForecast(Convert.ToDecimal(Forecast)));
            else
            {
                Forecast = 0;
            }
        }

        public DateTime Date { get; private set; }
        public double Price { get; private set; }
        public double Return { get; private set; }
        public double SquareRet { get; private set; }
        public double EwmaFast { get; private set; }
        public double EwmaSlow { get; private set; }

        public double RawCrossover { get; private set; }
        public double Variance { get; private set; }
        public double StdDeviation { get; private set; }

        public double VolatilityAdjustedCrossOver { get; private set; }

        public double Forecast { get; set; }

        public double UnCapped { get; private set; }
    }
}
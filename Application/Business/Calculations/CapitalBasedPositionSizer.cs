namespace Application.Business.Calculations
{
    public class CapitalBasedPositionSizer
    {
        private readonly double forecast;
        private readonly double maximumRisk;
        private readonly double accountBalance;
        private readonly double lotSize;
        private readonly double pipSize;

        public CapitalBasedPositionSizer(double forecast, double maximumRisk, double accountBalance,
            double lotSize, double pipSize)
        {
            this.forecast = Math.Sqrt(forecast * forecast);
            this.maximumRisk = maximumRisk;
            this.accountBalance = accountBalance;
            this.lotSize = lotSize;
            this.pipSize = pipSize;
        }

        public double Calculate()
        {
            // Step 1: Determine risk amount
            double riskAmount = accountBalance * maximumRisk * forecast;

            // Step 2: Calculate pip value per standard lot
            double pipValue = lotSize * pipSize;

            // Step 3: Compute position size in standard lots
            double positionSize = riskAmount / pipValue;

            return positionSize; // Returns trade size in standard lots
        }
    }
}
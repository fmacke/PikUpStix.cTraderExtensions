namespace Application.Business.Calculations
{
    public class StopLossBasedPositionSizer
    {
        private readonly double forecast;
        private readonly double maximumRisk;
        private readonly double accountBalance;
        private readonly double lotSize;
        private readonly double pipSize;
        private readonly double stopLossPrice;
        private readonly double entryPrice;

        public StopLossBasedPositionSizer(double forecast, double maximumRisk, double accountBalance, double lotSize, double pipSize, double stopLossPrice, double entryPrice)
        {
            this.forecast = Math.Sqrt(forecast * forecast);
            this.maximumRisk = maximumRisk;
            this.accountBalance = accountBalance;
            this.lotSize = lotSize;
            this.pipSize = pipSize;
            this.stopLossPrice = stopLossPrice;
            this.entryPrice = entryPrice;
            if (stopLossPrice == 0)
                throw new ArgumentException("Stop loss price cannot be zero if using PositionSizer.");
        }


        public double Calculate()
        {
            // Step 1: Determine risk amount
            double riskAmount = maximumRisk * accountBalance * forecast;

            // Step 2: Calculate price difference (stop loss range)
            double priceDifference = Math.Round((entryPrice - stopLossPrice),4);

            // Step 3: Compute pip value (correctly scaled)
            double pipValue = (lotSize * pipSize);

            // Step 4: Calculate risk per standard lot
            double riskPerLot = (priceDifference / pipSize) * pipValue;

            // Step 5: Compute trade size in **standard lots**
            double positionSize = riskAmount / riskPerLot;

            return positionSize; // Correctly returns trade size in **standard lots**
        }
    }
}

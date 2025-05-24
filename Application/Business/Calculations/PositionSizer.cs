namespace Application.Business.Calculations
{
    public class PositionSizer
    {
        private readonly double forecast;
        private readonly double maximumRisk;
        private readonly double accountBalance;
        private readonly double lotSize;
        private readonly double pipSize;
        private readonly double stopLossPrice;
        private readonly double entryPrice;

        public PositionSizer(double forecast, double maximumRisk, double accountBalance, double lotSize, double pipSize, double stopLossPrice, double entryPrice)
        {
            this.forecast = Math.Sqrt(forecast*forecast);
            this.maximumRisk = maximumRisk;
            this.accountBalance = accountBalance;
            this.lotSize = lotSize;
            this.pipSize = pipSize;
            this.stopLossPrice = stopLossPrice;
            this.entryPrice = entryPrice;
            if(stopLossPrice == 0)
                throw new ArgumentException("Stop loss price cannot be zero if using PositionSizer.");
        }

        public double Calculate()
        {
            double riskAmount = accountBalance * maximumRisk;
            double priceDifference = Math.Sqrt(Math.Pow(Math.Round((entryPrice - stopLossPrice), 4),2));
            double unitsToTrade;

            // Use pip logic only if pipSize < 1 and lotSize > 1 (likely Forex)
            if (pipSize < 1 && lotSize > 1)
            {
                double pipDifference = priceDifference / pipSize;
                double riskPerUnit = pipDifference * pipSize;
                unitsToTrade = (riskAmount / riskPerUnit) * 1000;
            }
            else
            {
                // Direct price difference (stocks, gold, crypto)
                double riskPerUnit = priceDifference;
                unitsToTrade = riskAmount / riskPerUnit;
            }

            // Convert units to lots
            double lotsToTrade = (unitsToTrade / lotSize) * forecast;

            return lotsToTrade;
        }

    }
}

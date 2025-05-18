using System.Diagnostics;

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
            this.forecast = forecast;
            this.maximumRisk = maximumRisk;
            this.accountBalance = accountBalance;
            this.lotSize = lotSize;
            this.pipSize = pipSize;
            this.stopLossPrice = stopLossPrice;
            this.entryPrice = entryPrice;
        }

        public double Calculate()
        {
            double riskAmount = accountBalance * maximumRisk;
            double priceDifference = Math.Abs(entryPrice - stopLossPrice);

            double unitsToTrade;

            // Use pip logic only if pipSize < 1 and lotSize > 1 (likely Forex)
            if (pipSize < 1 && lotSize > 1)
            {
                double pipDifference = priceDifference / pipSize;
                double pipValuePerUnit = pipSize;
                double riskPerUnit = pipDifference * pipValuePerUnit;

                unitsToTrade = riskAmount / riskPerUnit;
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

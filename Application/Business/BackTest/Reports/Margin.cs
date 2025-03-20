using Domain.Entities;
using Domain.Enums;

namespace Application.Business.BackTest.Reports
{
    public static class Margin
    {
        public static double Calculate(double contractUnit, double exchangeRate, Domain.Entities.Position trade,
            double currentPriceData, double volume)
        {
            var priceMovement = 0.0;
            if (trade.PositionType == PositionType.BUY)
                priceMovement = currentPriceData - trade.EntryPrice;
            else
                priceMovement = trade.EntryPrice - currentPriceData;

            return contractUnit * exchangeRate * priceMovement * EnsurePositive(volume);
        }

        private static double EnsurePositive(double number)
        {
            var powers = Math.Pow(number, 2);
            var squred = Math.Sqrt(powers);
            return squred;
        }
    }
}
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.BackTest.Reports
{
    public static class Margin
    {
        public static decimal Calculate(decimal contractUnit, decimal exchangeRate, Test_Trades trade,
            decimal currentPriceData, decimal volume)
        {
            var priceMovement = new decimal();
            if (trade.Direction == PositionType.BUY.ToString())
                priceMovement = currentPriceData - trade.EntryPrice;
            else
                priceMovement = trade.EntryPrice - currentPriceData;

            return Convert.ToDecimal(contractUnit * exchangeRate * priceMovement * EnsurePositive(volume));
        }

        private static decimal EnsurePositive(decimal number)
        {
            var powers = Math.Pow(Convert.ToDouble(number), Convert.ToDouble(2));
            var squred = Math.Sqrt(powers);
            return Convert.ToDecimal(squred);
        }
    }
}
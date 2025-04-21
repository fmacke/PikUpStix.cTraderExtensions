using Domain.Enums;

namespace Application.Business.Calculations
{
    public class Margin : ICalculate
    {
        private double contractUnit;
        private double exchangeRate;
        private double volume;
        private Domain.Entities.Position trade;
        private double closePrice;
        public Margin(double contractUnit, double exchangeRate, Domain.Entities.Position trade, double closePrice, double volume)
        {
            this.contractUnit = contractUnit;
            this.exchangeRate = exchangeRate;
            this.trade = trade;
            this.closePrice = closePrice;
            this.volume = volume;
        }

        public double Calculate()
        {
            var priceMovement = 0.0;
            if (trade.PositionType == PositionType.BUY)
                priceMovement = closePrice - trade.EntryPrice;
            else
                priceMovement = trade.EntryPrice - closePrice;
            return contractUnit * exchangeRate * priceMovement * EnsurePositive(volume);
        }

        private static double EnsurePositive(double number)
        {
            return Math.Sqrt(Math.Pow(number, 2));
        }
    }
}
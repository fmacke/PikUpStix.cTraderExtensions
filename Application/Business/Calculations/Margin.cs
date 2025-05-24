using Domain.Enums;

namespace Application.Business.Calculations
{
    public class Margin : ICalculate
    {
        private double contractUnit;
        private double exchangeRate;
        private double volume;
        private double lotSize;
        private Domain.Entities.Position trade;
        private double closePrice;
        public Margin(double lotSize, double contractUnit, double exchangeRate, Domain.Entities.Position trade, double closePrice, double volume)
        {
            this.contractUnit = contractUnit;
            this.exchangeRate = exchangeRate;
            this.trade = trade;
            this.closePrice = closePrice;
            this.volume = volume;
            this.lotSize = lotSize;
        }

        public double Calculate()
        {
            var priceMovement = 0.0;
            if (trade.PositionType == PositionType.BUY)
                priceMovement = Math.Round(closePrice - trade.EntryPrice, 3);
            else
                priceMovement = Math.Round(trade.EntryPrice - closePrice, 3);
            return contractUnit * exchangeRate * priceMovement * EnsurePositive(volume) * lotSize;
           // return lotSize * contractUnit * exchangeRate * tradeVolume * closePrice;
        }

        private static double EnsurePositive(double number)
        {
            return Math.Sqrt(Math.Pow(number, 2));
        }
    }
}
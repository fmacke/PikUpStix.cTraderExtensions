using Domain.Enums;

namespace Application.Business.Calculations
{
    public class StopLossAtPrice : ICalculate
    {
        public double ExistingMargin { get; private set; }
        public double StopLossMaxPercent { get; private set; }
        public double ContractUnit { get; private set; }
        public double Volume { get; private set; }
        public double ExchangeRate { get; private set; }
        public double PurchasePrice { get; private set; }
        public double BiddingPrice { get; private set; }
        public double MinimumPriceFluctuation { get; set; }
        public PositionType PositionType { get; private set; }
        public StopLossAtPrice(double existingMargin, double stopLossMaxPercent, double contractUnit,
            double volume, double exchangeRate, PositionType posType, double askingPrice, double biddingPrice, double minimumPriceFluctuation)
        {
            if(stopLossMaxPercent > 0.5)
                throw new Exception("Stop loss is set at over 50% of account balance! Is that really correct?");
            ExistingMargin = existingMargin;
            StopLossMaxPercent = stopLossMaxPercent;
            ContractUnit = contractUnit;
            Volume = volume;
            ExchangeRate = exchangeRate;
            PurchasePrice = askingPrice;
            BiddingPrice = biddingPrice;
            PositionType = posType;
            MinimumPriceFluctuation = minimumPriceFluctuation;
        }
        
        public double Calculate()
        {
            double stopLossAmount = StopLossInPips();
            double stopLossPrice = stopLossPrice = PurchasePrice + stopLossAmount;

            if (PositionType == PositionType.BUY)
                stopLossPrice = PurchasePrice - stopLossAmount;

            // Adjust to the nearest minimum price fluctuation
            stopLossPrice = Math.Round(stopLossPrice / MinimumPriceFluctuation) * MinimumPriceFluctuation;

            return stopLossPrice;
        }

        public double StopLossInPips()
        {

            return ExistingMargin * StopLossMaxPercent / (ContractUnit * Volume * ExchangeRate);
        }

        public bool IsStopLossHitAt(double currentPrice)
        {
            if (PositionType == PositionType.BUY && currentPrice <= Calculate())
                return true;
            if (PositionType == PositionType.SELL && currentPrice >= Calculate())
                return true;
            return false;
        }
    }
}
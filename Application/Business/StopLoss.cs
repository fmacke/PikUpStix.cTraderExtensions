using Domain.Enums;

namespace Application.Business
{
    public class StopLoss
    {
        public StopLoss(double existingMargin, double stopLossMaxPercent, double contractUnit,
            double volume, double exchangeRate, PositionType posType, double askingPrice, double biddingPrice, double minimumPriceFluctuation)
        {
            ExistingMargin = existingMargin;
            StopLossMaxPercent = stopLossMaxPercent;
            ContractUnit = contractUnit;
            Volume = volume;
            ExchangeRate = exchangeRate;
            AskingPrice = askingPrice;
            BiddingPrice = biddingPrice;
            PositionType = posType;
            MinimumPriceFluctuation = minimumPriceFluctuation;
        }

        public double ExistingMargin { get; private set; }
        public double StopLossMaxPercent { get; private set; }
        public double ContractUnit { get; private set; }
        public double Volume { get; private set; }
        public double ExchangeRate { get; private set; }
        public double AskingPrice { get; private set; }
        public double BiddingPrice { get; private set; }
        public double MinimumPriceFluctuation { get; set; }
        public PositionType PositionType { get; private set; }

        public double StopLossInPips()
        {
            double stopLossValue = ExistingMargin * StopLossMaxPercent/100 * ExchangeRate;

            // Note pip size calculation should be same irrespective of trade direction
            var vol = Volume;
            if (PositionType == PositionType.SELL)
                vol = Volume * -1;
            double onePipValue =
                ContractUnit * ExchangeRate * vol * MinimumPriceFluctuation;
            if (onePipValue == 0)
                return 0;
            return stopLossValue / onePipValue;
        }

        public double StopLossInCurrency()
        {
            double stopLossInPips = StopLossInPips();
            double currentPrice = AskingPrice;
            if (PositionType == PositionType.SELL)
            {
                stopLossInPips = stopLossInPips * -1;
                currentPrice = BiddingPrice;
            }
            double stopInCurrency = CalculateStopAdjustmentByInstrument(stopLossInPips);
            var x = currentPrice - stopInCurrency;
            return x;
        }

        public bool StopLossHit(double currentPrice)
        {
            if (PositionType == PositionType.BUY && currentPrice <= StopLossInCurrency())
                return true;
            if (PositionType == PositionType.SELL && currentPrice >= StopLossInCurrency())
                return true;
            return false;
        }

        private double CalculateStopAdjustmentByInstrument(double stopLossInPips)
        {
            //for (int x = 0; x < unitOfChange; x++)
            //{
            //    stopAdjustment = stopAdjustment/10;
            //}
            //return stopAdjustment;
            return stopLossInPips * MinimumPriceFluctuation;
        }
    }
}
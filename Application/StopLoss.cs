using Domain.Enums;

namespace Application
{
    public class StopLoss
    {
        public StopLoss(decimal existingMargin, decimal stopLossMaxPercent, decimal contractUnit,
            decimal volume, decimal exchangeRate, PositionType posType, double askingPrice, double biddingPrice, decimal minimumPriceFluctuation)
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

        public decimal ExistingMargin { get; private set; }
        public decimal StopLossMaxPercent { get; private set; }
        public decimal ContractUnit { get; private set; }
        public decimal Volume { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public double AskingPrice { get; private set; }
        public double BiddingPrice { get; private set; }
        public decimal MinimumPriceFluctuation { get; set; }
        public PositionType PositionType { get; private set; }

        public decimal StopLossInPips()
        {
            decimal stopLossValue = ExistingMargin * StopLossMaxPercent * ExchangeRate;

            // Note pip size calculation should be same irrespective of trade direction
            var vol = Volume;
            if (PositionType == PositionType.SELL)
                vol = Volume * -1;
            decimal onePipValue =
                Convert.ToDecimal(ContractUnit * ExchangeRate * vol * MinimumPriceFluctuation);
            if (onePipValue == 0)
                return 0M;
            return stopLossValue / onePipValue;
        }

        public decimal StopLossInCurrency()
        {
            decimal stopLossInPips = StopLossInPips();
            double currentPrice = AskingPrice;
            if (PositionType == PositionType.SELL)
            {
                stopLossInPips = stopLossInPips * -1;
                currentPrice = BiddingPrice;
            }
            decimal stopInCurrency = CalculateStopAdjustmentByInstrument(stopLossInPips);
            var x = Convert.ToDecimal(currentPrice) - stopInCurrency;
            return x;
        }

        public bool StopLossHit(decimal currentPrice)
        {
            if (PositionType == PositionType.BUY && currentPrice <= StopLossInCurrency())
                return true;
            if (PositionType == PositionType.SELL && currentPrice >= StopLossInCurrency())
                return true;
            return false;
        }

        private decimal CalculateStopAdjustmentByInstrument(decimal stopLossInPips)
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
using Domain.Enums;

namespace Application.Business.Calculations
{
    public class TrailingStop
    {
        private PositionType tradeType;
        private double entryPrice;
        private double stopLossAt;
        private double biddingPrice;
        private double askingPrice;
        public double TrailingStopAt { get; set; }
        public bool TrailingStopUpdated { get; set; }
        public TrailingStop(PositionType tradeType, double entryPrice, double stopLossAt, double biddingPrice, double askingPrice, double executeTrailAtPips, double moveTrailByPips)
        {
            if (executeTrailAtPips < moveTrailByPips)
                throw new Exception("Trailing stop is too tight");
            TrailingStopUpdated = false;
            TrailingStopAt = stopLossAt;

            this.tradeType = tradeType;
            this.entryPrice = entryPrice;
            this.stopLossAt = stopLossAt;
            this.biddingPrice = biddingPrice;
            this.askingPrice = askingPrice;

            if (tradeType == PositionType.BUY)
            {
                
                if (stopLossAt < entryPrice)
                    if (askingPrice > entryPrice)
                        if (askingPrice - entryPrice > executeTrailAtPips)
                        {
                            TrailingStopAt = entryPrice + moveTrailByPips;
                            TrailingStopUpdated = true;
                        }
            }
            if (tradeType == PositionType.SELL)
            {

                if (stopLossAt > entryPrice)
                    if (biddingPrice < entryPrice)
                        if (entryPrice - biddingPrice > executeTrailAtPips)
                        {
                            TrailingStopAt = entryPrice - moveTrailByPips;
                            TrailingStopUpdated = true;
                        }
            }
        }
    }
}

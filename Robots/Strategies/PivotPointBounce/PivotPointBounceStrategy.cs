using Application.Business;
using Application.Business.Indicator;
using Domain.Entities;
using Domain.Enums;
using Robots.Common;
using Robots.Interfaces;

namespace Robots.Strategies.PivotPointBounce
{
    public class PivotPointStrategy : IStrategy
    {
        public PivotPoints PivotPoints { get; private set; }
        public AdxScores AdxScores { get; private set; }
        public bool MarketTrending { get; private set; }
        public double CurrentPrice { get; private set; }
        public DateTime CursorDate { get; private set; }
        public List<PositionUpdate> PositionInstructions { get; set; } = new List<PositionUpdate>();
        public List<HistoricalData> Bars { get; set; } = new List<HistoricalData>();

        public PivotPointStrategy(DateTime cursorDate, string symbolName, bool takeProfitAtPivot, PivotPoints pivotPoints, AdxScores adxValues, double bid, double ask, Positions positions,
            List<PendingOrderCommon> orders, List<HistoricalData> bars)
        {
            //CancelExpiredOrders(orders);
            if (orders.Count < 1)
            {
                CursorDate = cursorDate;
                CurrentPrice = (bid + ask) / 2;
                PivotPoints = pivotPoints;
                AdxScores = adxValues;
                Bars = bars;
                if (ConditionsForOrderMet(CurrentPrice) && positions.Count < 1)
                {
                    var position = CreateOrder(cursorDate, symbolName, takeProfitAtPivot);
                    PositionInstructions.Add(new PositionUpdate(position, InstructionType.PlaceOrder, 1));
                }
            }
        }

        private void CancelExpiredOrders(List<PendingOrderCommon> orders)
        {
            foreach (var order in orders)
                PositionInstructions.Add(new PositionUpdate(new Position(){Id = order.Id}, InstructionType.CancelOrder, 1));
        }
        
        private Position CreateOrder(DateTime cursorDate, string symbolName, bool takeProfitAtPivot)
        {
            var position = DetermineEntryPoint(takeProfitAtPivot);
            position.SymbolName = symbolName;
            position.Volume = 5000;
            position.CreatedAt = cursorDate;
            position.ExpirationDate = new DateTime(cursorDate.Year, cursorDate.Month, cursorDate.Day, 23, 0, 0);
            return position;
        }
        private Position DetermineEntryPoint(bool takeProfitAtPivot)
        {
            var position = new Position();
            if (LongConditionMet())
            {
                //Long Entry at Support1
                position.EntryPrice = PivotPoints.Support1;
                position.TradeType = TradeType.Buy;
                position.StopLoss = CalculatePips(PivotPoints.Support1 - PivotPoints.Support2);
                if (takeProfitAtPivot)
                    position.TakeProfit = CalculatePips(PivotPoints.Pivot - PivotPoints.Support1);//Take Profit at Pivot
                else
                    position.TakeProfit = CalculatePips(PivotPoints.Resistance1 - PivotPoints.Support1);//Take Profit at Resistance1
                return position;
            }
            if (ShortConditionMet())
            {
                //Short Entry at Resistance1
                position.EntryPrice = PivotPoints.Resistance1;
                position.TradeType = TradeType.Sell;
                position.StopLoss = CalculatePips(PivotPoints.Resistance1 - PivotPoints.Resistance2);
                if (takeProfitAtPivot)
                    position.TakeProfit = CalculatePips(PivotPoints.Resistance1 - PivotPoints.Pivot);//Take Profit at Pivot
                else
                    position.TakeProfit = CalculatePips(PivotPoints.Support1 - PivotPoints.Resistance1); ;//Take at Support1
                return position;
            }
            throw new Exception("An attempt to place order was placed despite the preliminary conditions not being met.");
        }

        private double? CalculatePips(double priceDiff)
        {
            var powered = Math.Pow(priceDiff, 2);
            return Math.Sqrt(powered)*10000;
        }

        private bool ConditionsForOrderMet(double currentPrice)
        {
            if (LongConditionMet())
                return true; 
            if (ShortConditionMet())
                return true;
            return false;// "No Entry";
        }
        private bool LongConditionMet()
        {
            var previousPreviousPrice = Bars[2].LowPrice;  
            var previousPrice = Bars[1].LowPrice; 
            var currentPrice = Bars[0].LowPrice; 
            //"Long Entry at Support1";
            if (previousPrice < PivotPoints.Support1 
                && currentPrice > PivotPoints.Support1
                && AdxScores.DIPlus > 20)
            {
                return true; // Long position should be placed }
            }
            return false;
        }
        private bool ShortConditionMet()
        {
            var previousPreviousPrice = Bars[2].HighPrice;
            var previousPrice = Bars[1].HighPrice;
            var currentPrice = Bars[0].HighPrice;
            //"Short Entry at Support1";
            if (previousPrice > PivotPoints.Resistance1
                && currentPrice < PivotPoints.Resistance1
                && AdxScores.DIMinus > 20)
            {
                return true; // Short position should be placed }
            }
            return false;
        }

        private bool BelowPivotPoint(double pivot)
        {
            return CurrentPrice <= pivot ? true : false;
        }

        private bool IsPivotPointBounce()
        {
            return Bars[1].LowPrice <= PivotPoints.Resistance1;
        }
        private bool IsCurrentPriceAbovePivot()
        {
            return Bars[0].ClosePrice >= PivotPoints.Resistance1;
        }

        //private bool ShortConditionMet()
        //{
        //    return false; // TESTING LONG ONLY RIGHT NOW
        //    // "Short Entry at Resistance1";
        //    return CurrentPrice >= PivotPoints.Resistance1;
        //}
        ////private bool LongConditionMet()
        //{
        //    //"Long Entry at Support1";
        //    return CurrentPrice <= PivotPoints.Support1;
        //}
        //private bool ShortConditionMet()
        //{
        //    // "Short Entry at Resistance1";
        //    return CurrentPrice >= PivotPoints.Resistance1;
        //}
    }
}

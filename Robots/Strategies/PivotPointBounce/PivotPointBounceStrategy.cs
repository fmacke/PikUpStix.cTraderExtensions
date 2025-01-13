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
        public bool MarketTrending { get; private set; }
        public double CurrentPrice { get; private set; }
        public List<PositionUpdate> PositionInstructions { get; set; } = new List<PositionUpdate>();
        public List<HistoricalData> Bars { get; set; } = new List<HistoricalData>();

        public PivotPointStrategy(string symbolName, bool takeProfitAtPivot, PivotPoints pivotPoints, double bid, double ask, Positions positions,
            List<PendingOrderCommon> orders, List<HistoricalData> bars)
        {
            CancelExpiredOrders(orders);
            CurrentPrice = (bid + ask) / 2;
            PivotPoints = pivotPoints;
            Bars = bars;
            if (ConditionsForOrderMet(CurrentPrice) && positions.Count < 1)
            {                
                var position = CreateOrder(symbolName, takeProfitAtPivot);
                PositionInstructions.Add(new PositionUpdate(position, InstructionType.PlaceOrder, 1));
            }
        }

        private void CancelExpiredOrders(List<PendingOrderCommon> orders)
        {
            foreach (var order in orders)
                PositionInstructions.Add(new PositionUpdate(new Position(){Id = order.Id}, InstructionType.CancelOrder, 1));
        }
        
        private Position CreateOrder(string symbolName, bool takeProfitAtPivot)
        {
            var position = DetermineEntryPoint(takeProfitAtPivot);
            position.SymbolName = symbolName;
            position.Volume = 5000;
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
            //"Long Entry at Support1";
            var noOfBarsToCheck = 5;
            if (Bars.Count < noOfBarsToCheck)
                return false;
            Bars = Bars.TakeLast(noOfBarsToCheck).ToList();
            if (BelowPivotPoint(PivotPoints.Pivot))
                if (IsPivotPointBounce(2))
                    if (IsAbovePivot(1)) { return true; }
            return false;
        }

        private bool BelowPivotPoint(double pivot)
        {
            return CurrentPrice <= pivot ? true : false;
        }

        private bool IsPivotPointBounce(int barBack)
        {
            return Bars[Bars.Count - barBack].LowPrice <= PivotPoints.Resistance1 && PivotPoints.Resistance1 <= Bars[Bars.Count - barBack].HighPrice;
        }
        private bool IsAbovePivot(int barBack)
        {
            return Bars[Bars.Count - barBack].ClosePrice >= PivotPoints.Resistance1;
        }

        private bool ShortConditionMet()
        {
            return false; // TESTING LONG ONLY RIGHT NOW
            // "Short Entry at Resistance1";
            return CurrentPrice >= PivotPoints.Resistance1;
        }
        //private bool LongConditionMet()
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

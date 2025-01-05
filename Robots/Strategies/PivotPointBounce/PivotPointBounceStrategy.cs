using Application.Business;
using Application.Business.Indicator;
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

        public PivotPointStrategy(string symbolName, double high, double low, double close, double bid, double ask, Positions positions,
            List<PendingOrderCommon> orders)
        {
            CancelExpiredOrders(orders);
            CurrentPrice = (bid + ask) / 2;
            PivotPoints = new PivotPoints(high, low, close);            
            if (ConditionsForOrderMet(CurrentPrice) && positions.Count < 1)
            {                
                var position = CreateOrder(symbolName);
                PositionInstructions.Add(new PositionUpdate(position, InstructionType.PlaceOrder, 1));
            }
        }

        private void CancelExpiredOrders(List<PendingOrderCommon> orders)
        {
            foreach (var order in orders)
                PositionInstructions.Add(new PositionUpdate(new Position(){Id = order.Id}, InstructionType.CancelOrder, 1));
        }
        
        private Position CreateOrder(string symbolName)
        {
            var position = DetermineEntryPoint();
            position.SymbolName = symbolName;
            position.Volume = 5000;
            return position;
        }
        private Position DetermineEntryPoint()
        {
            var position = new Position();
            if (LongConditionMet())
            {
                //Long Entry at Support1
                position.EntryPrice = PivotPoints.Support1;
                position.TradeType = TradeType.Buy;
                position.StopLoss = CalculatePips(PivotPoints.Support1 - PivotPoints.Support2);
                position.TakeProfit = CalculatePips(PivotPoints.Pivot - PivotPoints.Support1);//Take Profit at Pivot or Resistance1
                return position;
            }
            if (ShortConditionMet())
            {
                //Short Entry at Resistance1
                position.EntryPrice = PivotPoints.Resistance1;
                position.TradeType = TradeType.Sell;
                position.StopLoss = CalculatePips(PivotPoints.Resistance1 - PivotPoints.Resistance2);
                position.TakeProfit = CalculatePips(PivotPoints.Pivot - PivotPoints.Resistance1); ;//Take Profit at Pivot or Support1
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
            return CurrentPrice <= PivotPoints.Support1;
        }
        private bool ShortConditionMet()
        {
            // "Short Entry at Resistance1";
            return CurrentPrice >= PivotPoints.Resistance1;
        }
    }
}

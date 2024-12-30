using Application.Business;
using Domain.Enums;
using Robots.Common;
using Robots.Interfaces;

namespace Robots.Strategies.PivotPointBounce
{
    public class PivotPointStrategy : IStrategy
    {
        public double Pivot { get; private set; }
        public double Support1 { get; private set; }
        public double Resistance1 { get; private set; }
        public double Support2 { get; private set; }
        public double Resistance2 { get; private set; }
        public double CurrentPrice { get; private set; }
        public bool MarketTrending { get; private set; }
        public List<PositionUpdate> PositionInstructions { get; set; } = new List<PositionUpdate>();

        public PivotPointStrategy(double high, double low, double close, double bid, double ask, Positions positions)
        {
            CalculatePivotPoints(high, low, close, bid, ask);            
            if (ConditionsForOrderMet(CurrentPrice) && positions.Count < 1)
            {                
                var position = CreateOrder();                 
                PositionInstructions.Add(new PositionUpdate(position, position.StopLoss, position.TakeProfit, InstructionType.Open, 1, 1));
            }
        }

        private void CalculatePivotPoints( double high, double low, double close, double bid, double ask)
        {
            CurrentPrice = (bid + ask) / 2;
            Pivot = (high + low + close) / 3;
            Support1 = 2 * Pivot - high;
            Resistance1 = 2 * Pivot - low;
            Support2 = Pivot - (high - low);
            Resistance2 = Pivot + (high - low);
        }

        private Position CreateOrder()
        {
            var position = DetermineEntryPoint(CurrentPrice);
            return position;
        }

        private Position DetermineEntryPoint(double currentPrice)
        {
            var position = new Position();
            if (LongConditionMet())
            {
                //Long Entry at Support1
                position.CreatedAt = DateTime.UtcNow;
                position.EntryPrice = Support1;
                position.TradeType = TradeType.Buy;
                position.StopLoss = Support2;
                position.TakeProfit = Pivot;//Take Profit at Pivot or Resistance1
                return position;
            }
            if (ShortConditionMet())
            {
                //Short Entry at Resistance1
                position.CreatedAt = DateTime.UtcNow;
                position.EntryPrice = Resistance1;
                position.TradeType = TradeType.Sell;
                position.StopLoss = Resistance2;
                position.TakeProfit = Pivot;//Take Profit at Pivot or Support1
                return position;
            }
            throw new Exception("An attempt to place order was placed despite the preliminary conditions not being met.");
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
            return CurrentPrice <= Support1;
        }

        private bool ShortConditionMet()
        {
            // "Short Entry at Resistance1";
            return CurrentPrice >= Resistance1;
        }
    }
}

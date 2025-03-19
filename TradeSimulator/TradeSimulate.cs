using Application.Business;
using cAlgo.API;
using Domain.Entities;
using Domain.Enums;
using Robots.Interfaces;
using TradeSimulator.Business;

namespace TradeSimulator
{
    public static class PositionHandler
    {
        internal static void ClosePosition(ref List<TestTrade> openPositions, ref List<TestTrade> historicalTrades, TestTrade position)
        {
            // TODO: Calculate profit and loss and add this to the historical trades
            historicalTrades.Add(position);
            openPositions.Remove(position);
        }

        internal static void Modify(ref List<TestTrade> openPositions, Position position)
        {
            // todo: implement this
            var positionToModify = openPositions.FirstOrDefault(p => p.Id == position.Id);
            throw new NotImplementedException();
            var stop = instruction.Position.TradeType == Domain.Enums.TradeType.Buy ?
                            instruction.Position.EntryPrice - (instruction.Position.StopLoss * Symbol.PipSize) :
                            instruction.Position.EntryPrice + (instruction.Position.StopLoss * Symbol.PipSize);
            position.ModifyStopLossPrice(stop);
            if (!position.VolumeInUnits.Equals(Symbol.NormalizeVolumeInUnits(instruction.Position.Volume)))
                ModifyPosition(position, Symbol.NormalizeVolumeInUnits(instruction.Position.Volume));
        }

        internal static void Open(ref List<TestTrade> openPositions, Position position)
        {
            // todo: implement this
            throw new NotImplementedException();
            var normalise = Symbol.NormalizeVolumeInUnits(instruction.Position.Volume);
            ExecuteMarketOrder(tradeType, SymbolName, normalise, x.GetType().Name, instruction.Position.StopLoss,
                instruction.Position.TakeProfit);
        }

        internal static void PlaceOrder(ref List<TestTrade> openPositions, Application.Business.Position position)
        {
            //todo: implement this
            throw new NotImplementedException();
            var placeOrderRes = PlaceLimitOrder(tradeType,
                            instruction.Position.SymbolName,
                            Symbol.NormalizeVolumeInUnits(instruction.Position.Volume),
                            Convert.ToDouble(instruction.Position.EntryPrice),
                            GetType().Name,
                            Convert.ToDouble(instruction.Position.StopLoss),
                            Convert.ToDouble(instruction.Position.TakeProfit),
                            ProtectionType.Relative,
                            instruction.Position.ExpirationDate);
        }

        internal static void CancelOrder(ref List<TestTrade> openPositions, Application.Business.Position position)
        {
            // todo: implement this
            throw new NotImplementedException();
            foreach (var order in PendingOrders)
                if (instruction.Position.Id == order.Id)
                    CancelPendingOrder(order);
        }
    }
    public class TradeSimulate : TradeSimulateBase
    {
        List<TestTrade> OpenPositions  = new List<TestTrade>();
        List<TestTrade> ClosedTrades  = new List<TestTrade>();
        public IStrategy Strategy { get; private set; }

        public TradeSimulate(List<HistoricalData> bars) : base(bars)
        {
        }
        protected internal override void OnBar()
        {
            ManagePositions(Strategy);
        }

        protected internal override void OnStart()
        {
            Console.WriteLine("OnStart");
        }
        protected internal override void OnStop()
        {
            Console.WriteLine("OnStop");
        }
        public void ManagePositions(IStrategy x)
        {
            foreach (var instruction in x.PositionInstructions)
            {
                switch (instruction.InstructionType)
                {
                    case InstructionType.Close:
                        PositionHandler.ClosePosition(ref OpenPositions, ref ClosedTrades, instruction.Position);
                        break;
                    case InstructionType.Modify:
                        PositionHandler.Modify(ref OpenPositions, instruction.Position);
                        break;
                    case InstructionType.Open:
                        PositionHandler.Open(ref OpenPositions, instruction.Position);
                        break;
                    case InstructionType.PlaceOrder:
                        PositionHandler.PlaceOrder(ref OpenPositions, instruction.Position);
                        break;
                    case InstructionType.CancelOrder:
                        PositionHandler.CancelOrder(ref OpenPositions, instruction.Position);
                        break;
                }
            }
        }
    }
}

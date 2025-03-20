using Domain.Entities;
using Domain.Enums;
using Robots.Common;

namespace TradeSimulator
{
    public class PositionHandler
    {
        private readonly Dictionary<InstructionType, Action<PositionUpdate>> _instructionActions;
        private List<PositionUpdate> _positionInstructions;
        private List<Position> _openPositions;
        private List<Position> _closedPositions;

        private void HandleOpen(PositionUpdate update)
        {
            _openPositions.Add(update.Position);
        }
        private void HandleClose(PositionUpdate update)
        {
            // TODO: Calculate profit and loss and add this to the historical trades
            _closedPositions.Add(update.Position);
            _openPositions.RemoveAll(p => p.Id == update.Position.Id);
        }
        private void HandleModify(PositionUpdate update) => Console.WriteLine("Handling Modify instruction.");
        private void HandleLeave(PositionUpdate update) => Console.WriteLine("Handling Leave instruction.");
        private void HandlePlaceOrder(PositionUpdate update) => Console.WriteLine("Handling PlaceOrder instruction.");
        private void HandleCancelOrder(PositionUpdate update) => Console.WriteLine("Handling CancelOrder instruction.");

        public PositionHandler(List<PositionUpdate> positionInstructions, ref List<Position> openPositions, ref List<Position> closedTrades)
        {
            _instructionActions = new Dictionary<InstructionType, Action<PositionUpdate>>
            {
                { InstructionType.Open, HandleOpen },
                { InstructionType.Close, HandleClose },
                { InstructionType.Modify, HandleModify },
                { InstructionType.Leave, HandleLeave },
                { InstructionType.PlaceOrder, HandlePlaceOrder },
                { InstructionType.CancelOrder, HandleCancelOrder }
            };
            _positionInstructions = positionInstructions;
            _openPositions = openPositions;
            _closedPositions = closedTrades;
        }
        public void ExecuteInstructions()
        {
            foreach (var instruction in _positionInstructions)
            {
                if (_instructionActions.TryGetValue(instruction.InstructionType, out var action))
                {
                    action(instruction);
                }
                else
                {
                    Console.WriteLine("Unknown instruction.");
                }
            }

        }

        

        //internal static void Modify(ref List<Position> openPositions, Position position)
        //{
        //    // todo: implement this
        //    var positionToModify = openPositions.FirstOrDefault(p => p.Id == position.Id);
        //    throw new NotImplementedException();
        //    var stop = instruction.Position.TradeType == Domain.Enums.TradeType.Buy ?
        //                    instruction.Position.EntryPrice - (instruction.Position.StopLoss * Symbol.PipSize) :
        //                    instruction.Position.EntryPrice + (instruction.Position.StopLoss * Symbol.PipSize);
        //    position.ModifyStopLossPrice(stop);
        //    if (!position.VolumeInUnits.Equals(Symbol.NormalizeVolumeInUnits(instruction.Position.Volume)))
        //        ModifyPosition(position, Symbol.NormalizeVolumeInUnits(instruction.Position.Volume));
        //}

        //internal static void Open(ref List<Position> openPositions, Position position)
        //{
        //    // todo: implement this
        //    throw new NotImplementedException();
        //    var normalise = Symbol.NormalizeVolumeInUnits(instruction.Position.Volume);
        //    ExecuteMarketOrder(tradeType, SymbolName, normalise, x.GetType().Name, instruction.Position.StopLoss,
        //        instruction.Position.TakeProfit);
        //}

        //internal static void PlaceOrder(ref List<Position> openPositions, Application.Business.Position position)
        //{
        //    //todo: implement this
        //    throw new NotImplementedException();
        //    var placeOrderRes = PlaceLimitOrder(tradeType,
        //                    instruction.Position.SymbolName,
        //                    Symbol.NormalizeVolumeInUnits(instruction.Position.Volume),
        //                    Convert.ToDouble(instruction.Position.EntryPrice),
        //                    GetType().Name,
        //                    Convert.ToDouble(instruction.Position.StopLoss),
        //                    Convert.ToDouble(instruction.Position.TakeProfit),
        //                    ProtectionType.Relative,
        //                    instruction.Position.ExpirationDate);
        //}

        //internal static void CancelOrder(ref List<Position> openPositions, Application.Business.Position position)
        //{
        //    // todo: implement this
        //    throw new NotImplementedException();
        //    foreach (var order in PendingOrders)
        //        if (instruction.Position.Id == order.Id)
        //            CancelPendingOrder(order);
        //}


    }
}

using Domain.Entities;
using Domain.Enums;
using Robots.Common;

namespace TradeSimulator.Business
{
    public class PositionHandler
    {
        private readonly Dictionary<InstructionType, Action<PositionUpdate>> _instructionActions;
        private List<PositionUpdate> _positionInstructions;
        private List<Position> _openPositions;
        private List<Position> _closedPositions;
        public PositionHandler(List<PositionUpdate> positionInstructions, ref List<Position> openPositions, ref List<Position> closedTrades)
        {
            _instructionActions = new Dictionary<InstructionType, Action<PositionUpdate>>
            {
                { InstructionType.Open, HandleOpen },
                { InstructionType.Close, HandleClose },
                { InstructionType.Modify, HandleModify }
            };
            _positionInstructions = positionInstructions;
            _openPositions = openPositions;
            _closedPositions = closedTrades;
        }

        private void HandleOpen(PositionUpdate update)
        {
            new OpenPositionHandler(ref _openPositions).OpenPosition(update.Position);
        }
        private void HandleClose(PositionUpdate update)
        {
            new ClosePositionHandler(ref _openPositions, ref _closedPositions).ClosePosition(update.Position, update.CloseAt);
        }
        private void HandleModify(PositionUpdate update)
        {
            new ModifyPositionHandler(ref _openPositions).ModifyPosition(update.Position, update.AdjustStopLossTo, update.AdjustTakeProfitTo);
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
    }
}

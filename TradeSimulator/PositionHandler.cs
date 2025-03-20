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
            _closedPositions.Add(update.Position);
            _openPositions.Remove(update.Position);
        }
        private void HandleModify(PositionUpdate update)
        {
            if (!update.Modify)
                return;            
            var positionToModify = _openPositions.Find(p => p.Id == update.Position.Id);
            if (positionToModify != null)
            {
                if (positionToModify.TakeProfit.HasValue)
                    positionToModify.TakeProfit = update.AdjustTakeProfitTo;
                if (positionToModify.StopLoss.HasValue)
                    positionToModify.StopLoss = update.AdjustStopLossTo;
            }
        }

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

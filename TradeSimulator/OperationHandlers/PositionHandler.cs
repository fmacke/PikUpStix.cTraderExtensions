using Application.Business.Positioning;
using Domain.Entities;
using Domain.Enums;

namespace TradeSimulator.Business
{
    public class PositionHandler
    {
        private readonly Dictionary<InstructionType, Action<IPositionInstruction>> _instructionActions;
        private List<IPositionInstruction> _positionInstructions;
        private List<Position> _openPositions;
        private List<Position> _closedPositions;
        private double exchangeRate;
        private double contractUnit;

        public PositionHandler(List<IPositionInstruction> positionInstructions, ref List<Position> openPositions, ref List<Position> closedTrades, double exchangeRate, double contractUnit)
        {
            _instructionActions = new Dictionary<InstructionType, Action<IPositionInstruction>>
            {
                { InstructionType.Open, instruction => HandleOpen((OpenInstruction)instruction) },
                { InstructionType.Close, instruction => HandleClose((CloseInstruction)instruction) },
                { InstructionType.Modify, instruction => HandleModify((ModifyInstruction)instruction) }
            };
            _positionInstructions = positionInstructions;
            _openPositions = openPositions;
            _closedPositions = closedTrades;

            this.exchangeRate = exchangeRate;
            this.contractUnit = contractUnit;
        }   
        private void HandleOpen<T>(T update) where T : OpenInstruction
        {
            new OpenPositionHandler(ref _openPositions).OpenPosition(update);
        }

        private void HandleClose<T>(T update) where T : CloseInstruction
        {
            new ClosePositionHandler(ref _openPositions, ref _closedPositions).ClosePosition(update.Position, update.ClosePrice, update.ClosedAt, contractUnit, exchangeRate);
        }

        private void HandleModify<T>(T update) where T : ModifyInstruction
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

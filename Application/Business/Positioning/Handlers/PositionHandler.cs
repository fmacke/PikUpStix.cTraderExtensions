using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
    public class PositionHandler
    {
        private readonly Dictionary<InstructionType, Action<IPositionInstruction>> _instructionActions;
        private List<IPositionInstruction> _positionInstructions;
        private List<Position> _openPositions;
        private List<Position> _closedPositions;
        public List<IMarketInfo> MarketInfo { get; set; }

        public PositionHandler(List<IPositionInstruction> positionInstructions, ref List<Position> openPositions, ref List<Position> closedTrades, List<IMarketInfo> marketInfo)
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
            MarketInfo = marketInfo;
        }   
        private void HandleOpen<T>(T update) where T : OpenInstruction
        {
            update.Position.SymbolName = MarketInfo.Find(m => m.SymbolName == update.Position.SymbolName).SymbolName;
            new OpenPositionHandler(ref _openPositions).OpenPosition(update);
        }

        private void HandleClose<T>(T update) where T : CloseInstruction
        {
            var contractUnit = MarketInfo.Find(m => m.SymbolName == update.Position.SymbolName).ContractUnit;
            var exchangeRate = MarketInfo.Find(m => m.SymbolName == update.Position.SymbolName).ExchangeRate;
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

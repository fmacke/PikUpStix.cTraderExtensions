using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Application.Business.Positioning;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Robots.Strategies
{
    public class SimpleTestStrategy : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public List<Test_Parameter> TestParameters { get; set; } = new List<Test_Parameter>();
        private List<IPositionInstruction> _positionInstructions = new List<IPositionInstruction>();
        public IValidationService ValidationService { get; set; } = new ValidationService();

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            _positionInstructions.Clear();
            foreach (var marketInfo in marketInfos)
            {
                if (!marketInfo.Positions.Where(x => x.Status == PositionStatus.OPEN).Any())
                {
                    Position position = PositionCreator.CreatePosition(PositionType.BUY, 1, 0.02, 0.01, 0.02, marketInfo, null);
                    _positionInstructions.Add(new OpenInstruction(position, ValidationService));
                    LogMessages.Add($"Buy signal for {marketInfo.SymbolName} at price {marketInfo.Ask}");
                }
            }
            return _positionInstructions;
        }
    }
}
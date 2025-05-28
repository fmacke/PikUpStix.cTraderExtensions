using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Application.Business.Calculations;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Robots.Strategies
{
    public class KISS : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public List<Test_Parameter> TestParameters { get; set; } = new List<Test_Parameter>();
        private List<IPositionInstruction> _positionInstructions = new List<IPositionInstruction>();
        public IValidationService ValidationService { get; set;  } = new ValidationService();

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            _positionInstructions.Clear();
            foreach (var marketInfo in marketInfos)
            {
                if (!marketInfo.Positions.Where(p => p.Status == PositionStatus.OPEN).Any())
                {
                    var stopLoss = marketInfo.Ask - 0.01; // Example stop loss calculation
                    var takeProfit = marketInfo.Ask + 0.01; // Example stop loss calculation                    
                    var positionSizeb = new StopLossBasedPositionSizer(1, 0.02, marketInfo.CurrentCapital, marketInfo.LotSize, marketInfo.PipSize, stopLoss, marketInfo.Ask).Calculate();
                    var position = new Position()
                    {
                        SymbolName = marketInfo.SymbolName,
                        PositionType = PositionType.BUY,
                        EntryPrice = marketInfo.Ask,
                        StopLoss = stopLoss,
                        TakeProfit = takeProfit,
                        Volume = positionSizeb * marketInfo.LotSize,
                        Created = marketInfo.CursorDate,
                        //ExpirationDate = new DateTime(marketInfo.CursorDate.Year, marketInfo.CursorDate.Month, marketInfo.CursorDate.Day, 23, 59, 0)
                    };
                    _positionInstructions.Add(new OpenInstruction(position, ValidationService));
                }
            }
            return _positionInstructions;
        }
    }
}

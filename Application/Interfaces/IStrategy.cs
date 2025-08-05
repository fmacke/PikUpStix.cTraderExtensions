using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStrategy
    {
        List<string> LogMessages { get; set; }
        List<Test_Parameter> TestParameters { get; set; }
        List<IPositionInstruction> PositionInstructions { get; set; }
        List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos);        
        IValidationService GetValidationService();
        void LoadDefaultParameters(Dictionary<string, string> parameters);
    }
}

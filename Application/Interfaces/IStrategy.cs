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
        List<IPositionInstruction> Run(List<IMarketInfo> marketInfos);
        public IValidationService ValidationService { get; }
    }
}

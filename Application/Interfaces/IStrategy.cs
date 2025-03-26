using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStrategy
    {
        List<string> LogMessages { get; set; }
        List<Test_Parameter> TestParameters { get; }
        List<IPositionInstruction> Run(List<IMarketInfo> marketInfos);
    }
}

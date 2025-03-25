using Application.Business.Market;
using Application.Business.Positioning;
using Domain.Entities;

namespace Application.Business.Strategy
{
    public interface IStrategy
    {
        List<string> LogMessages { get; set; }
        List<Test_Parameter> TestParameters { get; }
        List<IPositionInstruction> Run(List<IMarketInfo> marketInfos);
    }
}

using Domain.Entities;

namespace TradeSimulator.StrategySetup
{
    internal interface IGetStrategyParameters
    {
        List<Test_Parameter> Run();
    }
}

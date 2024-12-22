using DataServices.Calls;

namespace DataServices
{
    public interface IDataService
    {
        InstrumentCalls Instruments { get; }
        TestParameterCalls TestParameters { get; }
        TestCalls Tests { get; }
        TestTradeCalls TestTrades { get; }
    }
}
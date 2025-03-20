using DataServices.Calls;

namespace DataServices
{
    public interface IDataService
    {
        InstrumentCalls InstrumentCaller { get; }
        TestParameterCalls TestParameterCaller { get; }
        TestCalls TestCaller { get; }
        PositionCalls PositionCaller { get; }
        HistoricalDataCalls HistoricalDataCaller { get; }
    }
}
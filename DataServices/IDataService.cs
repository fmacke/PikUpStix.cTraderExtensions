using DataServices.Calls;

namespace DataServices
{
    public interface IDataService
    {
        Instruments Instruments { get; }
        TestParameters TestParameters { get; }
        Tests Tests { get; }
    }
}
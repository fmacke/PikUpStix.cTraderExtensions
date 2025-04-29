using Application.Business.Indicator.Signal;
using Application.Business.Market;
using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using DataServices;
using Domain.Entities;
using Domain.Enums;
using Robots.Strategies;
using TradeSimulator.Simulate;

internal class Program
{
    public static IStrategy Strategy { get; set; } = new VolumePriceAnalysis();  // set IStrategy here
    public static int InstrumentId { get; set; } = 3; // the instrument id to be used for testing
    public static IMarketInfo TestInfo { get; set; }
    public static bool SaveTestResult { get; set; } = true;

    private static void Main(string[] args)
    {
        GetMarketData(InstrumentId);
        Strategy.TestParameters = new List<Test_Parameter>()
        {
            new Test_Parameter() { Name = "ForecastThreshold[Double]", Value = "1" }
        };
        var tradeSimulator = new TradeSimulate(TestInfo, Strategy, 10000, SaveTestResult);
        tradeSimulator.Run();
    }

    private static void GetMarketData(int instrumentId)
    {
        DataService dataServices = new DataService();
        var instrument = dataServices.InstrumentCaller.GetInstrument(instrumentId);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<HistoricalDataProfile>());
        var mapper = config.CreateMapper();
        var marketData = mapper.Map<List<HistoricalData>>(instrument.HistoricalDatas.ToList());
        TestInfo = new MarketInfo(
            new DateTime(),
            0,
            0,
            new List<Position>(),
            marketData,
            instrument.InstrumentName,
            instrument.Currency,
            10000,
            instrument.ContractUnit,
            100000,
            1,
            new ConfirmingSignals(new List<ISignal>()),
            ConvertToTimeFrame(instrument.Frequency)
        );
    }

    private static TimeFrame ConvertToTimeFrame(string frequency)
    {
        TimeFrame timeFrame = new TimeFrame();
        if (Enum.TryParse(frequency, true, out timeFrame))
        {
            return timeFrame;
        }
        throw new Exception("Frequency does not match with any known TimeFrame");
    }

    private static List<Test_Parameter> GetPivotPointParams()
    {
        return new List<Test_Parameter>()
        {
            new Test_Parameter() { Name = "ForecastThreshold[Double]", Value = "1" },
            new Test_Parameter() { Name = "ConfirmingForecastThreshold[Double]", Value = "0.5" },
            new Test_Parameter() { Name = "TakeProfitAtPivot[Boolean]", Value = "true" },
            new Test_Parameter() { Name = "RiskPerTrade[Double]", Value = "2" },
            new Test_Parameter() { Name = "EnableMA[Boolean]", Value = "true" },
            new Test_Parameter() { Name = "EnableRSI[Boolean]", Value = "true" },
            new Test_Parameter() { Name = "UseAdx[Boolean]", Value = "true" },
            new Test_Parameter() { Name = "AdxLowThreshold[Int]", Value = "20" },
        };
    }
}
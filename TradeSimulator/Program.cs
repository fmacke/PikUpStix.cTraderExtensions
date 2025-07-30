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
    public static IStrategy Strategy { get; set; } = new SimpleTestStrategy();  // set IStrategy here
    public static int BarInstrumentId { get; set; } = 3; // the onBar instrument id to be used for testing
    public static int TickInstrumentId { get; set; } = 4; // the onTick instrument id to be used for testing
    public static IMarketInfo TestInfo { get; set; }
    public static bool SaveTestResult { get; set; } = true;
    private static DateTime startTestingFrom = new DateTime(2024, 01, 01);
    private static DateTime endTestingAt = new DateTime(2025, 01, 01);

    private static void Main(string[] args)
    {
        GetMarketData(BarInstrumentId, TickInstrumentId);
        int[] periods = { 100 };
        double[] volumeWeights = { 0.8 };
        double[] riskPerTrade = { 0.1 };
        double[] stopLossAmount = { 1 };//0.0001, 0.0005, 0.0008, 0.001, 0.01 };
        
        foreach (var period in periods)
        {
            foreach (var volumeWeight in volumeWeights)
            {
                foreach (var risk in riskPerTrade)
                {
                    foreach (var stopLoss in stopLossAmount)
                    {
                        Strategy.TestParameters = new List<Test_Parameter>();
                        Dictionary<string, string> parameters = new Dictionary<string, string>
                        {
                            { "Periods[Int]", period.ToString() },
                            { "VolumeWeight[Double]", volumeWeight.ToString() },
                            { "RiskPerTrade[Double]", risk.ToString() },
                            { "StopLossAmount[Double]", stopLoss.ToString() }
                        };
                        Strategy.LoadDefaultParameters(parameters);
                        var tradeSimulator = new TradeSimulate(TestInfo, Strategy, 10000, SaveTestResult);
                        tradeSimulator.Run();
                    }
                }
            }
        }
    }

    

    private static void GetMarketData(int barInstrumentId, int tickInstrumentid)
    {
        DataService dataServices = new DataService();
        Application.Features.Instruments.Queries.GetById.GetInstrumentByIdResponse barInstrument;
        Application.Features.Instruments.Queries.GetById.GetInstrumentByIdResponse tickInstrument;
        List<HistoricalData> barData  = GetHistoricalData(barInstrumentId, dataServices, out barInstrument);
        List<HistoricalData> tickData = GetHistoricalData(tickInstrumentid, dataServices, out tickInstrument);
        TestInfo = new MarketInfo(
            new DateTime(),
            0,
            0,
            new List<Position>(),
            barData,
            tickData,
            barInstrument.InstrumentName,
            barInstrument.Currency,
            10000,
            barInstrument.MinimumPriceFluctuation, //pip size
            barInstrument.ContractUnit,// lot size
            1,
            new ConfirmingSignals(new List<ISignal>()),
            ConvertToTimeFrame(barInstrument.Frequency),
            ConvertToTimeFrame(tickInstrument.Frequency)
        );
        TestInfo.InstrumentId = barInstrumentId;
    }

    private static List<HistoricalData> GetHistoricalData(int barInstrumentId, DataService dataServices, out Application.Features.Instruments.Queries.GetById.GetInstrumentByIdResponse instrument)
    {
        instrument = dataServices.InstrumentCaller.GetInstrument(barInstrumentId);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<HistoricalDataProfile>());
        var mapper = config.CreateMapper();
        var marketData = mapper.Map<List<HistoricalData>>(
            instrument.HistoricalDatas.Where(x => x.Date > startTestingFrom && x.Date < endTestingAt).ToList());
        return marketData;
    }

    private static TimeFrame ConvertToTimeFrame(string frequency)
    {
        if (TimeFrameParser.TryParse(frequency, out TimeFrame result))
            return result;
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
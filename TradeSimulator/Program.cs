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
    public static IStrategy Strategy { get; set; } = new KISS();  // set IStrategy here
    public static int InstrumentId { get; set; } = 3; // the tick instrument id to be used for testing
    public static IMarketInfo TestInfo { get; set; }
    public static bool SaveTestResult { get; set; } = true;

    private static void Main(string[] args)
    {
        GetMarketData(InstrumentId);
        int[] periods = { 25 };
        double[] volumeWeights = { 0.5 };// { 0.2, 0.4, 0.5, 0.6, 0.8 };
        double[] riskPerTrade = { 0.02 };
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
                        GetVolumePriceAnalysisParameters(period, volumeWeight, risk, stopLoss);
                        var tradeSimulator = new TradeSimulate(TestInfo, Strategy, 10000, SaveTestResult);
                        tradeSimulator.Run();
                    }
                }
            }
        }
    }

    private static void GetVolumePriceAnalysisParameters(int period, double volumeWeight, double riskPerTrade, double stopLossAmount)
    {
        Strategy.TestParameters.Add(new Test_Parameter() { Name = "RiskPerTrade[Double]", Value = riskPerTrade.ToString() });
        Strategy.TestParameters.Add(new Test_Parameter() { Name = "Periods[Int]", Value = period.ToString() });
        Strategy.TestParameters.Add(new Test_Parameter() { Name = "PriceWeight[Double]", Value = (1 - volumeWeight).ToString() });
        Strategy.TestParameters.Add(new Test_Parameter() { Name = "VolumeWeight[Double]", Value = volumeWeight.ToString() });
        Strategy.TestParameters.Add(new Test_Parameter() { Name = "StopLossAmount[Double]", Value = stopLossAmount.ToString() });

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
            instrument.MinimumPriceFluctuation, //pip size
            instrument.ContractUnit,// lot size
            1,
            new ConfirmingSignals(new List<ISignal>()),
            ConvertToTimeFrame(instrument.Frequency)
        );
        TestInfo.InstrumentId = instrumentId;
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
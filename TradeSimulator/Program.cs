using Application.Business.Indicator.Signal;
using Application.Business.Market;
using Application.Business.Simulate;
using Application.Interfaces;
using Application.Mappings;
using AutoMapper;
using DataServices;
using Domain.Entities;
using Robots.Strategies;

internal class Program
{
    //public static List<HistoricalData> BarData { get; set; }
    public static IStrategy Strategy { get; set; }
    public static IMarketInfo MarketInfo { get; set; }
    private static void Main(string[] args)
    {
        Strategy = new KISS();
        GetMarketData(6);
        //var pivotPointData = GetMarketData(5); // the timeframe of the instrument to be used for pivot point calculation
        //marketData.AddRange(pivotPointData);
        //var strategy = new PivotPointConfirmStrategy(GetPivotPointParams());
        var tradeSimulator = new TradeSimulate(MarketInfo, Strategy, 10000);
        tradeSimulator.Run();
    }

    private static void GetMarketData(int instrumentId)
    {
        DataService dataServices = new DataService();
        var instrument = dataServices.InstrumentCaller.GetInstrument(instrumentId);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<HistoricalDataProfile>());
        var mapper = config.CreateMapper();
        var marketData = mapper.Map<List<HistoricalData>>(instrument.HistoricalDatas.ToList());
        MarketInfo = new MarketInfo(
            new DateTime(),
            0,
            0,
            new List<Position>(),
            marketData,
            instrument.InstrumentName,
            instrument.Currency,
            10000,
            instrument.ContractUnit,
            1,
            new ConfirmingSignals(new List<ISignal>()),
            Domain.Enums.TimeFrame.H1
        );
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
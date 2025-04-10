using Application.Business.Simulate;
using Application.Mappings;
using AutoMapper;
using DataServices;
using Domain.Entities;
using Robots.Strategies.PivotPointBounce;

internal class Program
{
    private static void Main(string[] args)
    {
        var marketData = GetMarketData(1);
        var pivotPointData = GetMarketData(2); // the timeframe of the instrument to be used for pivot point calculation
        var strategy = new PivotPointConfirmStrategy(GetPivotPointParams());
        var tradeSimulator = new TradeSimulate(marketData, strategy, 10000);
        tradeSimulator.Run();
    }

    private static List<HistoricalData> GetMarketData(int instrumentId)
    {
        DataService dataServices = new DataService();
        var instrument = dataServices.InstrumentCaller.GetInstrument(instrumentId);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<HistoricalDataProfile>());
        var mapper = config.CreateMapper();
        var marketData = mapper.Map<List<HistoricalData>>(instrument.HistoricalDatas.ToList());
        return marketData;
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
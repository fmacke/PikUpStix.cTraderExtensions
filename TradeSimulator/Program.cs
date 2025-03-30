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

        DataService dataServices = new DataService();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<HistoricalDataProfile>());
        var mapper = config.CreateMapper();
        var Bars = dataServices.HistoricalDataCaller.GetAllHistoricalDataCachedAsync(); 
        var historicalTrades = mapper.Map<List<HistoricalData>>(Bars);
        var strategy = new PivotPointConfirmStrategy(GetPivotPointParams());
        var tradeSimulator = new TradeSimulate(historicalTrades, strategy, 10000);
        tradeSimulator.Run();
    }

    private static List<Test_Parameter> GetPivotPointParams()
    {
        [Parameter("ForecastThreshold", DefaultValue = 1, Step = 0.1, MaxValue = 1, MinValue = 0, Group = "Strategy")]
        public double ForecastThreshold { get; set; }
        [Parameter("Confirming Forecast Threshold", DefaultValue = 0.5, Step = 0.1, MaxValue = 1, MinValue = 0, Group = "Strategy")]
        public double ConfirmingForecastThreshold { get; set; }
        [Parameter("Take Profit at Pivot", DefaultValue = "true", Group = "Strategy")]
        public bool TakeProfitAtPivot { get; set; }
        [Parameter("RiskPerTrade", DefaultValue = 2, Step = 0.5, MaxValue = 20, MinValue = 0.5, Group = "Strategy")]
        public double RiskPerTrade { get; set; }
        [Parameter("Enable MA", DefaultValue = "true", Group = "Confirming Signals")]
        public bool EnableMA { get; set; }
        [Parameter("Enable RSI", DefaultValue = "true", Group = "Confirming Signals")]
        public bool EnableRSI { get; set; }
        [Parameter("UseAdx", DefaultValue = "true", Group = "Confirming Signals")]
        public bool EnableAdx { get; set; }
        [Parameter("Adx Low Threshold", DefaultValue = 20, Step = 2, MaxValue = 24, MinValue = 0, Group = "Confirming Signals")]
        public int AdxLowThreshold { get; set; }
        [Parameter("Adx High Threshold", DefaultValue = 25, Step = 2, MaxValue = 40, MinValue = 24, Group = "Confirming Signals")]
        public int AdxHighThreshold { get; set; }

    }
}
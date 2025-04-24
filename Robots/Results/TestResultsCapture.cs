using Application.Features.TestParameters.Commands.Create;
using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Update;
using DataServices;
using AutoMapper;
using Domain.Entities;
using Application.Features.Positions.Commands.Create;
using Application.Mappings;
using Application.Business.Reports;

namespace Robots.Results
{
    public class TestResultsCapture
    {
        public int TestId { get; private set; }
        public List<Test_Parameter> TestParams { get; set; } = new List<Test_Parameter>();
        public double StartingCapital { get; private set; }

        public TestResultsCapture(string description, double accountBalance, Dictionary<string, string> robotProperties, IDataService dataService)
        {
            StartTest(description, accountBalance, dataService);
            foreach (var prop in robotProperties)
            {
                TestParams.Add(new Test_Parameter
                {
                    Name = prop.Key,
                    Value = prop.Value,
                    TestId = TestId
                });
                dataService.TestParameterCaller.AddTestParameters(new CreateTestParameterCommand()
                {
                    Name = prop.Key,
                    Value = prop.Value,
                    TestId = TestId
                });
            }
        }
        public TestResultsCapture(string description, double accountBalance, List<Test_Parameter> parameters, IDataService dataService)
        {
            StartTest(description, accountBalance, dataService);
            foreach (var param in parameters)
            {
                dataService.TestParameterCaller.AddTestParameters(new CreateTestParameterCommand()
                {
                    Name = param.Name,
                    Value = param.Value,
                    TestId = TestId
                });
            }
        }
        public void StartTest(string description, double accountBalance, IDataService dataService)
        {
            StartingCapital = accountBalance;
            TestId = dataService.TestCaller.AddTest(new CreateTestCommand()
            {
                FromDate = new DateTime(1900, 1, 1),
                ToDate = new DateTime(1900, 1, 1),
                StartingCapital = accountBalance,
                EndingCapital = 0,
                Description = description,
                TestEndAt = DateTime.Now,
                TestRunAt = DateTime.Now
            });
        }
        public string Capture(string method, CreatePositionRangeCommand tts, IDataService dataService, double maximumAdverseExcursion)
        {
            try
            {   
                var config = new MapperConfiguration(cfg => cfg.AddProfile<PositionsProfile>());
                var mapper = config.CreateMapper();
                var historicalTrades = mapper.Map<List<Position>>(tts);
                var tradeStatistics = new TradeStatistics(historicalTrades, StartingCapital, maximumAdverseExcursion);

                dataService.PositionCaller.AddPositionRange(tts);
                var test = dataService.TestCaller.GetTest(TestId);
                var endingCaptial = tts.Count == 0 ? 0 : tts.Sum(x => x.Margin) + test.StartingCapital;
                var fromDate = tts.Count == 0 ? new DateTime(1900, 1, 1) : tts.Min(x => x.Created).AddDays(-1);
                var toDate = tts.Count == 0 ? new DateTime(1900, 1, 1) : Convert.ToDateTime(tts.Max(x => x.ClosedAt)).AddDays(1);

                dataService.TestCaller.UpdateTest(new UpdateTestCommand()
                {
                    Id = TestId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    EndingCapital = endingCaptial,
                    TestEndAt = DateTime.Now,
                    MaxAdverseExcursion = tradeStatistics.MaxAdverseExcursion,
                    SharpeRatio = tradeStatistics.SharpeRatio,
                    NetProfit = tradeStatistics.NetProfit,
                    Commission = tradeStatistics.Commission,
                    MaxEquityDrawdown = tradeStatistics.MaxEquityDrawdown,
                    MaxBalanceDrawdown = tradeStatistics.MaxBalanceDrawdown,
                    TotalTrades = tradeStatistics.TotalTrades,
                    WinningTrades = tradeStatistics.WinningTrades,
                    MaxConsecutiveLosingTrades = tradeStatistics.MaxConsecutiveLosingTrades,
                    LargestLosingTrades = tradeStatistics.LargestLosingTrades,
                    LosingTrades = tradeStatistics.LosingTrades,
                    MaxConsecutiveWinningTrades = tradeStatistics.MaxConsecutiveWinningTrades,
                    LargestWinningTrade = tradeStatistics.LargestWinningTrade,
                    AverageTrade = tradeStatistics.AverageTrade,
                    SortinoRatio = tradeStatistics.SortinoRatio,
                    GrossProfit = tradeStatistics.GrossProfit,
                    GrossLoss = tradeStatistics.GrossLoss,
                    NetShortProfit = tradeStatistics.NetShortProfit,
                    NetLongProfit = tradeStatistics.NetLongProfit,
                    GrossShortProfit = tradeStatistics.GrossShortProfit,
                    GrossLongProfit = tradeStatistics.GrossLongProfit,
                    ProfitFactor = tradeStatistics.ProfitFactor,
                    ProfitFactorLongTrades = tradeStatistics.ProfitFactorLongTrades,
                    ProfitFactorShortTrades = tradeStatistics.ProfitFactorShortTrades,
                    NetShortLoss = tradeStatistics.NetShortLoss,
                    NetLongLoss = tradeStatistics.NetLongLoss,
                    GrossShortLoss = tradeStatistics.GrossShortLoss,
                    GrossLongLoss = tradeStatistics.GrossLongLoss,
                    ProfitableTradesRatio = tradeStatistics.ProfitableTradesRatio,
                    LosingTradesRatio = tradeStatistics.LosingTradesRatio,
                    ProfitableLongTradesRatio = tradeStatistics.ProfitableLongTradesRatio,
                    ProfitableShortTradesRatio = tradeStatistics.ProfitableShortTradesRatio,
                    AverageWin = tradeStatistics.AverageWin,
                    AverageWinLong = tradeStatistics.AverageWinLong,
                    AverageWinShort = tradeStatistics.AverageWinShort,
                    AverageLoss = tradeStatistics.AverageLoss,
                    AverageLossLong = tradeStatistics.AverageLossLong,
                    AverageLossShort = tradeStatistics.AverageLossShort
                });
            }
            catch (Exception ex)
            {
                
                return ex.Message.ToString();
            }
            return "pass captured";
        }

        
    }
}

using Application.Business.BackTest.Reports;
using Application.Features.TestParameters.Commands.Create;
using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Update;
using cAlgo.API;
using DataServices;
using AutoMapper;
using Domain.Entities;
using Application.Features.Positions.Commands.Create;
using Domain.Enums;
using Application.Mappings;

namespace FXProBridge.Capture
{
    public class TestResultsCapture
    {
        public int TestId { get; private set; }
        public List<Test_Parameter> TestParams { get; set; } = new List<Test_Parameter>();
        public double StartingCapital { get; private set; }

        public TestResultsCapture(string description, double accountBalance, Dictionary<string, string> robotProperties, IDataService dataService)
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
        public string Capture(string method, List<HistoricalTrade> trades, IDataService dataService, double maximumAdverseExcursion)
        {
            try
            {                
                var tts = new CreatePositionRangeCommand();
                foreach (var tr in trades)
                {
                    tts.Add(new CreatePositionCommand
                    {
                        TestId = TestId,
                        Comment = tr.ClosingDealId.ToString() + " || " + tr.Label,
                        Created = tr.EntryTime,
                        Volume = tr.VolumeInUnits,
                        PositionType = GetPositionType(tr.TradeType),
                        EntryPrice = tr.EntryPrice,
                        Commission = tr.Commissions,                        
                        ClosedAt = tr.ClosingTime,
                        ClosePrice = tr.ClosingPrice,
                        InstrumentId = 1,//db.Instruments.First(x => x.InstrumentName.Equals(tr.SymbolName) && x.DataSource == "FXPRO").Id,
                        Status = PositionStatus.CLOSED,
                        Margin = tr.NetProfit
                    });
                }
                var config = new MapperConfiguration(cfg => cfg.AddProfile<PositionsProfile>());
                var mapper = config.CreateMapper();
                var historicalTrades = mapper.Map<List<Domain.Entities.Position>>(tts);
                var tradeStatistics = new TradeStatistics(historicalTrades, StartingCapital, maximumAdverseExcursion);

                dataService.PositionCaller.AddPositionRange(tts);
                var test = dataService.TestCaller.GetTest(TestId);
                dataService.TestCaller.UpdateTest(new UpdateTestCommand()
                {
                    Id = TestId,
                    FromDate = tts.Min(x => x.Created).AddDays(-1),
                    ToDate = Convert.ToDateTime(tts.Max(x => x.ClosedAt)).AddDays(1),
                    EndingCapital = tts.Sum(x => x.Margin) + test.StartingCapital,
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

        private PositionType GetPositionType(TradeType tradeType)
        {
            if(tradeType.GetType().Name == "BUY")
            {
                return PositionType.BUY;
            }
            else
            {
                return PositionType.SELL;
            }
        }
    }
}

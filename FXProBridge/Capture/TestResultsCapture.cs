using Application.Features.TestParameters.Commands.Create;
using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Update;
using Application.Features.TestTrades.Commands.Create;
//using cAlgo.API;
using DataServices;
using Domain.Entities;

namespace FXProBridge.Capture
{
    public class TestResultsCapture
    {
        public int TestId { get; private set; }
        public List<Test_Parameter> TestParams { get; set; } = new List<Test_Parameter>();

        public TestResultsCapture(string description, decimal accountBalance, Dictionary<string, string> robotProperties, IDataService dataService)
        {
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
        public string Capture(string method, List<HistoricalTrade> trades, IDataService dataService)
        {
            try
            {
                var historicalTrades = trades;
                DateTime from = new DateTime(2005, 1, 1);
                //var instruments = db.Instruments;
                var tts = new CreateTestTradeRangeCommand();
                foreach (var tr in trades)
                {
                    tts.Add(new CreateTestTradeCommand
                    {
                        TestId = TestId,
                        Comment = tr.ClosingDealId.ToString() + " || " + tr.Label,
                        CapitalAtClose = Convert.ToDecimal(tr.Balance),
                        Created = tr.EntryTime,
                        Volume = Convert.ToDecimal(tr.VolumeInUnits),
                        Direction = tr.TradeType.ToString().ToUpper(),
                        EntryPrice = Convert.ToDecimal(tr.EntryPrice),
                        Commission = Convert.ToDecimal(tr.Commissions),
                        // STOPLOSS - not possible to get this from HistoricalTrade item.  I presume this is because SL it can change over the lifetime of a position
                        ClosedAt = tr.ClosingTime,
                        ClosePrice = Convert.ToDecimal(tr.ClosingPrice),
                        InstrumentId = 1,//db.Instruments.First(x => x.InstrumentName.Equals(tr.SymbolName) && x.DataSource == "FXPRO").Id,
                        InstrumentWeight = "NONE",
                        Status = "HISTORICALTRADE",
                        Margin = Convert.ToDecimal(tr.NetProfit)
                    });
                }

                dataService.TestTradeCaller.AddTestTradeRange(tts);
                var test = dataService.TestCaller.GetTest(TestId);
                dataService.TestCaller.UpdateTest(new UpdateTestCommand()
                {
                    Id = TestId,
                    FromDate = tts.Min(x => x.Created).AddDays(-1),
                    ToDate = Convert.ToDateTime(tts.Max(x => x.ClosedAt)).AddDays(1),
                    EndingCapital = tts.Sum(x => x.Margin) + test.StartingCapital
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

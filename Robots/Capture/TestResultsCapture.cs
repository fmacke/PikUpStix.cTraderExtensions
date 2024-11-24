using cAlgo.API;
using Domain.Entities;
using Infrastructure.Contexts;
using System.Data.SqlClient;


namespace Robots.Capture
{
    public class TestResultsCapture
    {
        public int TestId { get; private set; }
        public List<Test_Parameters> TestParams { get; }

        private ApplicationDbContext db;
        public TestResultsCapture(string description, object robot)
        {
            string connString = @"Server=localhost;Database=TradingBE;User Id=sa;Password=Gogogo123!;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open(); // Your code here }
            }
            db = new ApplicationDbContext();
            var rob = (Robot)robot;
            //DbConfiguration.SetConfiguration(new CustomDbConfig());
            //string e = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;  // For getting new app.config path when fxPro updates and requires new connectionstring entry
            var testResult = new Test()
            {
                FromDate = new DateTime(1900, 1, 1),
                ToDate = new DateTime(1900, 1, 1),
                StartingCapital = Convert.ToDecimal(rob.Account.Balance),
                EndingCapital = 0,
                Description = description,
                TestEndAt = DateTime.Now,
                TestRunAt = DateTime.Now
            };

            db.Tests.Add(testResult);
            db.SaveChanges();
            TestId = testResult.Id;

            TestParams = RobotProperties.GetRobotProperties(robot, TestId);

            db.Test_Parameters.AddRange(TestParams);
            db.SaveChanges();
        }
        public string Capture(string method, List<HistoricalTrade> trades)
        {
            try
            {
                var historicalTrades = trades;

                DateTime from = new DateTime(2005, 1, 1);

                var instruments = db.Instruments;

                var tts = new List<Test_Trades>();
                foreach (var tr in trades)
                {
                    tts.Add(new Test_Trades
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
                        InstrumentId = db.Instruments.First(x => x.InstrumentName.Equals(tr.SymbolName) && x.DataSource == "FXPRO").Id,
                        InstrumentWeight = "NONE",
                        Status = "HISTORICALTRADE",
                        Margin = Convert.ToDecimal(tr.NetProfit)
                    });
                }
                db.Test_Trades.AddRange(tts);
                db.SaveChanges();

                var testResult = db.Tests.First(x => x.Id == TestId);
                testResult.FromDate = tts.Min(x => x.Created).AddDays(-1);
                testResult.ToDate = Convert.ToDateTime(tts.Max(x => x.ClosedAt)).AddDays(1);
                testResult.EndingCapital = tts.Sum(x => x.Margin) + testResult.StartingCapital;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return "pass captured";
        }

    }
}

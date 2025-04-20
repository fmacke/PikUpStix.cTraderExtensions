using Application.Business.Calculations;
using Application.Features.Positions.Commands.Create;
using cAlgo.API;
using DataServices;
using Domain.Enums;
using Domain.Entities;
using Robots.Results;

namespace FXProBridge.Capture
{
    /// <summary>
    /// Robot Wrapper that deals with recording test results.
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class RobotTestWrapper : BarDataCapture
    {
        public bool IsTestRun { get; set; } = true;
        public TestResultsCapture? ResultsCapture { get; set; }
        public DataService DataService { get; set; }
        public Dictionary<string, string> TestParams { get; set; } = new Dictionary<string, string>();
        public  double _maximumAdverseExcursion { get; set; } = 0.0;

        public RobotTestWrapper()
        {
            DataService = new DataService();
        }
        protected override void OnStart()
        {
            if (IsTestRun)
                LogTestStart(this);
            //base.OnStart();
        }
        protected override void OnTick()
        {
            _maximumAdverseExcursion = MaxExcursion.Get(new List<Domain.Entities.Position>(), 0);
        }
        protected override void OnStop()
        {
            if (IsTestRun)
            {
                //db.Instruments.First(x => x.InstrumentName.Equals(tr.SymbolName) && x.DataSource == "FXPRO").Id,
                LogTestEnd(History, 1);//REMOVE HARD CODING
            }
            base.OnStop();
        }
        private void LogTestStart(Robot robot)
        {
            var startBalance = robot.Account.Balance;
            if (IsTestRun)
                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), startBalance, TestParams, DataService);
        }
        private string LogTestEnd(History history, int instrumentId)
        {            
            if (IsTestRun && ResultsCapture != null)
            {
                var tts = new CreatePositionRangeCommand();
                foreach (var tr in history.ToList())
                {
                    tts.Add(new CreatePositionCommand
                    {
                        TestId = ResultsCapture.TestId,
                        Comment = tr.ClosingDealId.ToString() + " || " + tr.Label,
                        Created = tr.EntryTime,
                        Volume = tr.VolumeInUnits,
                        PositionType = GetPositionType(tr.TradeType),
                        EntryPrice = tr.EntryPrice,
                        Commission = tr.Commissions,
                        ClosedAt = tr.ClosingTime,
                        ClosePrice = tr.ClosingPrice,
                        InstrumentId = 1,
                        Status = PositionStatus.CLOSED,
                        Margin = tr.NetProfit
                    });
                }
                return ResultsCapture.Capture("onStop", tts, DataService, _maximumAdverseExcursion);
            }
            return "Not a test run.";
        }
        
        private PositionType GetPositionType(TradeType tradeType)
        {
            if (tradeType.GetType().Name == "BUY")
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
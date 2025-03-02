using cAlgo.API;
using DataServices;

namespace FXProBridge.Capture
{
    /// <summary>
    /// Robot Wrapper that deals with recording test results.
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class RobotTestWrapper : Robot
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
            CalculateMaxExcursion();
        }
        protected override void OnStop()
        {
            if (IsTestRun)
            {
                LogTestEnd(History);
            }
        }
        private void LogTestStart(Robot robot)
        {
            var startBalance = robot.Account.Balance;
            if (IsTestRun)
                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), startBalance, TestParams, DataService);
        }
        private string LogTestEnd(History history)
        {            
            if (IsTestRun && ResultsCapture != null)
            {
                return ResultsCapture.Capture("onStop", history.ToList(), DataService, _maximumAdverseExcursion);
            }
            return "Not a test run.";
        }
        private void CalculateMaxExcursion()
        {
            foreach (var position in Positions)
            {
                // Calculate the adverse excursion for each open position
                double adverseExcursion = (position.EntryPrice - Bars.LowPrices.LastValue) / position.EntryPrice * 100;
                if (adverseExcursion > _maximumAdverseExcursion)
                {
                    _maximumAdverseExcursion = adverseExcursion;
                }
            }
        }
    }
}
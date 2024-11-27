using cAlgo.API;

namespace Robots.Capture
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class RobotTestWrapper : Robot
    {
        public bool IsTestRun { get; set; } = true;

        public TestResultsCapture? ResultsCapture { get; private set; } = null;

        protected override void OnStart()
        {
            if(IsTestRun)
                LogTestStart(this);
        }
        protected void LogTestStart(Robot robot)
        {            
            var startBalance = Convert.ToDecimal(robot.Account.Balance);
            if (IsTestRun)
                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), startBalance );
        }
        public string LogTestEnd(History history)
        {
            if (IsTestRun)
                return ResultsCapture.Capture("onStop", history.ToList());
            return "Not a test run.";
        }
    }
}


















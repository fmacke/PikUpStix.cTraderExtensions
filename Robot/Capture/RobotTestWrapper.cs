//using cAlgo.API;

//namespace Robots.Capture
//{
//    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
//    public class RobotTestWrapper : Robot
//    {
//        public bool IsTestRun { get; set; } = true;

//        public TestResultsCapture ResultsCapture { get; private set; }
//        protected void LogTestStart(object robot)
//        {
//            if (IsTestRun)
//                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), robot);
//        }
//        public string LogTestEnd(History history)
//        {
//            if (IsTestRun)
//                return ResultsCapture.Capture("onStop", history.ToList());
//            return "Not a test run.";
//        }
//    }
//}


















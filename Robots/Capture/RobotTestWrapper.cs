using cAlgo.API;
using DataServices;
using System.Reflection;

namespace Robots.Capture
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class RobotTestWrapper : Robot
    {
        public bool IsTestRun { get; set; } = true;
        public TestResultsCapture? ResultsCapture { get; set; }
        public DataService DataService { get; set; }
        public Dictionary<string, string> TestParams { get; set; }

        public RobotTestWrapper()
        {
            DataService = new DataService();
        }

        //public Dictionary<string, string> GetProperties()
        //{
        //    var propertyNames = new Dictionary<string, string>();
        //    PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo property in properties)
        //    {
        //        propertyNames.Add(property.Name, property.Name);
        //    }
        //    return propertyNames;
        //}

        protected override void OnStart()
        {
            if(IsTestRun)
                LogTestStart(this);
            //base.OnStart();
        }
        protected void LogTestStart(Robot robot)
        {            
            var startBalance = Convert.ToDecimal(robot.Account.Balance);
            if (IsTestRun)
                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), startBalance, TestParams, DataService);
        }
        public string LogTestEnd(History history)
        {
            if (IsTestRun && ResultsCapture != null)
                return ResultsCapture.Capture("onStop", history.ToList());
            return "Not a test run.";
        }
    }
}


















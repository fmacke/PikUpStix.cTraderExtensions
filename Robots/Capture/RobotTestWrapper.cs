using cAlgo.API;
using System.Reflection;

namespace Robots.Capture
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class RobotTestWrapper : Robot
    {
        public bool IsTestRun { get; set; } = true;
        public TestResultsCapture? ResultsCapture { get; private set; } = null;

        public Dictionary<string, string> GetProperties()
        {
            var propertyNames = new Dictionary<string, string>();
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                propertyNames.Add(property.Name, property.Name);
            }
            return propertyNames;
        }

        protected override void OnStart()
        {
            if(IsTestRun)
                LogTestStart(this);
        }
        protected void LogTestStart(Robot robot)
        {            
            var startBalance = Convert.ToDecimal(robot.Account.Balance);
            if (IsTestRun)
                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), startBalance, GetProperties());
        }
        public string LogTestEnd(History history)
        {
            if (IsTestRun)
                return ResultsCapture.Capture("onStop", history.ToList());
            return "Not a test run.";
        }
    }
}


















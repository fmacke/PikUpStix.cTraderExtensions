using System.Reflection;

namespace Robots.Capture
{
    public static class RobotProperties
    {
        public static Dictionary<string, string> GetRobotProperties(object robot)
        {
            var testParams = new Dictionary<string, string>();
            var subclassType = robot.GetType();
            foreach (PropertyInfo info in subclassType.GetProperties()
                .Where(x => x.DeclaringType == subclassType))
            {
                testParams.Add(info.Name + "[" + info.PropertyType.Name.ToString() + "]",
                    info.GetValue(robot).ToString());
            }
            return testParams;
        }
    }
}

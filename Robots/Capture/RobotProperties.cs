using Domain.Entities;
using System.Reflection;

namespace Robots.Capture
{
    public static class RobotProperties
    {
        public static Dictionary<string, string> GetRobotProperties(object robot)
        {
            var testParams = new Dictionary<string, string>();
            
            foreach (PropertyInfo info in robot.GetType().GetProperties()
                .Where(x => x.DeclaringType == robot.GetType()))
            {
                testParams.Add(info.Name + "[" + info.PropertyType.Name.ToString() + "]",
                    info.GetValue(robot).ToString());
            }
            return testParams;
        }
    }
}

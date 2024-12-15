using Domain.Entities;
using Robots.CarverTrendFollower;
using System.CodeDom;
using System.Reflection;

namespace Robots.Capture
{
    public static class RobotProperties
    {
        public static Dictionary<string, string> GetRobotProperties(object robot)//, Type type)
        {
            var testParams = new Dictionary<string, string>();

            // Get the type of the subclass
            var subclassType = typeof(CarverTrendFollowercTrader);

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

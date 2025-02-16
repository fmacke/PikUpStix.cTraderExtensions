using System.Reflection;

namespace Application.Business.Extensions
{
    public static class ParametersToDictionary
    {
        public static Dictionary<string, string> GetRobotProperties(object robot)
        {
            var testParams = new Dictionary<string, string>();
            var subclassType = robot.GetType().BaseType;
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

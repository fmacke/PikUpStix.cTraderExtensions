using Domain.Entities;
using System.Reflection;

namespace Robots.Capture
{
    public static class RobotProperties
    {
        public static List<Test_Parameters> GetRobotProperties(object robot, int testId)
        {
            var testParams = new List<Test_Parameters>();
            testParams.Add(new Test_Parameters()
            {
                TestId = testId,
                Name = "Robot [Class]",
                Value = robot.GetType().ToString()
            });
            foreach (PropertyInfo info in robot.GetType().GetProperties()
                .Where(x => x.DeclaringType == robot.GetType()))
            {
                testParams.Add(new Test_Parameters()
                {
                    TestId = testId,
                    Name = info.Name + "[" + info.PropertyType.Name.ToString() + "]",
                    Value = info.GetValue(robot).ToString()
                });
            }
            return testParams;
        }
    }
}

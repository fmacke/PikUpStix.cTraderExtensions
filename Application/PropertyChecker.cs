using Domain.Entities;

namespace Application
{
    public static class PropertyChecker
    {
        public static bool CheckExists(string parameterName, List<Test_Parameters> testParameters)
        {
            foreach (var testParam in testParameters)
                if (testParam.Name.Equals(parameterName))
                {
                    return true;
                }
            throw new Exception("The parameter, '" + parameterName + "' is required but missing from the inputs.");
        }
    }
}

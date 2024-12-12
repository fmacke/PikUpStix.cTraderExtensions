namespace Infrastructure.CacheKeys
{
    public static class Test_ParameterCacheKeys
    {
        public static string ListKey => "Test_ParameterList";

        public static string SelectListKey => "Test_ParameterSelectList";

        public static string GetKey(int testParameterId) => $"Test_Parameter-{testParameterId}";

        public static string GetDetailsKey(int testParameterId) => $"Test_ParameterDetails-{testParameterId}";
    }
}
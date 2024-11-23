namespace Infrastructure.CacheKeys
{
    public static class TestCacheKeys
    {
        public static string ListKey => "TestList";

        public static string SelectListKey => "TestSelectList";

        public static string GetKey(int testId) => $"Test-{testId}";

        public static string GetDetailsKey(int testId) => $"TestDetails-{testId}";
    }
}
namespace Infrastructure.CacheKeys
{
    public static class TestTradeCacheKeys
    {
        public static string ListKey => "TestTradeList";

        public static string SelectListKey => "TestTradeSelectList";

        public static string GetKey(int testTradeId) => $"TestTrade-{testTradeId}";

        public static string GetDetailsKey(int testTradeId) => $"TestTradeDetails-{testTradeId}";
    }
}
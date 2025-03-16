using Domain.Entities;

namespace Infrastructure.CacheKeys
{
    public static class HistoricalDataCacheKeys
    {
        public static string ListKey => "HistoricalDataList";

        public static string SelectListKey => "HistoricalDataSelectList";

        public static string GetKey(int historicalDataId) => $"HistoricalData-{historicalDataId}";

        public static string GetDetailsKey(int historicalDataId) => $"HistoricalDataDetails-{historicalDataId}";
    }
}
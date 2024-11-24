namespace Infrastructure.CacheKeys
{
    public static class InstrumentCacheKeys
    {
        public static string ListKey => "InstrumentList";

        public static string SelectListKey => "InstrumentSelectList";

        public static string GetKey(int instrumentId) => $"Instrument-{instrumentId}";

        public static string GetDetailsKey(int instrumentId) => $"InstrumentDetails-{instrumentId}";
    }
}
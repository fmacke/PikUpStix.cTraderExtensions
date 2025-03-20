namespace Infrastructure.CacheKeys
{
    public static class PositionCacheKeys
    {
        public static string ListKey => "PositionList";

        public static string SelectListKey => "PositionSelectList";

        public static string GetKey(int positionId) => $"Position-{positionId}";

        public static string GetDetailsKey(int positionId) => $"PositionDetails-{positionId}";
    }
}
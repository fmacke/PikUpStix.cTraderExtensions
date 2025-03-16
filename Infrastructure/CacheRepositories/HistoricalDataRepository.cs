using Microsoft.Extensions.Caching.Distributed;
using Application.Common.ThrowR;
using Infrastructure.CacheKeys;
using Application.Interfaces.Repositories;
using Application.Interfaces.CacheRepositories;
using Domain.Entities;

namespace Infrastructure.CacheRepositories
{
    public class HistoricalDataCacheRepository : IHistoricalDataCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHistoricalDataRepository _historicalDataRepository;

        public HistoricalDataCacheRepository(IDistributedCache distributedCache, IHistoricalDataRepository historicalDataRepository)
        {
            _distributedCache = distributedCache;
            _historicalDataRepository = historicalDataRepository;
        }

        public async Task<HistoricalData> GetByIdAsync(int historicalDataId)
        {
            string cacheKey = HistoricalDataCacheKeys.GetKey(historicalDataId);
            var historicalDataBytes = await _distributedCache.GetAsync(cacheKey);
            HistoricalData historicalData = null;
            if (historicalDataBytes != null)
            {
                historicalData = System.Text.Json.JsonSerializer.Deserialize<HistoricalData>(historicalDataBytes);
            }
            if (historicalData == null)
            {
                historicalData = await _historicalDataRepository.GetByIdAsync(historicalDataId);
                Throw.Exception.IfNull(historicalData, "HistoricalData", "No HistoricalData Found");
                var historicalDataBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(historicalData);
                await _distributedCache.SetAsync(cacheKey, historicalDataBytesToCache, new DistributedCacheEntryOptions());
            }
            return historicalData;
        }

        public async Task<List<HistoricalData>> GetCachedListAsync()
        {
            string cacheKey = HistoricalDataCacheKeys.ListKey;
            var historicalDataListBytes = await _distributedCache.GetAsync(cacheKey);
            List<HistoricalData> historicalDataList = null;
            if (historicalDataListBytes != null)
            {
                historicalDataList = System.Text.Json.JsonSerializer.Deserialize<List<HistoricalData>>(historicalDataListBytes);
            }
            if (historicalDataList == null)
            {
                historicalDataList = await _historicalDataRepository.GetListAsync();
                var historicalDataListBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(historicalDataList);
                await _distributedCache.SetAsync(cacheKey, historicalDataListBytesToCache, new DistributedCacheEntryOptions());
            }
            return historicalDataList;
        }
    }
}
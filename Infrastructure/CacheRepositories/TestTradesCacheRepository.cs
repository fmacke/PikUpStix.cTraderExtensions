using Microsoft.Extensions.Caching.Distributed;
using Application.Common.ThrowR;
using Infrastructure.CacheKeys;
using Application.Interfaces.Repositories;
using Application.Interfaces.CacheRepositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.CacheRepositories
{
    public class TestTradeCacheRepository : ITestTradeCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ITestTradeRepository _testTradeRepository;

        public TestTradeCacheRepository(IDistributedCache distributedCache, TestTradeRepository testTradeRepository)
        {
            _distributedCache = distributedCache;
            _testTradeRepository = testTradeRepository;
        }

        public async Task<TestTrade> GetByIdAsync(int testTradeId)
        {
            string cacheKey = TestTradeCacheKeys.GetKey(testTradeId);
            var testTradeBytes = await _distributedCache.GetAsync(cacheKey);
            TestTrade testTrade = null;
            if (testTradeBytes != null)
            {
                testTrade = System.Text.Json.JsonSerializer.Deserialize<TestTrade>(testTradeBytes);
            }
            if (testTrade == null)
            {
                testTrade = await _testTradeRepository.GetByIdAsync(testTradeId);
                Throw.Exception.IfNull(testTrade, "Test", "No Test Found");
                var testTradeBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(testTrade);
                await _distributedCache.SetAsync(cacheKey, testTradeBytesToCache, new DistributedCacheEntryOptions());
            }
            return testTrade;
        }

        public async Task<List<TestTrade>> GetCachedListAsync()
        {
            string cacheKey = TestTradeCacheKeys.ListKey;
            var testTradeListBytes = await _distributedCache.GetAsync(cacheKey);
            List<TestTrade> testTradeList = null;
            if (testTradeListBytes != null)
            {
                testTradeList = System.Text.Json.JsonSerializer.Deserialize<List<TestTrade>>(testTradeListBytes);
            }
            if (testTradeList == null)
            {
                testTradeList = await _testTradeRepository.GetListAsync();
                var testTradeListBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(testTradeList);
                await _distributedCache.SetAsync(cacheKey, testTradeListBytesToCache, new DistributedCacheEntryOptions());
            }
            return testTradeList;
        }
    }
}
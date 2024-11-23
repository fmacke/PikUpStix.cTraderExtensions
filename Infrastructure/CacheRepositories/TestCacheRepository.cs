using Microsoft.Extensions.Caching.Distributed;
using Application.Common.ThrowR;
using Infrastructure.CacheKeys;
using Application.Interfaces.Repositories;
using Application.Interfaces.CacheRepositories;
using Domain.Entities;

namespace Infrastructure.CacheRepositories
{
    public class TestCacheRepository : ITestCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ITestRepository _cVRRepository;

        public TestCacheRepository(IDistributedCache distributedCache, ITestRepository cVRRepository)
        {
            _distributedCache = distributedCache;
            _cVRRepository = cVRRepository;
        }

        public async Task<Test> GetByIdAsync(int cVRId)
        {
            string cacheKey = TestCacheKeys.GetKey(cVRId);
            var cVRBytes = await _distributedCache.GetAsync(cacheKey);
            Test cVR = null;
            if (cVRBytes != null)
            {
                cVR = System.Text.Json.JsonSerializer.Deserialize<Test>(cVRBytes);
            }
            if (cVR == null)
            {
                cVR = await _cVRRepository.GetByIdAsync(cVRId);
                Throw.Exception.IfNull(cVR, "Test", "No Test Found");
                var cVRBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(cVR);
                await _distributedCache.SetAsync(cacheKey, cVRBytesToCache, new DistributedCacheEntryOptions());
            }
            return cVR;
        }

        public async Task<List<Test>> GetCachedListAsync()
        {
            string cacheKey = TestCacheKeys.ListKey;
            var cVRListBytes = await _distributedCache.GetAsync(cacheKey);
            List<Test> cVRList = null;
            if (cVRListBytes != null)
            {
                cVRList = System.Text.Json.JsonSerializer.Deserialize<List<Test>>(cVRListBytes);
            }
            if (cVRList == null)
            {
                cVRList = await _cVRRepository.GetListAsync();
                var cVRListBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(cVRList);
                await _distributedCache.SetAsync(cacheKey, cVRListBytesToCache, new DistributedCacheEntryOptions());
            }
            return cVRList;
        }
    }
}
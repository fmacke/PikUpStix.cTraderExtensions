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
        private readonly ITestRepository _testRepository;

        public TestCacheRepository(IDistributedCache distributedCache, ITestRepository testRepository)
        {
            _distributedCache = distributedCache;
            _testRepository = testRepository;
        }

        public async Task<Test> GetByIdAsync(int testId)
        {
            string cacheKey = TestCacheKeys.GetKey(testId);
            var testBytes = await _distributedCache.GetAsync(cacheKey);
            Test test = null;
            if (testBytes != null)
            {
                test = System.Text.Json.JsonSerializer.Deserialize<Test>(testBytes);
            }
            if (test == null)
            {
                test = await _testRepository.GetByIdAsync(testId);
                Throw.Exception.IfNull(test, "Test", "No Test Found");
                var testBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(test);
                await _distributedCache.SetAsync(cacheKey, testBytesToCache, new DistributedCacheEntryOptions());
            }
            return test;
        }

        public async Task<List<Test>> GetCachedListAsync()
        {
            string cacheKey = TestCacheKeys.ListKey;
            var testListBytes = await _distributedCache.GetAsync(cacheKey);
            List<Test> testList = null;
            if (testListBytes != null)
            {
                testList = System.Text.Json.JsonSerializer.Deserialize<List<Test>>(testListBytes);
            }
            if (testList == null)
            {
                testList = await _testRepository.GetListAsync();
                var testListBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(testList);
                await _distributedCache.SetAsync(cacheKey, testListBytesToCache, new DistributedCacheEntryOptions());
            }
            return testList;
        }
    }
}
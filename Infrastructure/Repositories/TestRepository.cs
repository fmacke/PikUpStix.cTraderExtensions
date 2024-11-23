using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly IRepositoryAsync<Test> _repository;
        private readonly IDistributedCache _distributedCache;

        public TestRepository(IDistributedCache distributedCache, IRepositoryAsync<Test> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Test> Tests => _repository.Entities;

        public async Task DeleteAsync(Test test)
        {
            await _repository.DeleteAsync(test);
            await _distributedCache.RemoveAsync(CacheKeys.TestCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.TestCacheKeys.GetKey(test.Id));
        }

        public async Task<Test> GetByIdAsync(int testId)
        {
            return await _repository.Entities
                .Where(p => p.Id == testId ).FirstOrDefaultAsync();
        }

        public async Task<List<Test>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Test test)
        {
            await _repository.AddAsync(test);
            await _distributedCache.RemoveAsync(CacheKeys.TestCacheKeys.ListKey);
            return test.Id;
        }

        public async Task UpdateAsync(Test test)
        {
            await _repository.UpdateAsync(test);
            await _distributedCache.RemoveAsync(CacheKeys.TestCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.TestCacheKeys.GetKey(test.Id));
        }
    }
}
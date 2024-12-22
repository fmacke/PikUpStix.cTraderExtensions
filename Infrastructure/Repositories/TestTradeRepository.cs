using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class TestTradeRepository : ITestTradeRepository
    {
        private readonly IRepositoryAsync<TestTrade> _repository;
        private readonly IDistributedCache _distributedCache;

        public TestTradeRepository(IDistributedCache distributedCache, IRepositoryAsync<TestTrade> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<TestTrade> TestTrades => _repository.Entities;

        public async Task DeleteAsync(TestTrade TestTrade)
        {
            await _repository.DeleteAsync(TestTrade);
            await _distributedCache.RemoveAsync(CacheKeys.TestTradeCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.TestTradeCacheKeys.GetKey(TestTrade.Id));
        }

        public async Task<TestTrade> GetByIdAsync(int id)
        {
            return await _repository.Entities
                .Where(p => p.Id == id ).FirstOrDefaultAsync();
        }

        public async Task<List<TestTrade>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(TestTrade testTrade)
        {
            await _repository.AddAsync(testTrade);
            await _distributedCache.RemoveAsync(CacheKeys.TestTradeCacheKeys.ListKey);
            return testTrade.Id;
        }

        public async Task<int> InsertRangeAsync(List<TestTrade> testTrades)
        {
            await _repository.AddRangeAsync(testTrades); 
            await _distributedCache.RemoveAsync(CacheKeys.TestTradeCacheKeys.ListKey);
            return 1;
        }

        public async Task UpdateAsync(TestTrade TestTrade)
        {
            await _repository.UpdateAsync(TestTrade);
            await _distributedCache.RemoveAsync(CacheKeys.TestTradeCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.TestTradeCacheKeys.GetKey(TestTrade.Id));
        }
    }
}
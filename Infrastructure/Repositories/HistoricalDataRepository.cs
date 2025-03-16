using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class HistoricalDataRepository : IHistoricalDataRepository
    {
        private readonly IRepositoryAsync<HistoricalData> _repository;
        private readonly IDistributedCache _distributedCache;

        public HistoricalDataRepository(IDistributedCache distributedCache, IRepositoryAsync<HistoricalData> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<HistoricalData> HistoricalDatas => _repository.Entities;

        public async Task DeleteAsync(HistoricalData historicalData)
        {
            await _repository.DeleteAsync(historicalData);
            await _distributedCache.RemoveAsync(CacheKeys.HistoricalDataCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.HistoricalDataCacheKeys.GetKey(historicalData.Id));
        }

        public async Task<HistoricalData> GetByIdAsync(int historicalDataId)
        {
            return await _repository.Entities
                .Where(p => p.Id == historicalDataId ).FirstOrDefaultAsync();
        }

        public async Task<List<HistoricalData>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(HistoricalData historicalData)
        {
            await _repository.AddAsync(historicalData);
            await _distributedCache.RemoveAsync(CacheKeys.HistoricalDataCacheKeys.ListKey);
            return historicalData.Id;
        }

        public async Task<int> InsertRangeAsync(List<HistoricalData> historicalDatas)
        {
            await _repository.AddRangeAsync(historicalDatas);
            await _distributedCache.RemoveAsync(CacheKeys.HistoricalDataCacheKeys.ListKey);
            return 1;
        }

        public async Task UpdateAsync(HistoricalData historicalData)
        {
            await _repository.UpdateAsync(historicalData);
            await _distributedCache.RemoveAsync(CacheKeys.HistoricalDataCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.HistoricalDataCacheKeys.GetKey(historicalData.Id));
        }
    }
}
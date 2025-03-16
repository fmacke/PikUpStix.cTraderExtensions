using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private readonly IRepositoryAsync<Instrument> _repository;
        private readonly IDistributedCache _distributedCache;

        public InstrumentRepository(IDistributedCache distributedCache, IRepositoryAsync<Instrument> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Instrument> Instruments => _repository.Entities;

        public async Task DeleteAsync(Instrument instrument)
        {
            await _repository.DeleteAsync(instrument);
            await _distributedCache.RemoveAsync(CacheKeys.InstrumentCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.InstrumentCacheKeys.GetKey(instrument.Id));
        }

        public async Task<Instrument> GetByIdAsync(int instrumentId)
        {
            return await _repository.Entities
                .Include(p => p.HistoricalDatas)
                .Where(p => p.Id == instrumentId ).FirstOrDefaultAsync(e => e.Id == instrumentId);
        }

        public async Task<List<Instrument>> GetListAsync()
        {
            return await _repository.Entities.Include(p => p.HistoricalDatas).ToListAsync();
        }

        public async Task<int> InsertAsync(Instrument instrument)
        {
            await _repository.AddAsync(instrument);
            await _distributedCache.RemoveAsync(CacheKeys.InstrumentCacheKeys.ListKey);
            return instrument.Id;
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            await _repository.UpdateAsync(instrument);
            await _distributedCache.RemoveAsync(CacheKeys.InstrumentCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.InstrumentCacheKeys.GetKey(instrument.Id));
        }
    }
}
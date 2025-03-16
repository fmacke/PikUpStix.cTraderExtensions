using Microsoft.Extensions.Caching.Distributed;
using Application.Common.ThrowR;
using Infrastructure.CacheKeys;
using Application.Interfaces.Repositories;
using Application.Interfaces.CacheRepositories;
using Domain.Entities;

namespace Infrastructure.CacheRepositories
{
    public class InstrumentCacheRepository : IInstrumentCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IInstrumentRepository _instrumentRepository;

        public InstrumentCacheRepository(IDistributedCache distributedCache, IInstrumentRepository instrumentRepository)
        {
            _distributedCache = distributedCache;
            _instrumentRepository = instrumentRepository;
        }

        public async Task<Instrument> GetByIdAsync(int instrumentId)
        {
            string cacheKey = InstrumentCacheKeys.GetKey(instrumentId);
            var instrumentBytes = await _distributedCache.GetAsync(cacheKey);
            Instrument instrument = null;
            if (instrumentBytes != null)
            {
                instrument = System.Text.Json.JsonSerializer.Deserialize<Instrument>(instrumentBytes);
            }
            if (instrument == null)
            {
                instrument = await _instrumentRepository.GetByIdAsync(instrumentId);
                Throw.Exception.IfNull(instrument, "Instrument", "No Instrument Found");
                var instrumentBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(instrument);
                await _distributedCache.SetAsync(cacheKey, instrumentBytesToCache, new DistributedCacheEntryOptions());
            }
            return instrument;
        }

        public async Task<List<Instrument>> GetCachedListAsync()
        {
            string cacheKey = InstrumentCacheKeys.ListKey;
            var instrumentListBytes = await _distributedCache.GetAsync(cacheKey);
            List<Instrument> instrumentList = null;
            if (instrumentListBytes != null)
            {
                instrumentList = System.Text.Json.JsonSerializer.Deserialize<List<Instrument>>(instrumentListBytes);
            }
            if (instrumentList == null)
            {
                instrumentList = await _instrumentRepository.GetListAsync();
                var instrumentListBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(instrumentList);
                await _distributedCache.SetAsync(cacheKey, instrumentListBytesToCache, new DistributedCacheEntryOptions());
            }
            return instrumentList;
        }
    }
}
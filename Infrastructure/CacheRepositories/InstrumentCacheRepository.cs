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
        private readonly IInstrumentRepository _cVRRepository;

        public InstrumentCacheRepository(IDistributedCache distributedCache, IInstrumentRepository cVRRepository)
        {
            _distributedCache = distributedCache;
            _cVRRepository = cVRRepository;
        }

        public async Task<Instrument> GetByIdAsync(int cVRId)
        {
            string cacheKey = InstrumentCacheKeys.GetKey(cVRId);
            var cVRBytes = await _distributedCache.GetAsync(cacheKey);
            Instrument cVR = null;
            if (cVRBytes != null)
            {
                cVR = System.Text.Json.JsonSerializer.Deserialize<Instrument>(cVRBytes);
            }
            if (cVR == null)
            {
                cVR = await _cVRRepository.GetByIdAsync(cVRId);
                Throw.Exception.IfNull(cVR, "Instrument", "No Instrument Found");
                var cVRBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(cVR);
                await _distributedCache.SetAsync(cacheKey, cVRBytesToCache, new DistributedCacheEntryOptions());
            }
            return cVR;
        }

        public async Task<List<Instrument>> GetCachedListAsync()
        {
            string cacheKey = InstrumentCacheKeys.ListKey;
            var cVRListBytes = await _distributedCache.GetAsync(cacheKey);
            List<Instrument> cVRList = null;
            if (cVRListBytes != null)
            {
                cVRList = System.Text.Json.JsonSerializer.Deserialize<List<Instrument>>(cVRListBytes);
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
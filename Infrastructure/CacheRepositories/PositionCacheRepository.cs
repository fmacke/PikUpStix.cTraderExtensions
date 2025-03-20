using Microsoft.Extensions.Caching.Distributed;
using Application.Common.ThrowR;
using Infrastructure.CacheKeys;
using Application.Interfaces.Repositories;
using Application.Interfaces.CacheRepositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.CacheRepositories
{
    public class PositionCacheRepository : IPositionCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IPositionRepository _positionRepository;

        public PositionCacheRepository(IDistributedCache distributedCache, PositionRepository positionRepository)
        {
            _distributedCache = distributedCache;
            _positionRepository = positionRepository;
        }

        public async Task<Position> GetByIdAsync(int positionId)
        {
            string cacheKey = PositionCacheKeys.GetKey(positionId);
            var positionBytes = await _distributedCache.GetAsync(cacheKey);
            Position position = null;
            if (positionBytes != null)
            {
                position = System.Text.Json.JsonSerializer.Deserialize<Position>(positionBytes);
            }
            if (position == null)
            {
                position = await _positionRepository.GetByIdAsync(positionId);
                Throw.Exception.IfNull(position, "Test Trade", "No Test Trade Found");
                var positionBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(position);
                await _distributedCache.SetAsync(cacheKey, positionBytesToCache, new DistributedCacheEntryOptions());
            }
            return position;
        }

        public async Task<List<Position>> GetCachedListAsync()
        {
            string cacheKey = PositionCacheKeys.ListKey;
            var positionListBytes = await _distributedCache.GetAsync(cacheKey);
            List<Position> positionList = null;
            if (positionListBytes != null)
            {
                positionList = System.Text.Json.JsonSerializer.Deserialize<List<Position>>(positionListBytes);
            }
            if (positionList == null)
            {
                positionList = await _positionRepository.GetListAsync();
                var positionListBytesToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(positionList);
                await _distributedCache.SetAsync(cacheKey, positionListBytesToCache, new DistributedCacheEntryOptions());
            }
            return positionList;
        }
    }
}
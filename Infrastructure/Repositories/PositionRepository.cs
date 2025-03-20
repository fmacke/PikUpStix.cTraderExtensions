using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IRepositoryAsync<Position> _repository;
        private readonly IDistributedCache _distributedCache;

        public PositionRepository(IDistributedCache distributedCache, IRepositoryAsync<Position> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Position> Positions => _repository.Entities;

        public async Task DeleteAsync(Position Position)
        {
            await _repository.DeleteAsync(Position);
            await _distributedCache.RemoveAsync(CacheKeys.PositionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.PositionCacheKeys.GetKey(Position.Id));
        }

        public async Task<Position> GetByIdAsync(int id)
        {
            return await _repository.Entities
                .Where(p => p.Id == id ).FirstOrDefaultAsync();
        }

        public async Task<List<Position>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Position position)
        {
            await _repository.AddAsync(position);
            await _distributedCache.RemoveAsync(CacheKeys.PositionCacheKeys.ListKey);
            return position.Id;
        }

        public async Task<int> InsertRangeAsync(List<Position> positions)
        {
            await _repository.AddRangeAsync(positions); 
            await _distributedCache.RemoveAsync(CacheKeys.PositionCacheKeys.ListKey);
            return 1;
        }

        public async Task UpdateAsync(Position Position)
        {
            await _repository.UpdateAsync(Position);
            await _distributedCache.RemoveAsync(CacheKeys.PositionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.PositionCacheKeys.GetKey(Position.Id));
        }
    }
}
using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface IPositionCacheRepository
    {
        Task<List<Position>> GetCachedListAsync();
        Task<Position> GetByIdAsync(int testTradeId);
    }
}

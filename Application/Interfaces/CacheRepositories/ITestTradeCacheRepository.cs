using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface ITestTradeCacheRepository
    {
        Task<List<TestTrade>> GetCachedListAsync();
        Task<TestTrade> GetByIdAsync(int testTradeId);
    }
}

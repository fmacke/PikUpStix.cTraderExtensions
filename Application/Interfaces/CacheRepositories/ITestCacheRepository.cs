using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface ITestCacheRepository
    {
        Task<List<Test>> GetCachedListAsync();
        Task<Test> GetByIdAsync(int testId);
    }
}

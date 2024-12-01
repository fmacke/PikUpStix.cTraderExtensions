using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface ITestParametersCacheRepository
    {
        Task<List<Test_Parameters>> GetCachedListAsync();
        Task<Test_Parameters> GetByIdAsync(int id);
    }
}

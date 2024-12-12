using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface ITestParametersCacheRepository
    {
        Task<List<Test_Parameter>> GetCachedListAsync();
        Task<Test_Parameter> GetByIdAsync(int id);
    }
}

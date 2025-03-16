using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface IDataCacheRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetCachedListAsync();
    }
    public interface IHistoricalDataCacheRepository
    {
        Task<List<HistoricalData>> GetCachedListAsync();
        Task<HistoricalData> GetByIdAsync(int historicalDataId);
    }
}

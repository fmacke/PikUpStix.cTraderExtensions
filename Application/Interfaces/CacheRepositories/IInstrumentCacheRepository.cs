using Domain.Entities;

namespace Application.Interfaces.CacheRepositories
{
    public interface IInstrumentCacheRepository
    {
        Task<List<Instrument>> GetCachedListAsync();
        Task<Instrument> GetByIdAsync(int instrumentId);
    }
}

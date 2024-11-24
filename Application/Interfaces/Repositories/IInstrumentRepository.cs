using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IInstrumentRepository
    {
        IQueryable<Instrument> Instruments { get; }
        Task<List<Instrument>> GetListAsync();
        Task<Instrument> GetByIdAsync(int instrumentId);
        Task<int> InsertAsync(Instrument test);
        Task UpdateAsync(Instrument test);
        Task DeleteAsync(Instrument test);
    }
}

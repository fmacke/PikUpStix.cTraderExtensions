using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IHistoricalDataRepository
    {
        IQueryable<HistoricalData> HistoricalDatas { get; }
        Task<List<HistoricalData>> GetListAsync();
        Task<HistoricalData> GetByIdAsync(int historicalDataId);
        Task<int> InsertAsync(HistoricalData test);
        Task<int> InsertRangeAsync(List<HistoricalData> tests);
        Task UpdateAsync(HistoricalData test);
        Task DeleteAsync(HistoricalData test);
    }
}

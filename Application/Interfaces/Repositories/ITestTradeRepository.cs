using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITestTradeRepository
    {
        IQueryable<TestTrade> TestTrades { get; }
        Task<List<TestTrade>> GetListAsync();
        Task<TestTrade> GetByIdAsync(int testTradeId);
        Task<int> InsertAsync(TestTrade testTrade);
        Task<int> InsertRangeAsync(List<TestTrade> testTrades);
        Task UpdateAsync(TestTrade testTrade);
        Task DeleteAsync(TestTrade testTrade);
    }
}

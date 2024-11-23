using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITestRepository
    {
        IQueryable<Test> Tests { get; }
        Task<List<Test>> GetListAsync();
        Task<Test> GetByIdAsync(int testId);
        Task<int> InsertAsync(Test test);
        Task UpdateAsync(Test test);
        Task DeleteAsync(Test test);
    }
}

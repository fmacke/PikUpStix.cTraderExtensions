using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITestParametersRepository
    {
        IQueryable<Test_Parameters> Test_Parameterss { get; }
        Task<List<Test_Parameters>> GetListAsync();
        Task<Test_Parameters> GetByIdAsync(int testParameterId);
        Task<int> InsertAsync(Test_Parameters testParameter);
        Task UpdateAsync(Test_Parameters testParameter);
        Task DeleteAsync(Test_Parameters testParameter);
    }
}

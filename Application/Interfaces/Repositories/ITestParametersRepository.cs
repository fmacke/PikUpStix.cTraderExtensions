using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITestParametersRepository
    {
        IQueryable<Test_Parameter> Test_Parameterss { get; }
        Task<List<Test_Parameter>> GetListAsync();
        Task<Test_Parameter> GetByIdAsync(int testParameterId);
        Task<int> InsertAsync(Test_Parameter testParameter);
        Task UpdateAsync(Test_Parameter testParameter);
        Task DeleteAsync(Test_Parameter testParameter);
    }
}

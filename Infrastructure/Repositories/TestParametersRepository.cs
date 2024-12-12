using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class Test_ParametersRepository : ITestParametersRepository
    {
        private readonly IRepositoryAsync<Test_Parameter> _repository;
        private readonly IDistributedCache _distributedCache;

        public Test_ParametersRepository(IDistributedCache distributedCache, IRepositoryAsync<Test_Parameter> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Test_Parameter> Test_Parameters => _repository.Entities;

        //public IQueryable<Test_Parameter> Test_Parameterss => throw new NotImplementedException();

        public async Task DeleteAsync(Test_Parameter testParameter)
        {
            await _repository.DeleteAsync(testParameter);
            await _distributedCache.RemoveAsync(CacheKeys.Test_ParameterCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.Test_ParameterCacheKeys.GetKey(testParameter.Id));
        }

        public async Task<Test_Parameter> GetByIdAsync(int testParameterId)
        {
            return await _repository.Entities
                .Where(p => p.Id == testParameterId ).FirstOrDefaultAsync();
        }

        public async Task<List<Test_Parameter>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Test_Parameter testParameter)
        {
            await _repository.AddAsync(testParameter);
            await _distributedCache.RemoveAsync(CacheKeys.Test_ParameterCacheKeys.ListKey);
            return testParameter.Id;
        }

        public async Task UpdateAsync(Test_Parameter testParameter)
        {
            await _repository.UpdateAsync(testParameter);
            await _distributedCache.RemoveAsync(CacheKeys.Test_ParameterCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(CacheKeys.Test_ParameterCacheKeys.GetKey(testParameter.Id));
        }
    }
}
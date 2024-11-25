using Application.Common.Results;
using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Queries.GetAllCached;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class Tests
    {
        private ServiceProvider serviceProvider;

        public Tests(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public List<GetAllTestsCachedResponse> GetAllTestsCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<ITestService>().GetAllTestsCachedAsync();
            return result.Result.Data;
        }
        public int AddTest(CreateTestCommand model)
        {
            var id = serviceProvider.GetRequiredService<ITestService>().AddTest(model).Result.Data;
            return id;
        }
    }
    public interface ITestService
    {
        Task<Result<List<GetAllTestsCachedResponse>>> GetAllTestsCachedAsync();
        Task<Result<int>> AddTest(CreateTestCommand model);
    }
    public class TestService : ITestService
    {
        private readonly IMediator _mediator;

        public TestService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<List<GetAllTestsCachedResponse>>> GetAllTestsCachedAsync()
        {
            var query = new GetAllTestsCachedQuery();
            return await _mediator.Send(query);
        }
        public async Task<Result<int>> AddTest(CreateTestCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
    }
}

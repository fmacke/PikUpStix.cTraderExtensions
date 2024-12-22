using Application.Common.Results;
using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Update;
using Application.Features.Tests.Queries.GetAllCached;
using Application.Features.Tests.Queries.GetById;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class TestCalls
    {
        private ServiceProvider serviceProvider;

        public TestCalls(ServiceProvider serviceProvider)
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
        public GetTestByIdResponse GetTest(int id)
        {
            var result = serviceProvider.GetRequiredService<ITestService>().GetTest(id);
            return result.Result.Data;
        }
        public int UpdateTest(UpdateTestCommand model)
        {
            var id = serviceProvider.GetRequiredService<ITestService>().UpdateTest(model).Result.Data;
            return id;
        }
    }
    public interface ITestService
    {
        Task<Result<List<GetAllTestsCachedResponse>>> GetAllTestsCachedAsync();
        Task<Result<GetTestByIdResponse>> GetTest(int id);
        Task<Result<int>> AddTest(CreateTestCommand model);
        Task<Result<int>> UpdateTest(UpdateTestCommand model);
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

        public async Task<Result<int>> UpdateTest(UpdateTestCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }

        public async Task<Result<GetTestByIdResponse>> GetTest(int id)
        {
            var query = new GetTestByIdQuery();
            query.Id = id;
            return await _mediator.Send(query);
        }
    }
}

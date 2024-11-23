using Application.Common.Results;
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
    }
    public interface ITestService
    {
        Task<Result<List<GetAllTestsCachedResponse>>> GetAllTestsCachedAsync();
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
    }
}

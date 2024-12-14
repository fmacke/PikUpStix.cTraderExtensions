using Application.Common.Results;
using Application.Features.TestParameters.Commands.Create;
using Application.Features.TestParameters.Queries.GetAllCached;
using Application.Features.Tests.Commands.Create;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class TestParameters
    {
        private ServiceProvider serviceProvider;

        public TestParameters(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public List<GetAllTestParametersCachedResponse> GetAllTestsParametersCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<ITestParametersService>().GetAllTestsParametersAsync();
            return result.Result.Data;
        }
        public int AddTestParameters(CreateTestParameterCommand model)
        {
            var id = serviceProvider.GetRequiredService<ITestParametersService>().AddTestParameters(model).Result.Data;
            return id;
        }
    }
    public interface ITestParametersService
    {
        Task<Result<List<GetAllTestParametersCachedResponse>>> GetAllTestsParametersAsync();
        Task<Result<int>> AddTestParameters(CreateTestParameterCommand model);
    }
    public class TestParametersService : ITestParametersService
    {
        private readonly IMediator _mediator;

        public TestParametersService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<List<GetAllTestParametersCachedResponse>>> GetAllTestsParametersAsync()
        {
            var query = new GetAllTestParametersCachedQuery();
            return await _mediator.Send(query);
        }
        public async Task<Result<int>> AddTestParameters(CreateTestParameterCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
    }
}

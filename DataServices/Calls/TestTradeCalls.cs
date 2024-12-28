using Application.Common.Results;
using Application.Features.TestTrades.Commands.Create;
using Application.Features.TestTrades.Queries.GetAllCached;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class TestTradeCalls
    {
        private ServiceProvider serviceProvider;
        public TestTradeCalls(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public List<GetAllTestTradesCachedResponse> GetAllTestsTradesCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<ITestTradesService>().GetAllTestsTradesAsync();
            return result.Result.Data;
        }
        public int AddTestTrade(CreateTestTradeCommand model)
        {
            var id = serviceProvider.GetRequiredService<ITestTradesService>().AddTestTrade(model).Result.Data;
            return id;
        }
        public int AddTestTradeRange(CreateTestTradeRangeCommand models)
        {
            var id = serviceProvider.GetRequiredService<ITestTradesService>().AddTestTradeRange(models).Result.Data;
            return id;
        }
    }
    public interface ITestTradesService
    {
        Task<Result<List<GetAllTestTradesCachedResponse>>> GetAllTestsTradesAsync();
        Task<Result<int>> AddTestTrade(CreateTestTradeCommand model);
        Task<Result<int>> AddTestTradeRange(CreateTestTradeRangeCommand models);
    }
    public class TestTradesService : ITestTradesService
    {
        private readonly IMediator _mediator;
        public TestTradesService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<List<GetAllTestTradesCachedResponse>>> GetAllTestsTradesAsync()
        { 
            var query = new GetAllTestTradesCachedQuery();
            return await _mediator.Send(query);
        }
        public async Task<Result<int>> AddTestTrade(CreateTestTradeCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
        public async Task<Result<int>> AddTestTradeRange(CreateTestTradeRangeCommand models)
        {
            var result = await _mediator.Send(models);
            return result;
        }
    }
}

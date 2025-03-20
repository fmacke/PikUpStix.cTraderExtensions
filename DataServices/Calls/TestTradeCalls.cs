using Application.Common.Results;
using Application.Features.Positions.Commands.Create;
using Application.Features.TestTrades.Queries.GetAllCached;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class PositionCalls
    {
        private ServiceProvider serviceProvider;
        public PositionCalls(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public List<GetAllPositionsCachedResponse> GetAllTestsTradesCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<IPositionsService>().GetAllTestsTradesAsync();
            return result.Result.Data;
        }
        public int AddPosition(CreatePositionCommand model)
        {
            var id = serviceProvider.GetRequiredService<IPositionsService>().AddPosition(model).Result.Data;
            return id;
        }
        public int AddPositionRange(CreatePositionRangeCommand models)
        {
            var id = serviceProvider.GetRequiredService<IPositionsService>().AddPositionRange(models).Result.Data;
            return id;
        }
    }
    public interface IPositionsService
    {
        Task<Result<List<GetAllPositionsCachedResponse>>> GetAllTestsTradesAsync();
        Task<Result<int>> AddPosition(CreatePositionCommand model);
        Task<Result<int>> AddPositionRange(CreatePositionRangeCommand models);
    }
    public class PositionsService : IPositionsService
    {
        private readonly IMediator _mediator;
        public PositionsService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<List<GetAllPositionsCachedResponse>>> GetAllTestsTradesAsync()
        { 
            var query = new GetAllPositionsCachedQuery();
            return await _mediator.Send(query);
        }
        public async Task<Result<int>> AddPosition(CreatePositionCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
        public async Task<Result<int>> AddPositionRange(CreatePositionRangeCommand models)
        {
            var result = await _mediator.Send(models);
            return result;
        }
    }
}

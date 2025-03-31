using Application.Common.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Features.HistoricalDatas.Create;
using Application.Features.HistoricalDatas.Queries.GetAllCached;

namespace DataServices.Calls
{
    public class HistoricalDataCalls
    {
        private ServiceProvider serviceProvider;

        public HistoricalDataCalls(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public List<GetAllHistoricalDataCachedResponse> GetAllHistoricalDataCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<IHistoricalDataService>().GetAllHistoricalDataCachedAsync();
            return result.Result.Data;
        }
        public int AddHistoricalData(CreateHistoricalDataCommand model)
        {
            var id = serviceProvider.GetRequiredService<IHistoricalDataService>().AddHistoricalData(model).Result.Data;
            return id;
        }

        public int AddHistoricalDataRange(CreateHistoricalDataRangeCommand historicalData)
        {
            var result = serviceProvider.GetRequiredService<IHistoricalDataService>().AddHistoricalDataRange(historicalData).Result.Data;
            return result;
        }
    }
    public interface IHistoricalDataService
    {
        Task<Result<List<GetAllHistoricalDataCachedResponse>>> GetAllHistoricalDataCachedAsync();
        Task<Result<int>> AddHistoricalData(CreateHistoricalDataCommand model);
        Task<Result<int>> AddHistoricalDataRange(CreateHistoricalDataRangeCommand model);
    }
    public class HistoricalDataService : IHistoricalDataService
    {
        private readonly IMediator _mediator;

        public HistoricalDataService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<List<GetAllHistoricalDataCachedResponse>>> GetAllHistoricalDataCachedAsync()
        {
            var query = new GetAllHistoricalDataCachedQuery();
            return await _mediator.Send(query);
        }
        public async Task<Result<int>> AddHistoricalData(CreateHistoricalDataCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
        public async Task<Result<int>> AddHistoricalDataRange(CreateHistoricalDataRangeCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
    }
}

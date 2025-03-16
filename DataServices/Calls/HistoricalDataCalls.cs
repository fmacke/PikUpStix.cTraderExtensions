using Application.Common.Results;
using Application.Features.HistoricalDatas.Commands.Create;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class HistoricalDataCalls
    {
        private ServiceProvider serviceProvider;

        public HistoricalDataCalls(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public int AddHistoricalData(CreateHistoricalDataCommand model)
        {
            var id = serviceProvider.GetRequiredService<IHistoricalDataService>().AddHistoricalData(model).Result.Data;
            return id;
        }

        public int AddHistoricalDataRange(CreateHistoricalDataRangeCommand historicalDatas)
        {
            var result = serviceProvider.GetRequiredService<IHistoricalDataService>().AddHistoricalDataRange(historicalDatas).Result.Data;
            return result;
        }
    }
    public interface IHistoricalDataService
    {
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

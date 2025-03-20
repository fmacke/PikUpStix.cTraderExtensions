using Application.Common.Results;
using Application.Features.Instruments.Commands.Create;
using Application.Features.Instruments.Queries.GetAllCached;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class InstrumentCalls
    {
        private ServiceProvider serviceProvider;

        public InstrumentCalls(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public List<GetAllInstrumentsCachedResponse> GetAllInstrumentsCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<IInstrumentService>().GetAllInstrumentsCachedAsync();
            return result.Result.Data;
        }
        public int AddInstrument(CreateInstrumentCommand model)
        {
            var id = serviceProvider.GetRequiredService<IInstrumentService>().AddInstrument(model).Result.Data;
            return id;
        }
    }
    public interface IInstrumentService
    {
        Task<Result<List<GetAllInstrumentsCachedResponse>>> GetAllInstrumentsCachedAsync();
        Task<Result<int>> AddInstrument(CreateInstrumentCommand model);
    }
    public class InstrumentService : IInstrumentService
    {
        private readonly IMediator _mediator;

        public InstrumentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<List<GetAllInstrumentsCachedResponse>>> GetAllInstrumentsCachedAsync()
        {
            var query = new GetAllInstrumentsCachedQuery();
            return await _mediator.Send(query);
        }
        public async Task<Result<int>> AddInstrument(CreateInstrumentCommand model)
        {
            var result = await _mediator.Send(model);
            return result;
        }
    }
}

using Application.Common.Results;
using Application.Features.Instruments.Queries.GetAllCached;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices.Calls
{
    public class Instruments
    {
        private ServiceProvider serviceProvider;

        public Instruments(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public List<GetAllInstrumentsCachedResponse> GetAllInstrumentsCachedAsync()
        {
            var result = serviceProvider.GetRequiredService<IInstrumentService>().GetAllInstrumentsCachedAsync();
            return result.Result.Data;
        }
    }
    public interface IInstrumentService
    {
        Task<Result<List<GetAllInstrumentsCachedResponse>>> GetAllInstrumentsCachedAsync();
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
    }
}

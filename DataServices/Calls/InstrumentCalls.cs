using Application.Common.Results;
using Application.Features.HistoricalDatas.Create;
using Application.Features.Instruments.Commands.Create;
using Application.Features.Instruments.Queries.GetAllCached;
using Application.Features.Instruments.Queries.GetById;
using Application.Features.Tests.Queries.GetById;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
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
        public GetInstrumentByIdResponse GetInstrument(int id)
        {
            var result = serviceProvider.GetRequiredService<IInstrumentService>().GetInstrument(id);
            return result.Result.Data;
        }
        public int AddInstrument(CreateInstrumentCommand model)
        {
            var id = serviceProvider.GetRequiredService<IInstrumentService>().AddInstrument(model).Result.Data;
            return id;
        }
        public void AddOrUpdateInstrument(Instrument instrument)
        {
            var existingInstrumentData = new Instrument();
            var instruments = GetAllInstrumentsCachedAsync();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<InstrumentProfile>());
            var mapper = config.CreateMapper();
            if (instruments.Any(x => x.InstrumentName == instrument.InstrumentName
                && x.DataSource == instrument.DataSource
                && x.Frequency == instrument.Frequency))
            {
                existingInstrumentData = mapper.Map<Instrument>(instruments.First(x => x.InstrumentName == instrument.InstrumentName
                    && x.DataSource == instrument.DataSource
                    && x.Frequency == instrument.Frequency));
                AddAnyNewDataToDb(existingInstrumentData, instrument);
            }
            else
            {
                var addInstrumentCommand = mapper.Map<CreateInstrumentCommand>(instrument);
                AddInstrument(addInstrumentCommand);
            }
        }
        private void AddAnyNewDataToDb(Instrument existingInstrumentData, Instrument instrument)
        {
            var rangeToAdd = new CreateHistoricalDataRangeCommand();
            foreach (var bar in instrument.HistoricalDatas)
            {
                if (!existingInstrumentData.HistoricalDatas.Any(x => x.Date.Value.Year == bar.Date.Value.Year
                    && x.Date.Value.Month == bar.Date.Value.Month
                    && x.Date.Value.Day == bar.Date.Value.Day
                    && x.Date.Value.Hour == bar.Date.Value.Hour
                    && x.Date.Value.Minute == bar.Date.Value.Minute
                    && x.Date.Value.Second == bar.Date.Value.Second))
                {
                    // Add this new data to db
                    rangeToAdd.Add(new CreateHistoricalDataCommand()
                    {
                        Date = bar.Date,
                        OpenPrice = bar.OpenPrice,
                        ClosePrice = bar.ClosePrice,
                        LowPrice = bar.LowPrice,
                        HighPrice = bar.HighPrice,
                        Volume = bar.Volume,
                        Settle = bar.Settle,
                        OpenInterest = bar.OpenInterest,
                        InstrumentId = existingInstrumentData.Id
                    });
                }
            }
            new HistoricalDataCalls(serviceProvider).AddHistoricalDataRange(rangeToAdd);
        }
    }
    public interface IInstrumentService
    {
        Task<Result<List<GetAllInstrumentsCachedResponse>>> GetAllInstrumentsCachedAsync();
        Task<Result<GetInstrumentByIdResponse>> GetInstrument(int id);
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
        public async Task<Result<GetInstrumentByIdResponse>> GetInstrument(int id)
        {
            var query = new GetInstrumentByIdQuery();
            query.Id = id;
            return await _mediator.Send(query);
        }
    }
}

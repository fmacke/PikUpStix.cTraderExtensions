using Application.Common.Results;
using Application.Features.HistoricalDatas.Create;
using Application.Features.Instruments.Commands.Create;
using Application.Features.Instruments.Queries.GetAllCached;
using Application.Features.Instruments.Queries.GetById;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        private void AddAnyNewDataToDb(Instrument existingInstrumentData, Instrument newInstrumentData)
        {
            DateTime? latestExistingDate = existingInstrumentData.HistoricalDatas
                                                .OrderByDescending(x => x.Date)
                                                .Select(x => (DateTime?)x.Date) // Cast to nullable DateTime
                                                .FirstOrDefault();

            var rangeToAdd = new CreateHistoricalDataRangeCommand();

            if (latestExistingDate.HasValue)
            {
                // 2. Filter new data: Only consider bars that are strictly newer than the latest existing bar.
                // Using a strict greater than (>) to avoid adding duplicates if dates are identical but other data differs
                // (e.g., if you only store minutes, but get seconds in new data, you might want to adjust precision here).
                // For exact matches, consider comparing up to the precision you care about (e.g., .Ticks)
                var newDataToProcess = newInstrumentData.HistoricalDatas
                                            .Where(bar => bar.Date > latestExistingDate.Value)
                                            .OrderBy(bar => bar.Date); // Order by date to add them chronologically

                foreach (var bar in newDataToProcess)
                {
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
            else
            {
                // If there's no existing data, add all new data.
                // This assumes newInstrumentData.HistoricalDatas is sorted or will be added in order.
                foreach (var bar in newInstrumentData.HistoricalDatas.OrderBy(x => x.Date))
                {
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

            // 3. Add the filtered range to the database
            if (rangeToAdd.Any()) // Only call if there's data to add
            {
                new HistoricalDataCalls(serviceProvider).AddHistoricalDataRange(rangeToAdd);
            }
            else
            {
                Console.WriteLine("No new data found to add.");
            }
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

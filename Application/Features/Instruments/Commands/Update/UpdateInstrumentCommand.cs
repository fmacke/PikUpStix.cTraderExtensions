using MediatR;
using Application.Common.Interfaces;
using Application.Interfaces.Repositories;
using Application.Common.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Instruments.Commands.Update
{
    public class UpdateInstrumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public string Provider { get; set; }
        public string DataName { get; set; }
        public string DataSource { get; set; }
        public string Format { get; set; }
        public string Frequency { get; set; }
        public string Sort { get; set; }
        public double ContractUnit { get; set; }
        public string ContractUnitType { get; set; }
        public string PriceQuotation { get; set; }
        public double MinimumPriceFluctuation { get; set; }
        public string Currency { get; set; }
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; }
        public virtual ICollection<PortfolioInstrument> PortfolioInstruments { get; set; }
        public virtual ICollection<TestTrade> Test_Trades { get; set; }
    }
    public class UpdateInstrumentCommandHandler : IRequestHandler<UpdateInstrumentCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IMapper _mapper;

        public UpdateInstrumentCommandHandler(IInstrumentRepository instrumentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _instrumentRepository = instrumentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateInstrumentCommand command, CancellationToken cancellationToken)
        {
            var instrument = await _instrumentRepository.GetByIdAsync(command.Id);

            if (instrument == null)
            {
                return Result<int>.Fail($"Instrument Not Found.");
            }
            else
            {
                UpdateModifiedInstrument(ref instrument, command);
                await _instrumentRepository.UpdateAsync(instrument);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(instrument.Id);
            }
        }

        private void UpdateModifiedInstrument(ref Instrument instrument, UpdateInstrumentCommand command)
        {
            instrument.InstrumentName = command.InstrumentName;
            instrument.DataName = command.DataName;
            instrument.DataSource = command.DataSource;
            instrument.Provider = command.Provider;
            instrument.PriceQuotation = command.PriceQuotation;
            instrument.Currency = command.Currency;
            instrument.Frequency = command.Frequency;
            instrument.ContractUnit = command.ContractUnit;
            instrument.ContractUnitType = command.ContractUnitType;
            instrument.MinimumPriceFluctuation = command.MinimumPriceFluctuation;
            instrument.Format = command.Format;
            instrument.Sort = command.Sort;
            //instrument.HistoricalDatas = instrument.HistoricalDatas;
        }
    }
}
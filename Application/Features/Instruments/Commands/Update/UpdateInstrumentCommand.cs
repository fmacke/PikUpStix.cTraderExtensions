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
        public decimal ContractUnit { get; set; }
        public string ContractUnitType { get; set; }
        public string PriceQuotation { get; set; }
        public decimal MinimumPriceFluctuation { get; set; }
        public string Currency { get; set; }
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; }
        public virtual ICollection<PortfolioInstrument> PortfolioInstruments { get; set; }
        public virtual ICollection<Test_Trades> Test_Trades { get; set; }
    }
    public class UpdateInstrumentCommandHandler : IRequestHandler<UpdateInstrumentCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInstrumentRepository _cvrRepository;
        private readonly IMapper _mapper;

        public UpdateInstrumentCommandHandler(IInstrumentRepository cvrRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cvrRepository = cvrRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateInstrumentCommand command, CancellationToken cancellationToken)
        {
            var cvr = await _cvrRepository.GetByIdAsync(command.Id);

            if (cvr == null)
            {
                return Result<int>.Fail($"Instrument Not Found.");
            }
            else
            {
                UpdateModifiedInstrument(ref cvr, command);
                await _cvrRepository.UpdateAsync(cvr);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(cvr.Id);
            }
        }

        private void UpdateModifiedInstrument(ref Instrument cvr, UpdateInstrumentCommand command)
        {

        }
    }
}
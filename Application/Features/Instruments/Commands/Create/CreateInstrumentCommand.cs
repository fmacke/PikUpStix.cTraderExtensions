using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Instruments.Commands.Create
{
    public partial class CreateInstrumentCommand : IRequest<Result<int>>
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

    public class CreateInstrumentCommandHandler : IRequestHandler<CreateInstrumentCommand, Result<int>>
    {
        private readonly IInstrumentRepository _cvrRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateInstrumentCommandHandler(IInstrumentRepository cvrRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cvrRepository = cvrRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var cvr = _mapper.Map<Instrument>(request);
            await _cvrRepository.InsertAsync(cvr);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(cvr.Id);
        }
    }
}

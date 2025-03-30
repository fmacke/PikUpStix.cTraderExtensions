using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Features.HistoricalDatas.Create
{
    public partial class CreateHistoricalDataCommand : IRequest<Result<int>>
    {

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public double LowPrice { get; set; }
        public double HighPrice { get; set; }
        public double Volume { get; set; }
        public double Settle { get; set; }
        public double OpenInterest { get; set; }
        public int InstrumentId { get; set; }
    }

    public class CreateHistoricalDataCommandHandler : IRequestHandler<CreateHistoricalDataCommand, Result<int>>
    {
        private readonly IHistoricalDataRepository _historicalDataRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateHistoricalDataCommandHandler(IHistoricalDataRepository historicalDataRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _historicalDataRepository = historicalDataRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateHistoricalDataCommand request, CancellationToken cancellationToken)
        {
            var historicalData = _mapper.Map<HistoricalData>(request);
            await _historicalDataRepository.InsertAsync(historicalData);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(historicalData.Id);
        }
    }
}

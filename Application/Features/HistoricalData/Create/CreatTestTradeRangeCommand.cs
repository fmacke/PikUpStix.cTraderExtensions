using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.HistoricalDatas.Commands.Create
{
    public partial class CreateHistoricalDataRangeCommand : List<CreateHistoricalDataCommand>, IRequest<Result<int>>
    {
    }
    public class CreateHistoricalDatasRangeCommandHandler : IRequestHandler<CreateHistoricalDataRangeCommand, Result<int>>
    {
        private readonly IHistoricalDataRepository _HistoricalDatasRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateHistoricalDatasRangeCommandHandler(IHistoricalDataRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _HistoricalDatasRepository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateHistoricalDataRangeCommand request, CancellationToken cancellationToken)
        {
            var historicalDatas = _mapper.Map<List<HistoricalData>>(request);
            await _HistoricalDatasRepository.InsertRangeAsync(historicalDatas);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success("range added successfully");
        }
    }
}

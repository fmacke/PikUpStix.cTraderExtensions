using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.TestTrades.Commands.Create
{
    public partial class CreateTestTradeRangeCommand : List<CreateTestTradeCommand>, IRequest<Result<int>>
    {
    }
    public class CreateTestTradesRangeCommandHandler : IRequestHandler<CreateTestTradeRangeCommand, Result<int>>
    {
        private readonly ITestTradeRepository _TestTradesRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateTestTradesRangeCommandHandler(ITestTradeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _TestTradesRepository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateTestTradeRangeCommand request, CancellationToken cancellationToken)
        {
            var testTrades = _mapper.Map<List<TestTrade>>(request);
            await _TestTradesRepository.InsertRangeAsync(testTrades);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success("range added successfully");
        }
    }
}

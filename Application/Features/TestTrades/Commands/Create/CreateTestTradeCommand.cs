    using AutoMapper;
    using MediatR;
    using Application.Common.Results;
    using Application.Common.Interfaces;
    using Domain.Entities;
using Application.Features.TestParameters.Commands.Create;
    using Application.Interfaces.Repositories;


    namespace Application.Features.TestTrades.Commands.Create
    {
        public partial class CreateTestTradeCommand : IRequest<Result<int>>
        {
            public int Id { get; set; }
            public int TestId { get; set; }
            public int InstrumentId { get; set; }
            public double Volume { get; set; }
            public string Direction { get; set; }
            public double EntryPrice { get; set; }
            public double TakeProfit { get; set; }
            public double StopLoss { get; set; }
            public double Commission { get; set; }
            public DateTime Created { get; set; }
            public string Comment { get; set; }
            public double ClosePrice { get; set; }
            public int TrailingStop { get; set; }
            public double Margin { get; set; }
            public string InstrumentWeight { get; set; }
            public string Status { get; set; }
            public DateTime? ClosedAt { get; set; }
            public double? CapitalAtEntry { get; set; }
            public double? CapitalAtClose { get; set; }
            public double? ForecastAtEntry { get; set; }
            public double? ForecastAtClose { get; set; }
        }
        public class CreateTestTradesCommandHandler : IRequestHandler<CreateTestTradeCommand, Result<int>>
        {
            private readonly ITestTradeRepository _TestTradesRepository;
            private readonly IMapper _mapper;

            private IUnitOfWork _unitOfWork { get; set; }

            public CreateTestTradesCommandHandler(ITestTradeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _TestTradesRepository = repository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(CreateTestTradeCommand request, CancellationToken cancellationToken)
            {
                var testTrades = _mapper.Map<TestTrade>(request);
                await _TestTradesRepository.InsertAsync(testTrades);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testTrades.Id);
            }

        }
    }

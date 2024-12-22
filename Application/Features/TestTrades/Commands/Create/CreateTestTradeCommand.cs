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
            public decimal Volume { get; set; }
            public string Direction { get; set; }
            public decimal EntryPrice { get; set; }
            public decimal TakeProfit { get; set; }
            public decimal StopLoss { get; set; }
            public decimal Commission { get; set; }
            public DateTime Created { get; set; }
            public string Comment { get; set; }
            public decimal ClosePrice { get; set; }
            public int TrailingStop { get; set; }
            public decimal Margin { get; set; }
            public string InstrumentWeight { get; set; }
            public string Status { get; set; }
            public DateTime? ClosedAt { get; set; }
            public decimal? CapitalAtEntry { get; set; }
            public decimal? CapitalAtClose { get; set; }
            public decimal? ForecastAtEntry { get; set; }
            public decimal? ForecastAtClose { get; set; }
        }
        public class CreateTestTradesCommandHandler : IRequestHandler<CreateTestParameterCommand, Result<int>>
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

            public async Task<Result<int>> Handle(CreateTestParameterCommand request, CancellationToken cancellationToken)
            {
                var testTrades = _mapper.Map<TestTrade>(request);
                await _TestTradesRepository.InsertAsync(testTrades);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testTrades.Id);
            }

        }
    }

using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.Tests.Commands.Create
{
    public partial class CreateTestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal StartingCapital { get; set; }
        public decimal EndingCapital { get; set; }
        public string Description { get; set; }
        public DateTime? TestRunAt { get; set; }
        public DateTime? TestEndAt { get; set; }
        public double? MaxAdverseExcursion { get; set; }
        public double? SharpeRatio { get; set; }
        public decimal NetProfit { get; set; }
        public decimal Commission { get; set; }
        public double MaxEquityDrawdown { get; set; }
        public double MaxBalanceDrawdown { get; set; }
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int MaxConsecutiveWinningTrades { get; set; }
        public decimal LargestWinningTrade { get; set; }
        public int LosingTrades { get; set; }
        public int MaxConsecutiveLosingTrades { get; set; }
        public decimal LargestLosingTrades { get; set; }
        public decimal AverageTrade { get; set; }
        public double SortinoRatio { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossLoss { get; set; }
        public decimal NetShortProfit { get; set; }
        public decimal NetLongProfit { get; set; }
        public decimal GrossShortProfit { get; set; }
        public decimal GrossLongProfit { get; set; }
        public double ProfitFactor { get; set; }
        public double ProfitFactorLongTrades { get; set; }
        public double ProfitFactorShortTrades { get; set; }
        public decimal NetShortLoss { get; set; }
        public decimal NetLongLoss { get; set; }
        public decimal GrossShortLoss { get; set; }
        public decimal GrossLongLoss { get; set; }
        public double ProfitableTradesRatio { get; set; }
        public double LosingTradesRatio { get; set; }
        public double ProfitableLongTradesRatio { get; set; }
        public double ProfitableShortTradesRatio { get; set; }
        public decimal AverageWin { get; set; }
        public decimal AverageWinLong { get; set; }
        public decimal AverageWinShort { get; set; }
        public decimal AverageLoss { get; set; }
        public decimal AverageLossLong { get; set; }
        public decimal AverageLossShort { get; set; }
        //public ICollection<TestTrade> TestTrades { get; set; } = new HashSet<TestTrade>();
        //public ICollection<Test_Parameter> Test_Parameters { get; set; } = new HashSet<Test_Parameter>();
    }
    public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, Result<int>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateTestCommandHandler(ITestRepository testRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            var test = _mapper.Map<Test>(request);
            await _testRepository.InsertAsync(test);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(test.Id);
        }

    }
}

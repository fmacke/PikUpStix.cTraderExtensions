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
        public double StartingCapital { get; set; }
        public double EndingCapital { get; set; }
        public string Description { get; set; }
        public DateTime? TestRunAt { get; set; }
        public DateTime? TestEndAt { get; set; }
        public double? MaxAdverseExcursion { get; set; }
        public double? SharpeRatio { get; set; }
        public double NetProfit { get; set; }
        public double Commission { get; set; }
        public double MaxEquityDrawdown { get; set; }
        public double MaxBalanceDrawdown { get; set; }
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int MaxConsecutiveWinningTrades { get; set; }
        public double LargestWinningTrade { get; set; }
        public int LosingTrades { get; set; }
        public int MaxConsecutiveLosingTrades { get; set; }
        public double LargestLosingTrades { get; set; }
        public double AverageTrade { get; set; }
        public double SortinoRatio { get; set; }
        public double GrossProfit { get; set; }
        public double GrossLoss { get; set; }
        public double NetShortProfit { get; set; }
        public double NetLongProfit { get; set; }
        public double GrossShortProfit { get; set; }
        public double GrossLongProfit { get; set; }
        public double ProfitFactor { get; set; }
        public double ProfitFactorLongTrades { get; set; }
        public double ProfitFactorShortTrades { get; set; }
        public double NetShortLoss { get; set; }
        public double NetLongLoss { get; set; }
        public double GrossShortLoss { get; set; }
        public double GrossLongLoss { get; set; }
        public double ProfitableTradesRatio { get; set; }
        public double LosingTradesRatio { get; set; }
        public double ProfitableLongTradesRatio { get; set; }
        public double ProfitableShortTradesRatio { get; set; }
        public double AverageWin { get; set; }
        public double AverageWinLong { get; set; }
        public double AverageWinShort { get; set; }
        public double AverageLoss { get; set; }
        public double AverageLossLong { get; set; }
        public double AverageLossShort { get; set; }
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

using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using AutoMapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.Tests.Commands.Update
{
    public class UpdateTestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime FromDate { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime ToDate { get; set; }
        [Column(TypeName = "money")]
        public decimal StartingCapital { get; set; }
        [Column(TypeName = "money")]
        public decimal EndingCapital { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TestRunAt { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? TestEndAt { get; set; }
        public double? MaxAdverseExcursion { get; set; }
        public double? SharpeRatio { get; set; }
        [Column(TypeName = "money")]
        public decimal NetProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal Commission { get; set; }
        public double MaxEquityDrawdown { get; set; }
        public double MaxBalanceDrawdown { get; set; }
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int MaxConsecutiveWinningTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal LargestWinningTrade { get; set; }
        public int LosingTrades { get; set; }
        public int MaxConsecutiveLosingTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal LargestLosingTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageTrade { get; set; }
        public double SortinoRatio { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal NetShortProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal NetLongProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossShortProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossLongProfit { get; set; }
        public double ProfitFactor { get; set; }
        public double ProfitFactorLongTrades { get; set; }
        public double ProfitFactorShortTrades { get; set; }
        [Column(TypeName = "money")]
        public decimal NetShortLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal NetLongLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossShortLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal GrossLongLoss { get; set; }
        public double ProfitableTradesRatio { get; set; }
        public double LosingTradesRatio { get; set; }
        public double ProfitableLongTradesRatio { get; set; }
        public double ProfitableShortTradesRatio { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageWin { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageWinLong { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageWinShort { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageLoss { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageLossLong { get; set; }
        [Column(TypeName = "money")]
        public decimal AverageLossShort { get; set; }

    }
    
    public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;

        public UpdateTestCommandHandler(ITestRepository testRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateTestCommand command, CancellationToken cancellationToken)
        {
            var test = await _testRepository.GetByIdAsync(command.Id);

            if (test == null)
            {
                return Result<int>.Fail($"Test Not Found.");
            }
            else
            {
                UpdateModifiedTest(ref test, command);
                await _testRepository.UpdateAsync(test);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(test.Id);
            }
        }

        private void UpdateModifiedTest(ref Test test, UpdateTestCommand command)
        {
            test.MaxAdverseExcursion = command.MaxAdverseExcursion ?? test.MaxAdverseExcursion;
            test.SharpeRatio = command.SharpeRatio ?? test.SharpeRatio;
        }
    }
}
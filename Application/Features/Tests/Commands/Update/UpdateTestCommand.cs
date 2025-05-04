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
            test.FromDate = command.FromDate;
            test.ToDate = command.ToDate;
            test.MaxAdverseExcursion = command.MaxAdverseExcursion ?? test.MaxAdverseExcursion;
            test.SharpeRatio = command.SharpeRatio ?? test.SharpeRatio;
            test.EndingCapital = command.EndingCapital;
            test.AverageWinShort = command.AverageWinShort;
            test.Commission = command.Commission;
            test.ProfitFactor = command.ProfitFactor;
            test.TestEndAt = command.TestEndAt;
            test.NetProfit = command.NetProfit;
            test.AverageLoss = command.AverageLoss;
            test.AverageLossLong = command.AverageLoss;
            test.AverageLossShort = command.AverageLossShort;
            test.AverageTrade = command.AverageTrade;
            test.MaxAdverseExcursion = command.MaxAdverseExcursion;
            test.MaxEquityDrawdown = command.MaxEquityDrawdown;
            test.WinningTrades = command.WinningTrades;
            test.TotalTrades = command.TotalTrades;
            test.LosingTrades = command.LosingTrades;
            test.MaxConsecutiveLosingTrades = command.MaxConsecutiveLosingTrades;
            test.MaxConsecutiveWinningTrades = command.MaxConsecutiveWinningTrades;
            test.MaxBalanceDrawdown = command.MaxBalanceDrawdown;
            test.LargestLosingTrades = command.LargestLosingTrades;
            test.LargestWinningTrade = command.LargestWinningTrade;
            test.SortinoRatio = command.SortinoRatio;
            test.GrossProfit = command.GrossProfit;
            test.GrossLoss = command.GrossLoss;
            test.NetLongProfit = command.NetLongProfit;
            test.NetLongLoss = command.NetLongLoss;
            test.NetShortLoss = command.NetShortLoss;
            test.NetShortProfit = command.NetShortProfit;
            test.GrossLongProfit = command.GrossLongProfit;
            test.GrossLongLoss = command.GrossLongLoss;
            test.GrossShortLoss = command.GrossShortLoss;
            test.GrossShortProfit = command.GrossShortProfit;
            test.ProfitFactorLongTrades = command.ProfitFactorLongTrades;
            test.ProfitFactorShortTrades = command.ProfitFactorShortTrades;
            test.ProfitableTradesRatio = command.ProfitableTradesRatio;
            test.ProfitableShortTradesRatio = command.ProfitableShortTradesRatio;
            test.ProfitableLongTradesRatio = command.ProfitableLongTradesRatio;
            test.AverageWin = command.AverageWin;
            test.AverageWinLong = command.AverageWinLong;
            test.AverageWinShort = command.AverageWinShort;
            test.AverageLossLong = command.AverageLossLong;
            test.AverageLossShort = command.AverageLossShort;
            test.AverageLoss = command.AverageLoss;

        }
    }
}
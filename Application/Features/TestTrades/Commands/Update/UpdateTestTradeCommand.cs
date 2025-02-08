using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using AutoMapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.TestTrades.Commands.Update
{
    public class UpdateTestTradesCommand : IRequest<Result<int>>
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
    
    public class UpdateTestTradesCommandHandler : IRequestHandler<UpdateTestTradesCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITestTradeRepository _testTradesRepository;
        private readonly IMapper _mapper;

        public UpdateTestTradesCommandHandler(ITestTradeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _testTradesRepository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateTestTradesCommand command, CancellationToken cancellationToken)
        {
            var testtrade = await _testTradesRepository.GetByIdAsync(command.Id);

            if (testtrade == null)
            {
                return Result<int>.Fail($"TestTrades Not Found.");
            }
            else
            {
                UpdateModifiedTestTrades(ref testtrade, command);
                await _testTradesRepository.UpdateAsync(testtrade);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testtrade.Id);
            }
        }

        private void UpdateModifiedTestTrades(ref TestTrade testTrade, UpdateTestTradesCommand command)
        {
            testTrade.Volume = command.Volume;
            testTrade.Direction = command.Direction;
            testTrade.EntryPrice = command.EntryPrice;
            testTrade.TakeProfit = command.TakeProfit;
            testTrade.StopLoss = command.StopLoss;
            testTrade.Commission = command.Commission;
            testTrade.Created = command.Created;
            testTrade.Comment = command.Comment;
            testTrade.ClosePrice = command.ClosePrice;
            testTrade.TrailingStop = command.TrailingStop;
            testTrade.Margin = command.Margin;
            testTrade.InstrumentWeight = command.InstrumentWeight;
            testTrade.Status = command.Status;
            testTrade.ClosedAt = command.ClosedAt;
            testTrade.CapitalAtEntry = command.CapitalAtEntry;
            testTrade.CapitalAtClose = command.CapitalAtClose;
            testTrade.ForecastAtEntry = command.ForecastAtEntry;
            testTrade.ForecastAtClose = command.ForecastAtClose;
        }
    }
}
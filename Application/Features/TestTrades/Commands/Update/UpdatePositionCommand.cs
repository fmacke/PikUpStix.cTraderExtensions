using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Domain.Enums;

namespace Application.Features.Positions.Commands.Update
{
    public class UpdatePositionsCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int InstrumentId { get; set; }
        public double Volume { get; set; } = 0;
        public PositionType PositionType { get; set; }
        public double EntryPrice { get; set; }
        public double? TakeProfit { get; set; }
        public double? StopLoss { get; set; }
        public double Commission { get; set; } = 0;
        public DateTime Created { get; set; }
        public string Comment { get; set; }
        public double? ClosePrice { get; set; }
        public double? TrailingStop { get; set; }
        public double Margin { get; set; } = 0;
        public PositionStatus Status { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string SymbolName { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    
    public class UpdatePositionsCommandHandler : IRequestHandler<UpdatePositionsCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPositionRepository _positionsRepository;
        private readonly IMapper _mapper;

        public UpdatePositionsCommandHandler(IPositionRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _positionsRepository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdatePositionsCommand command, CancellationToken cancellationToken)
        {
            var testtrade = await _positionsRepository.GetByIdAsync(command.Id);

            if (testtrade == null)
            {
                return Result<int>.Fail($"Positions Not Found.");
            }
            else
            {
                UpdateModifiedPositions(ref testtrade, command);
                await _positionsRepository.UpdateAsync(testtrade);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testtrade.Id);
            }
        }

        private void UpdateModifiedPositions(ref Position position, UpdatePositionsCommand command)
        {
            position.Volume = command.Volume;
            position.PositionType = command.PositionType;
            position.EntryPrice = command.EntryPrice;
            position.TakeProfit = command.TakeProfit;
            position.StopLoss = command.StopLoss;
            position.Commission = command.Commission;
            position.Created = command.Created;
            position.Comment = command.Comment;
            position.ClosePrice = command.ClosePrice;
            position.TrailingStop = command.TrailingStop;
            position.Margin = command.Margin;
            position.Status = command.Status;
            position.ClosedAt = command.ClosedAt;
        }
    }
}
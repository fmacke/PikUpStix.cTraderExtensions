using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces.Repositories;


namespace Application.Features.Positions.Commands.Create
    {
        public partial class CreatePositionCommand : IRequest<Result<int>>
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
        public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Result<int>>
        {
            private readonly IPositionRepository _PositionsRepository;
            private readonly IMapper _mapper;

            private IUnitOfWork _unitOfWork { get; set; }

            public CreatePositionCommandHandler(IPositionRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _PositionsRepository = repository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
            {
                var positions = _mapper.Map<Position>(request);
                await _PositionsRepository.InsertAsync(positions);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(positions.Id);
            }

        }
    }

using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.Positions.Commands.Create
{
    public partial class CreatePositionRangeCommand : List<CreatePositionCommand>, IRequest<Result<int>>
    {
    }
    public class CreatePositionsRangeCommandHandler : IRequestHandler<CreatePositionRangeCommand, Result<int>>
    {
        private readonly IPositionRepository _PositionsRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreatePositionsRangeCommandHandler(IPositionRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _PositionsRepository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreatePositionRangeCommand request, CancellationToken cancellationToken)
        {
            var positions = _mapper.Map<List<Position>>(request);
            await _PositionsRepository.InsertRangeAsync(positions);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success("range added successfully");
        }
    }
}

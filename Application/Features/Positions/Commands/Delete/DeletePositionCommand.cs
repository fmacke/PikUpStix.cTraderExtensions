using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;

namespace Application.Features.Positions.Commands.Delete
{
    public class DeletePositionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteDynamicTestTradesCommandHandler : IRequestHandler<DeletePositionCommand, Result<int>>
        {
            private readonly IPositionRepository _testTradesRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDynamicTestTradesCommandHandler(IPositionRepository respository, IUnitOfWork unitOfWork)
            {
                _testTradesRepository = respository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeletePositionCommand command, CancellationToken cancellationToken)
            {
                var testTrades = await _testTradesRepository.GetByIdAsync(command.Id);                
                await _testTradesRepository.DeleteAsync(testTrades);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testTrades.Id);
            }
        }
    }
}
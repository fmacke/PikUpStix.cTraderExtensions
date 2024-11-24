using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;

namespace Application.Features.CVRs.Commands.Delete
{
    public class DeleteCVRCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteDynamicCVRCommandHandler : IRequestHandler<DeleteCVRCommand, Result<int>>
        {
            private readonly ICVRRepository _cvrRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDynamicCVRCommandHandler(ICVRRepository cvrRepository, IUnitOfWork unitOfWork)
            {
                _cvrRepository = cvrRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteCVRCommand command, CancellationToken cancellationToken)
            {
                var cvr = await _cvrRepository.GetByIdAsync(command.Id);
                cvr.IsDeleted = true;
                await _cvrRepository.UpdateAsync(cvr);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(cvr.Id);
            }
        }
    }
}
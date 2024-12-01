using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;

namespace Application.Features.Tests.Commands.Delete
{
    public class DeleteTestParametersCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteDynamicTestCommandHandler : IRequestHandler<DeleteTestParametersCommand, Result<int>>
        {
            private readonly ITestRepository _testRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDynamicTestCommandHandler(ITestRepository testRepository, IUnitOfWork unitOfWork)
            {
                _testRepository = testRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteTestParametersCommand command, CancellationToken cancellationToken)
            {
                var test = await _testRepository.GetByIdAsync(command.Id);                
                await _testRepository.DeleteAsync(test);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(test.Id);
            }
        }
    }
}
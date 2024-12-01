using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;

namespace Application.Features.TestParameters.Commands.Delete
{
    public class DeleteTestParametersParametersCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteDynamicTestParametersCommandHandler : IRequestHandler<DeleteTestParametersParametersCommand, Result<int>>
        {
            private readonly ITestParametersRepository _testParametersRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDynamicTestParametersCommandHandler(ITestParametersRepository testParametersRepository, IUnitOfWork unitOfWork)
            {
                _testParametersRepository = testParametersRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteTestParametersParametersCommand command, CancellationToken cancellationToken)
            {
                var testParameters = await _testParametersRepository.GetByIdAsync(command.Id);                
                await _testParametersRepository.DeleteAsync(testParameters);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testParameters.Id);
            }
        }
    }
}
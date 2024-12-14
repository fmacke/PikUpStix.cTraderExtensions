using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;


namespace Application.Features.TestParameters.Commands.Create
{
    public partial class CreateTestParameterCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
    public class CreateTestParametersCommandHandler : IRequestHandler<CreateTestParameterCommand, Result<int>>
    {
        private readonly ITestParametersRepository _testParametersRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateTestParametersCommandHandler(ITestParametersRepository testParametersRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _testParametersRepository = testParametersRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateTestParameterCommand request, CancellationToken cancellationToken)
        {
            var testParameters = _mapper.Map<Test_Parameter>(request);
            await _testParametersRepository.InsertAsync(testParameters);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(testParameters.Id);
        }

    }
}

using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using AutoMapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Features.TestParameters.Commands.Update
{
    public class UpdateTestParametersCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int TestId { get; set; }
    }
    
    public class UpdateTestParametersCommandHandler : IRequestHandler<UpdateTestParametersCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITestParametersRepository _testParametersRepository;
        private readonly IMapper _mapper;

        public UpdateTestParametersCommandHandler(ITestParametersRepository testParametersRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _testParametersRepository = testParametersRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateTestParametersCommand command, CancellationToken cancellationToken)
        {
            var testParameters = await _testParametersRepository.GetByIdAsync(command.Id);

            if (testParameters == null)
            {
                return Result<int>.Fail($"TestParameters Not Found.");
            }
            else
            {
                UpdateModifiedTestParameters(ref testParameters, command);
                await _testParametersRepository.UpdateAsync(testParameters);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(testParameters.Id);
            }
        }

        private void UpdateModifiedTestParameters(ref Test_Parameter testParameters, UpdateTestParametersCommand command)
        {
            testParameters.Name = command.Name ?? testParameters.Name;
            testParameters.Value = command.Value ?? testParameters.Value;
        }
    }
}
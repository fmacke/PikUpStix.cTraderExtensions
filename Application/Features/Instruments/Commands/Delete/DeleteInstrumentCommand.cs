using MediatR;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Interfaces.Repositories;

namespace Application.Features.Instruments.Commands.Delete
{
    public class DeleteInstrumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteDynamicInstrumentCommandHandler : IRequestHandler<DeleteInstrumentCommand, Result<int>>
        {
            private readonly IInstrumentRepository _instrumentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDynamicInstrumentCommandHandler(IInstrumentRepository instrumentRepository, IUnitOfWork unitOfWork)
            {
                _instrumentRepository = instrumentRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteInstrumentCommand command, CancellationToken cancellationToken)
            {
                var instrument = await _instrumentRepository.GetByIdAsync(command.Id);
                await _instrumentRepository.UpdateAsync(instrument);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(instrument.Id);
            }
        }
    }
}
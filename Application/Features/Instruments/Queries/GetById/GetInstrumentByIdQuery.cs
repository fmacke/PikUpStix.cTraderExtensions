using Application.Interfaces.CacheRepositories;
using Application.Common.Results;
using AutoMapper;
using MediatR;

namespace Application.Features.Instruments.Queries.GetById
{
    public class GetInstrumentByIdQuery : IRequest<Result<GetInstrumentByIdResponse>>
    {
        public int Id { get; set; }

        public class GetInstrumentByIdQueryHandler : IRequestHandler<GetInstrumentByIdQuery, Result<GetInstrumentByIdResponse>>
        {
            private readonly IInstrumentCacheRepository _cVRCache;
            private readonly IMapper _mapper;

            public GetInstrumentByIdQueryHandler(IInstrumentCacheRepository cVRCache, IMapper mapper)
            {
                _cVRCache = cVRCache;
                _mapper = mapper;
            }

            public async Task<Result<GetInstrumentByIdResponse>> Handle(GetInstrumentByIdQuery query, CancellationToken cancellationToken)
            {
                var cVR = await _cVRCache.GetByIdAsync(query.Id);
                var mappedInstrument = _mapper.Map<GetInstrumentByIdResponse>(cVR);
                return Result<GetInstrumentByIdResponse>.Success(mappedInstrument);
            }
        }
    }
}

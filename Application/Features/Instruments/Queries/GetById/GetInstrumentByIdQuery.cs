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
            private readonly IInstrumentCacheRepository _instrumentCache;
            private readonly IMapper _mapper;

            public GetInstrumentByIdQueryHandler(IInstrumentCacheRepository instrumentCache, IMapper mapper)
            {
                _instrumentCache = instrumentCache;
                _mapper = mapper;
            }

            public async Task<Result<GetInstrumentByIdResponse>> Handle(GetInstrumentByIdQuery query, CancellationToken cancellationToken)
            {
                var instrument = await _instrumentCache.GetByIdAsync(query.Id);
                var mappedInstrument = _mapper.Map<GetInstrumentByIdResponse>(instrument);
                return Result<GetInstrumentByIdResponse>.Success(mappedInstrument);
            }
        }
    }
}

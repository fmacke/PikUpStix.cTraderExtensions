using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.Instruments.Queries.GetAllCached
{
    public class GetAllInstrumentsCachedQuery : IRequest<Result<List<GetAllInstrumentsCachedResponse>>>
    {
        public GetAllInstrumentsCachedQuery()
        {
        }
    }

    public class GetAllInstrumentCachedQueryHandler : IRequestHandler<GetAllInstrumentsCachedQuery, Result<List<GetAllInstrumentsCachedResponse>>>
    {
        private readonly IInstrumentCacheRepository _instrumentCache;
        private readonly IMapper _mapper;

        public GetAllInstrumentCachedQueryHandler(IInstrumentCacheRepository instrumentCache, IMapper mapper)
        {
            _instrumentCache = instrumentCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllInstrumentsCachedResponse>>> Handle(GetAllInstrumentsCachedQuery request, CancellationToken cancellationToken)
        {
            var instrumentList = await _instrumentCache.GetCachedListAsync();
            var mappedDynamicInstrument = _mapper.Map<List<GetAllInstrumentsCachedResponse>>(instrumentList);
            return Result<List<GetAllInstrumentsCachedResponse>>.Success(mappedDynamicInstrument);
        }
    }
}
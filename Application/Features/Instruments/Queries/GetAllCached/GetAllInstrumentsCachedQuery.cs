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
        private readonly IInstrumentCacheRepository _cVRCache;
        private readonly IMapper _mapper;

        public GetAllInstrumentCachedQueryHandler(IInstrumentCacheRepository cVRCache, IMapper mapper)
        {
            _cVRCache = cVRCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllInstrumentsCachedResponse>>> Handle(GetAllInstrumentsCachedQuery request, CancellationToken cancellationToken)
        {
            var cVRList = await _cVRCache.GetCachedListAsync();
            var mappedDynamicInstrument = _mapper.Map<List<GetAllInstrumentsCachedResponse>>(cVRList);
            return Result<List<GetAllInstrumentsCachedResponse>>.Success(mappedDynamicInstrument);
        }
    }
}
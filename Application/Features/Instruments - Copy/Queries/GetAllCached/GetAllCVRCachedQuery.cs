using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.CVRs.Queries.GetAllCached
{
    public class GetAllCVRCachedQuery : IRequest<Result<List<GetAllCVRCachedResponse>>>
    {
        public GetAllCVRCachedQuery()
        {
        }
    }

    public class GetAllCVRCachedQueryHandler : IRequestHandler<GetAllCVRCachedQuery, Result<List<GetAllCVRCachedResponse>>>
    {
        private readonly ICVRCacheRepository _cVRCache;
        private readonly IMapper _mapper;

        public GetAllCVRCachedQueryHandler(ICVRCacheRepository cVRCache, IMapper mapper)
        {
            _cVRCache = cVRCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCVRCachedResponse>>> Handle(GetAllCVRCachedQuery request, CancellationToken cancellationToken)
        {
            var cVRList = await _cVRCache.GetCachedListAsync();
            var mappedDynamicCVR = _mapper.Map<List<GetAllCVRCachedResponse>>(cVRList);
            return Result<List<GetAllCVRCachedResponse>>.Success(mappedDynamicCVR);
        }
    }
}
using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.Tests.Queries.GetAllCached
{
    public class GetAllTestsCachedQuery : IRequest<Result<List<GetAllTestsCachedResponse>>>
    {
        public GetAllTestsCachedQuery()
        {
        }
    }

    public class GetAllTestCachedQueryHandler : IRequestHandler<GetAllTestsCachedQuery, Result<List<GetAllTestsCachedResponse>>>
    {
        private readonly ITestCacheRepository _cVRCache;
        private readonly IMapper _mapper;

        public GetAllTestCachedQueryHandler(ITestCacheRepository cVRCache, IMapper mapper)
        {
            _cVRCache = cVRCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllTestsCachedResponse>>> Handle(GetAllTestsCachedQuery request, CancellationToken cancellationToken)
        {
            var cVRList = await _cVRCache.GetCachedListAsync();
            var mappedDynamicTest = _mapper.Map<List<GetAllTestsCachedResponse>>(cVRList);
            return Result<List<GetAllTestsCachedResponse>>.Success(mappedDynamicTest);
        }
    }
}
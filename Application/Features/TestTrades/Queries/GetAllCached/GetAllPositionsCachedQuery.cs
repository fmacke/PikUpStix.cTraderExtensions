using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.TestTrades.Queries.GetAllCached
{
    public class GetAllPositionsCachedQuery : IRequest<Result<List<GetAllPositionsCachedResponse>>>
    {
        public GetAllPositionsCachedQuery()
        {
        }
    }

    public class GetAllTestTradesCachedQueryHandler : IRequestHandler<GetAllPositionsCachedQuery, Result<List<GetAllPositionsCachedResponse>>>
    {
        private readonly IPositionCacheRepository _TestTradesCache;
        private readonly IMapper _mapper;

        public GetAllTestTradesCachedQueryHandler(IPositionCacheRepository TestTradesCache, IMapper mapper)
        {
            _TestTradesCache = TestTradesCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllPositionsCachedResponse>>> Handle(GetAllPositionsCachedQuery request, CancellationToken cancellationToken)
        {
            var testTradesList = await _TestTradesCache.GetCachedListAsync();
            var mappedDynamicTestTrades = _mapper.Map<List<GetAllPositionsCachedResponse>>(testTradesList);
            return Result<List<GetAllPositionsCachedResponse>>.Success(mappedDynamicTestTrades);
        }
    }
}
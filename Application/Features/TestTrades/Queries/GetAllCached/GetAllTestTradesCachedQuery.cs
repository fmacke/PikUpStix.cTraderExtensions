using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.TestTrades.Queries.GetAllCached
{
    public class GetAllTestTradesCachedQuery : IRequest<Result<List<GetAllTestTradesCachedResponse>>>
    {
        public GetAllTestTradesCachedQuery()
        {
        }
    }

    public class GetAllTestTradesCachedQueryHandler : IRequestHandler<GetAllTestTradesCachedQuery, Result<List<GetAllTestTradesCachedResponse>>>
    {
        private readonly ITestTradeCacheRepository _TestTradesCache;
        private readonly IMapper _mapper;

        public GetAllTestTradesCachedQueryHandler(ITestTradeCacheRepository TestTradesCache, IMapper mapper)
        {
            _TestTradesCache = TestTradesCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllTestTradesCachedResponse>>> Handle(GetAllTestTradesCachedQuery request, CancellationToken cancellationToken)
        {
            var testTradesList = await _TestTradesCache.GetCachedListAsync();
            var mappedDynamicTestTrades = _mapper.Map<List<GetAllTestTradesCachedResponse>>(testTradesList);
            return Result<List<GetAllTestTradesCachedResponse>>.Success(mappedDynamicTestTrades);
        }
    }
}
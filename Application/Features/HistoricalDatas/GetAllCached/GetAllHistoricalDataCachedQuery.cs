using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.HistoricalDatas.GetAllCached
{
    public class GetAllHistoricalDataCachedQuery : IRequest<Result<List<GetAllHistoricalDataCachedResponse>>>
    {
        public GetAllHistoricalDataCachedQuery()
        {
        }
    }

    public class GetAllHistoricalDataCachedQueryHandler : IRequestHandler<GetAllHistoricalDataCachedQuery, Result<List<GetAllHistoricalDataCachedResponse>>>
    {
        private readonly IHistoricalDataCacheRepository _historicalDataCache;
        private readonly IMapper _mapper;

        public GetAllHistoricalDataCachedQueryHandler(IHistoricalDataCacheRepository historicalDataCache, IMapper mapper)
        {
            _historicalDataCache = historicalDataCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllHistoricalDataCachedResponse>>> Handle(GetAllHistoricalDataCachedQuery request, CancellationToken cancellationToken)
        {
            var historicalDataList = await _historicalDataCache.GetCachedListAsync();
            var mappedDynamicHistoricalData = _mapper.Map<List<GetAllHistoricalDataCachedResponse>>(historicalDataList);
            return Result<List<GetAllHistoricalDataCachedResponse>>.Success(mappedDynamicHistoricalData);
        }
    }
}
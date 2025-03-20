using Application.Common.Results;
using Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Positions.Queries.GetById
{
    public class GetPositionByIdQuery : IRequest<Result<GetPositionByIdResponse>>
    {
        public int Id { get; set; }

        public class GetTestTradeByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, Result<GetPositionByIdResponse>>
        {
            private readonly IPositionCacheRepository _testTradeCache;
            private readonly IMapper _mapper;

            public GetTestTradeByIdQueryHandler(IPositionCacheRepository testTradeCache, IMapper mapper)
            {
                _testTradeCache = testTradeCache;
                _mapper = mapper;
            }

            public async Task<Result<GetPositionByIdResponse>> Handle(GetPositionByIdQuery query, CancellationToken cancellationToken)
            {
                var testTrade = await _testTradeCache.GetByIdAsync(query.Id);
                var mappedTestTrades = _mapper.Map<GetPositionByIdResponse>(testTrade);
                return Result<GetPositionByIdResponse>.Success(mappedTestTrades);
            }
        }
    }
}

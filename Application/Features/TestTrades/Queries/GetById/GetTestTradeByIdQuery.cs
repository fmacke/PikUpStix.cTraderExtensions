using Application.Common.Results;
using Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;

namespace Application.Features.TestTrades.Queries.GetById
{
    public class GetTestTradeByIdQuery : IRequest<Result<GetTestTradeByIdResponse>>
    {
        public int Id { get; set; }

        public class GetTestTradeByIdQueryHandler : IRequestHandler<GetTestTradeByIdQuery, Result<GetTestTradeByIdResponse>>
        {
            private readonly ITestTradeCacheRepository _testTradeCache;
            private readonly IMapper _mapper;

            public GetTestTradeByIdQueryHandler(ITestTradeCacheRepository testTradeCache, IMapper mapper)
            {
                _testTradeCache = testTradeCache;
                _mapper = mapper;
            }

            public async Task<Result<GetTestTradeByIdResponse>> Handle(GetTestTradeByIdQuery query, CancellationToken cancellationToken)
            {
                var testTrade = await _testTradeCache.GetByIdAsync(query.Id);
                var mappedTestTrades = _mapper.Map<GetTestTradeByIdResponse>(testTrade);
                return Result<GetTestTradeByIdResponse>.Success(mappedTestTrades);
            }
        }
    }
}

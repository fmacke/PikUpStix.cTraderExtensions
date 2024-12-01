using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Interfaces.CacheRepositories;

namespace Application.Features.TestParameters.Queries.GetAllCached
{
    public class GetAllTestParametersCachedQuery : IRequest<Result<List<GetAllTestParametersCachedResponse>>>
    {
        public GetAllTestParametersCachedQuery()
        {
        }
    }

    public class GetAllTestParametersCachedQueryHandler : IRequestHandler<GetAllTestParametersCachedQuery, Result<List<GetAllTestParametersCachedResponse>>>
    {
        private readonly ITestParametersCacheRepository _testParametersCache;
        private readonly IMapper _mapper;

        public GetAllTestParametersCachedQueryHandler(ITestParametersCacheRepository testParametersCache, IMapper mapper)
        {
            _testParametersCache = testParametersCache;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllTestParametersCachedResponse>>> Handle(GetAllTestParametersCachedQuery request, CancellationToken cancellationToken)
        {
            var testParametersList = await _testParametersCache.GetCachedListAsync();
            var mappedDynamicTestParameters = _mapper.Map<List<GetAllTestParametersCachedResponse>>(testParametersList);
            return Result<List<GetAllTestParametersCachedResponse>>.Success(mappedDynamicTestParameters);
        }
    }
}
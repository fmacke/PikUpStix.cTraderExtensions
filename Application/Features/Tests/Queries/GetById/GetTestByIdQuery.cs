using Application.Interfaces.CacheRepositories;
using Application.Common.Results;
using AutoMapper;
using MediatR;

namespace Application.Features.Tests.Queries.GetById
{
    public class GetTestByIdQuery : IRequest<Result<GetTestByIdResponse>>
    {
        public int Id { get; set; }

        public class GetTestByIdQueryHandler : IRequestHandler<GetTestByIdQuery, Result<GetTestByIdResponse>>
        {
            private readonly ITestCacheRepository _cVRCache;
            private readonly IMapper _mapper;

            public GetTestByIdQueryHandler(ITestCacheRepository cVRCache, IMapper mapper)
            {
                _cVRCache = cVRCache;
                _mapper = mapper;
            }

            public async Task<Result<GetTestByIdResponse>> Handle(GetTestByIdQuery query, CancellationToken cancellationToken)
            {
                var cVR = await _cVRCache.GetByIdAsync(query.Id);
                var mappedTest = _mapper.Map<GetTestByIdResponse>(cVR);
                return Result<GetTestByIdResponse>.Success(mappedTest);
            }
        }
    }
}

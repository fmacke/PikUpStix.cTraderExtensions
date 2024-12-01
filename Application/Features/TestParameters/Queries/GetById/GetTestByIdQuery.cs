using Application.Interfaces.CacheRepositories;
using Application.Common.Results;
using AutoMapper;
using MediatR;

namespace Application.Features.TestParameters.Queries.GetById
{
    public class GetTestParametersByIdQuery : IRequest<Result<GetTestParametersByIdResponse>>
    {
        public int Id { get; set; }

        public class GetTestParametersByIdQueryHandler : IRequestHandler<GetTestParametersByIdQuery, Result<GetTestParametersByIdResponse>>
        {
            private readonly ITestParametersCacheRepository _cVRCache;
            private readonly IMapper _mapper;

            public GetTestParametersByIdQueryHandler(ITestParametersCacheRepository cVRCache, IMapper mapper)
            {
                _cVRCache = cVRCache;
                _mapper = mapper;
            }

            public async Task<Result<GetTestParametersByIdResponse>> Handle(GetTestParametersByIdQuery query, CancellationToken cancellationToken)
            {
                var cVR = await _cVRCache.GetByIdAsync(query.Id);
                var mappedTestParameters = _mapper.Map<GetTestParametersByIdResponse>(cVR);
                return Result<GetTestParametersByIdResponse>.Success(mappedTestParameters);
            }
        }
    }
}

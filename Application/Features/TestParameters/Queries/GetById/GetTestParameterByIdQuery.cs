using Application.Common.Results;
using Application.Interfaces.CacheRepositories;
using AutoMapper;
using MediatR;

namespace Application.Features.TestParameters.Queries.GetById
{
    public class GetTestParameterByIdQuery : IRequest<Result<GetTestParameterByIdResponse>>
    {
        public int Id { get; set; }

        public class GetTestParameterByIdQueryHandler : IRequestHandler<GetTestParameterByIdQuery, Result<GetTestParameterByIdResponse>>
        {
            private readonly ITestParametersCacheRepository _cVRCache;
            private readonly IMapper _mapper;

            public GetTestParameterByIdQueryHandler(ITestParametersCacheRepository cVRCache, IMapper mapper)
            {
                _cVRCache = cVRCache;
                _mapper = mapper;
            }

            public async Task<Result<GetTestParameterByIdResponse>> Handle(GetTestParameterByIdQuery query, CancellationToken cancellationToken)
            {
                var cVR = await _cVRCache.GetByIdAsync(query.Id);
                var mappedTestParameters = _mapper.Map<GetTestParameterByIdResponse>(cVR);
                return Result<GetTestParameterByIdResponse>.Success(mappedTestParameters);
            }
        }
    }
}

using Application.Interfaces.CacheRepositories;
using Application.Common.Results;
using AutoMapper;
using MediatR;

namespace Application.Features.CVRs.Queries.GetById
{
    public class GetCVRByIdQuery : IRequest<Result<GetCVRByIdResponse>>
    {
        public int Id { get; set; }

        public class GetCVRByIdQueryHandler : IRequestHandler<GetCVRByIdQuery, Result<GetCVRByIdResponse>>
        {
            private readonly ICVRCacheRepository _cVRCache;
            private readonly IMapper _mapper;

            public GetCVRByIdQueryHandler(ICVRCacheRepository cVRCache, IMapper mapper)
            {
                _cVRCache = cVRCache;
                _mapper = mapper;
            }

            public async Task<Result<GetCVRByIdResponse>> Handle(GetCVRByIdQuery query, CancellationToken cancellationToken)
            {
                var cVR = await _cVRCache.GetByIdAsync(query.Id);
                var mappedCVR = _mapper.Map<GetCVRByIdResponse>(cVR);
                return Result<GetCVRByIdResponse>.Success(mappedCVR);
            }
        }
    }
}

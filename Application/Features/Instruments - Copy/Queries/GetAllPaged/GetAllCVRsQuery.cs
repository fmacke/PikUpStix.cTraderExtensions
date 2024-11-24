using MediatR;
using System.Linq.Expressions;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using Domain.Entities.CVRs;
using Application.Common.Extensions;

namespace Application.Features.CVRs.Queries.GetAllPaged
{
    public class GetAllCVRsQuery : IRequest<PaginatedResult<GetAllCVRsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllCVRsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllCVRsQueryHandler : IRequestHandler<GetAllCVRsQuery, PaginatedResult<GetAllCVRsResponse>>
    {
        private readonly ICVRRepository _repository;

        public GetAllCVRsQueryHandler(ICVRRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllCVRsResponse>> Handle(GetAllCVRsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CVR, GetAllCVRsResponse>> expression = e => new GetAllCVRsResponse
            {
                Id = e.Id,
                ContractInfo = e.ContractInfo,
                Input = e.Input,
                PreparedBy = e.PreparedBy,
                PreparedDate = e.PreparedDate,
                ReviewedBy = e.ReviewedBy,
                ReviewedDate = e.ReviewedDate,
                CreatedBy = e.CreatedBy,
                CreatedOn = e.CreatedOn,
                LastModifiedBy = e.LastModifiedBy,
                LastModifiedOn = e.LastModifiedOn
            };
            var paginatedList = await _repository.CVRs
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}
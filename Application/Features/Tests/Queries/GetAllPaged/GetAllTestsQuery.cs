using MediatR;
using System.Linq.Expressions;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using Application.Common.Extensions;
using Domain.Entities;

namespace Application.Features.Tests.Queries.GetAllPaged
{
    public class GetAllTestsQuery : IRequest<PaginatedResult<GetAllTestsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllTestsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllTestsQueryHandler : IRequestHandler<GetAllTestsQuery, PaginatedResult<GetAllTestsResponse>>
    {
        private readonly ITestRepository _repository;

        public GetAllTestsQueryHandler(ITestRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllTestsResponse>> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Test, GetAllTestsResponse>> expression = e => new GetAllTestsResponse
            {
                Id = e.Id
                /// todo: add other properties
            };
            var paginatedList = await _repository.Tests
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}
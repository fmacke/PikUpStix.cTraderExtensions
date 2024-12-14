using MediatR;
using System.Linq.Expressions;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using Application.Common.Extensions;
using Domain.Entities;

namespace Application.Features.TestParameters.Queries.GetAllPaged
{
    public class GetAllTestParametersQuery : IRequest<PaginatedResult<GetAllTestParametersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllTestParametersQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllTestParametersQueryHandler : IRequestHandler<GetAllTestParametersQuery, PaginatedResult<GetAllTestParametersResponse>>
    {
        private readonly ITestParametersRepository _repository;

        public GetAllTestParametersQueryHandler(ITestParametersRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllTestParametersResponse>> Handle(GetAllTestParametersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Test_Parameter, GetAllTestParametersResponse>> expression = e => new GetAllTestParametersResponse
            {
                Id = e.Id
                /// todo: add other properties
            };
            var paginatedList = await _repository.Test_Parameters
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}
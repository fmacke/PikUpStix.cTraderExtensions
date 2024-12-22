using MediatR;
using System.Linq.Expressions;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using Application.Common.Extensions;
using Domain.Entities;

namespace Application.Features.TestTrades.Queries.GetAllPaged
{
    public class GetAllTestTradesQuery : IRequest<PaginatedResult<GetAllTestTradesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllTestTradesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllTestTradesQueryHandler : IRequestHandler<GetAllTestTradesQuery, PaginatedResult<GetAllTestTradesResponse>>
    {
        private readonly ITestTradeRepository _repository;

        public GetAllTestTradesQueryHandler(ITestTradeRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllTestTradesResponse>> Handle(GetAllTestTradesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<TestTrade, GetAllTestTradesResponse>> expression = e => new GetAllTestTradesResponse
            {
                Id = e.Id
                /// todo: add other properties
            };
            var paginatedList = await _repository.TestTrades
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}
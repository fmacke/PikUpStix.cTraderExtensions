using MediatR;
using System.Linq.Expressions;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using Application.Common.Extensions;
using Domain.Entities;

namespace Application.Features.TestTrades.Queries.GetAllPaged
{
    public class GetAllPositionsQuery : IRequest<PaginatedResult<GetAllPositionsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllPositionsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllTestTradesQueryHandler : IRequestHandler<GetAllPositionsQuery, PaginatedResult<GetAllPositionsResponse>>
    {
        private readonly IPositionRepository _repository;

        public GetAllTestTradesQueryHandler(IPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllPositionsResponse>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Position, GetAllPositionsResponse>> expression = e => new GetAllPositionsResponse
            {
                Id = e.Id
                /// todo: add other properties
            };
            var paginatedList = await _repository.Positions
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}